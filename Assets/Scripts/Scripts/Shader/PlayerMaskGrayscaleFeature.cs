// PlayerMaskGrayscaleFeature.cs
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerMaskGrayscaleFeature : ScriptableRendererFeature
{
    [SerializeField] Shader shader;
    [SerializeField, Range(0f, 1f)] float softnessRatio = 0.1f;  // Radius 대비 Softness 비율
    [SerializeField, Range(0f, 2f)] float brightness = 1f;

    class GrayscalePass : ScriptableRenderPass
    {
        static readonly int TempRTId = Shader.PropertyToID("_MaskGrayTemp");
        readonly Material _mat;
        Vector2 _playerUV;
        float _radius, _softness, _brightness;

        public GrayscalePass(Material mat)
        {
            _mat = mat;
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public void Setup(Vector2 playerUV, float radius, float softness, float brightness)
        {
            _playerUV = playerUV;
            _radius = radius;
            _softness = softness;
            _brightness = brightness;
        }

        public override void Execute(ScriptableRenderContext ctx, ref RenderingData data)
        {
            var cmd = CommandBufferPool.Get(nameof(PlayerMaskGrayscaleFeature));
            var src = data.cameraData.renderer.cameraColorTargetHandle;
            var desc = data.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0;
            cmd.GetTemporaryRT(TempRTId, desc, FilterMode.Bilinear);

            cmd.SetGlobalVector("_PlayerPos", new Vector4(_playerUV.x, _playerUV.y, 0, 0));
            cmd.SetGlobalFloat("_Radius", _radius);
            cmd.SetGlobalFloat("_EdgeSoftness", _softness);
            cmd.SetGlobalFloat("_Brightness", _brightness);

            cmd.Blit(src, TempRTId, _mat);
            cmd.Blit(TempRTId, src);

            cmd.ReleaseTemporaryRT(TempRTId);
            ctx.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    Material _mat;
    GrayscalePass _pass;

    public override void Create()
    {
        if (shader == null)
            Debug.LogError("PlayerMaskGrayscaleFeature: Shader가 할당되지 않았습니다!");
        else
        {
            _mat = CoreUtils.CreateEngineMaterial(shader);
            _pass = new GrayscalePass(_mat);
        }
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData data)
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;
        var pauseZone = player.transform.Find("PauseZone");
        if (pauseZone == null) return;
        var circle = pauseZone.GetComponent<CircleCollider2D>();
        if (circle == null) return;

        Camera cam = data.cameraData.camera;
        Vector3 worldPos = pauseZone.position;
        Vector3 vpCenter = cam.WorldToViewportPoint(worldPos);

        // * worldRadius 값은 pauseZone의 CircleCollider2D.radius에 localScale 포함
        float worldRadius = circle.radius * Mathf.Max(pauseZone.lossyScale.x, pauseZone.lossyScale.y);

        // 1) 높이 기준 UV 반경 계산 (Y축 방향)
        Vector3 vpUp = cam.WorldToViewportPoint(worldPos + Vector3.up * worldRadius);
        float radiusUV = Mathf.Abs(vpUp.y - vpCenter.y);

        _pass.Setup(vpCenter, radiusUV, softnessRatio, brightness);
        renderer.EnqueuePass(_pass);
    }
}
