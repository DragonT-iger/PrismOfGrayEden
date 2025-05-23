// Assets/Editor/CanvasGuideEditor.cs
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class CanvasGuideEditor
{
    static bool _debugMode;
    static List<Vector3> _points = new List<Vector3>();
    static bool _isDragging;
    static Vector3 _dragStartWorld;
    static List<Vector3> _origPoints = new List<Vector3>();

    static CanvasGuideEditor()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sv)
    {
        var e = Event.current;

        // 1) Ctrl+R ���
        if ((e.control || e.command) && e.type == EventType.KeyDown && e.keyCode == KeyCode.R)
        {
            _debugMode = !_debugMode;
            e.Use();
        }
        if (!_debugMode) return;

        // 2) ã�� ĵ����
        var canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null) return;
        var rt = canvas.GetComponent<RectTransform>();
        var tr = canvas.transform;

        // 3) ���콺 �� ���� ���(ray �� plane)
        var cam = sv.camera;
        var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        var plane = new Plane(tr.forward, tr.position);
        if (!plane.Raycast(ray, out float enter)) return;
        var worldPos = ray.GetPoint(enter);

        // 4) GUI�� ��ǥ ǥ��
        Handles.BeginGUI();
        GUILayout.Window(67890, new Rect(10, 10, 200, 60), id =>
        {
            GUILayout.Label($"Canvas World:\nX:{worldPos.x:0.00}\nY:{worldPos.y:0.00}\nZ:{worldPos.z:0.00}");
            GUILayout.Label("LClick: Add  RClick: Remove");
            GUILayout.Label("Shift+Drag: Move All  Del: Clear");
        }, "GuideTool");
        Handles.EndGUI();

        // 5) Shift+Ŭ�� �巡�� �� ��ü �̵�
        if (e.shift && !_isDragging && e.type == EventType.MouseDown && e.button == 0)
        {
            _isDragging = true;
            _dragStartWorld = worldPos;
            _origPoints = new List<Vector3>(_points);
            e.Use();
        }
        if (_isDragging && e.type == EventType.MouseDrag && e.button == 0)
        {
            // �ٽ� ray��plane
            if (plane.Raycast(ray, out float enter2))
            {
                var pos2 = ray.GetPoint(enter2);
                var delta = pos2 - _dragStartWorld;
                for (int i = 0; i < _points.Count; i++)
                    _points[i] = _origPoints[i] + delta;
            }
            e.Use();
        }
        if (_isDragging && e.type == EventType.MouseUp && e.button == 0)
        {
            _isDragging = false;
            e.Use();
        }

        // 6) ��Ŭ�� �� �߰�, ��Ŭ�� �� ������ ����, Del �� ��ü ����
        if (!e.shift && e.type == EventType.MouseDown && e.button == 0)
        {
            _points.Add(worldPos);
            e.Use();
        }
        if (e.type == EventType.MouseDown && e.button == 1 && _points.Count > 0)
        {
            _points.RemoveAt(_points.Count - 1);
            e.Use();
        }
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete)
        {
            _points.Clear();
            e.Use();
        }

        // 7) ���̵���� �׸��� (ĵ���� ��ü ���� ũ�� ����)
        float halfW = rt.rect.width * tr.lossyScale.x * 0.5f;
        float halfH = rt.rect.height * tr.lossyScale.y * 0.5f;
        var rightDir = tr.right;
        var upDir = tr.up;
        Handles.color = Color.red;
        foreach (var p in _points)
        {
            // ������
            Handles.DrawLine(p - upDir * halfH, p + upDir * halfH);
            // ����
            Handles.DrawLine(p - rightDir * halfW, p + rightDir * halfW);
        }

        sv.Repaint();
    }
}
