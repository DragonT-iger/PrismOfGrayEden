using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerMaskGrayscaleFeature : ScriptableRendererFeature
{
    class GrayscalePass : ScriptableRenderPass
    {
        static readonly int TempRTId = Shader.PropertyToID("_MaskGrayTemp");
        readonly Material mat;
        Vector2 playerUV;
        float radiusUV;

        public GrayscalePass(Material m)
        {
            mat = m;
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public void Setup(Vector2 uv, float r)
        {
            playerUV = uv;
            radiusUV = r;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor desc)
        {
            ConfigureInput(ScriptableRenderPassInput.Color);
        }

        public override void Execute(ScriptableRenderContext ctx, ref RenderingData data)
        {
            var cmd = CommandBufferPool.Get("MaskGrayscale");
            RTHandle src = data.cameraData.renderer.cameraColorTargetHandle;

            var desc = data.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0;
            cmd.GetTemporaryRT(TempRTId, desc, FilterMode.Bilinear);

            cmd.SetGlobalVector("_PlayerPos", new Vector4(playerUV.x, playerUV.y, 0, 0));
            cmd.SetGlobalFloat("_Radius", radiusUV);

            cmd.Blit(src, TempRTId, mat);
            cmd.Blit(TempRTId, src);

            cmd.ReleaseTemporaryRT(TempRTId);
            ctx.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    [SerializeField] Shader shader;

    Material mat;
    GrayscalePass pass;

    public override void Create()
    {
        mat = CoreUtils.CreateEngineMaterial(shader);
        pass = new GrayscalePass(mat);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData data)
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;

        // PauseZone 자식에서 콜라이더 찾기
        var pauseZone = player.transform.Find("PauseZone");
        if (pauseZone == null) return;

        var circle = pauseZone.GetComponent<CircleCollider2D>();
        if (circle == null) {
            Debug.Log("CircleCollider2D 가 없습니다");
            return;
        }
        

        Camera cam = data.cameraData.camera;
        Vector3 pos = pauseZone.position;

        // 로컬 radius * 트랜스폼 스케일 → 월드 단위 반경
        float worldRadius = circle.radius *
            Mathf.Max(pauseZone.lossyScale.x, pauseZone.lossyScale.y);

        // 뷰포트 공간 변환
        Vector3 vpCenter = cam.WorldToViewportPoint(pos);
        Vector3 vpOffset = cam.WorldToViewportPoint(pos + Vector3.right * worldRadius);
        float radiusUV = Mathf.Abs(vpOffset.x - vpCenter.x);

        pass.Setup(new Vector2(vpCenter.x, vpCenter.y), radiusUV);
        renderer.EnqueuePass(pass);
    }

}
