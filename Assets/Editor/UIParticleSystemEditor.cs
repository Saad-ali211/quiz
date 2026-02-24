// Place this file in:  Assets/Editor/UIParticleSystemEditor.cs
// The Editor/ folder makes Unity compile it only for the editor, not in builds.

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIParticleSystem))]
public class UIParticleSystemEditor : Editor
{
    // ── Foldout states ────────────────────────────────────────────────────────
    private bool _foldMain      = true;
    private bool _foldEmission  = true;
    private bool _foldBurst     = true;
    private bool _foldShape     = true;
    private bool _foldColor     = true;
    private bool _foldSize      = true;
    private bool _foldVelocity  = false;
    private bool _foldRotation  = true;
    private bool _foldRenderer  = false;
    private bool _foldPresets   = false;

    // ── Styles (lazily initialized) ───────────────────────────────────────────
    private GUIStyle _headerStyle;
    private GUIStyle _labelStyle;
    private GUIStyle _sectionStyle;

    private static readonly Color _playColor   = new Color(0.35f, 0.85f, 0.45f);
    private static readonly Color _pauseColor  = new Color(0.9f,  0.75f, 0.2f);
    private static readonly Color _stopColor   = new Color(0.9f,  0.35f, 0.35f);
    private static readonly Color _burstColor  = new Color(0.45f, 0.65f, 1f);
    private static readonly Color _headerColor = new Color(0.18f, 0.18f, 0.22f);
    private static readonly Color _accentColor = new Color(0.4f,  0.7f,  1f);

    // ─────────────────────────────────────────────────────────────────────────
    // INSPECTOR GUI
    // ─────────────────────────────────────────────────────────────────────────
    public override void OnInspectorGUI()
    {
        InitStyles();

        UIParticleSystem ps = (UIParticleSystem)target;
        serializedObject.Update();

        DrawHeader(ps);
        DrawPreviewControls(ps);
        DrawStats(ps);

        EditorGUILayout.Space(4);

        DrawSection("⚙  Main Module",       ref _foldMain,     () => DrawMainModule());
        DrawSection("📡 Emission",           ref _foldEmission, () => DrawEmissionModule());
        DrawSection("💥 Burst",              ref _foldBurst,    () => DrawBurstModule(ps));
        DrawSection("🔷 Shape",              ref _foldShape,    () => DrawShapeModule());
        DrawSection("🎨 Color over Lifetime",ref _foldColor,    () => DrawColorModule());
        DrawSection("📐 Size over Lifetime", ref _foldSize,     () => DrawSizeModule());
        DrawSection("🌀 Velocity over Lifetime", ref _foldVelocity, () => DrawVelocityModule());
        DrawSection("🔄 Rotation",           ref _foldRotation, () => DrawRotationModule());
        DrawSection("🖼  Renderer",           ref _foldRenderer, () => DrawRendererModule());
        DrawSection("📦 Presets",            ref _foldPresets,  () => DrawPresets(ps));

        serializedObject.ApplyModifiedProperties();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // HEADER
    // ─────────────────────────────────────────────────────────────────────────
    private void DrawHeader(UIParticleSystem ps)
    {
        var rect = GUILayoutUtility.GetRect(0, 42, GUILayout.ExpandWidth(true));
        EditorGUI.DrawRect(rect, _headerColor);

        var titleRect = new Rect(rect.x + 10, rect.y + 6, rect.width - 20, 18);
        GUI.Label(titleRect, "UI Particle System", _headerStyle);

        var subRect = new Rect(rect.x + 10, rect.y + 24, rect.width - 20, 13);
        GUI.Label(subRect, "Canvas → Screen Space Overlay", _labelStyle);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PREVIEW CONTROLS
    // ─────────────────────────────────────────────────────────────────────────
    private void DrawPreviewControls(UIParticleSystem ps)
    {
        EditorGUILayout.Space(6);

        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();

            // ── PLAY ──────────────────────────────────────────────────────
            bool isPlaying = ps.PreviewPlaying && !ps.PreviewPaused;
            using (ColorScope(_playColor, isPlaying ? 1f : 0.55f))
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("PlayButton"),
                    GUILayout.Width(36), GUILayout.Height(28)))
                {
                    ps.EditorPlay();
                    EditorUtility.SetDirty(ps);
                }
            }

            // ── PAUSE ─────────────────────────────────────────────────────
            bool isPaused = ps.PreviewPaused;
            using (ColorScope(_pauseColor, isPaused ? 1f : 0.55f))
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("PauseButton"),
                    GUILayout.Width(36), GUILayout.Height(28)))
                {
                    ps.EditorPause();
                    EditorUtility.SetDirty(ps);
                }
            }

            // ── STOP ──────────────────────────────────────────────────────
            using (ColorScope(_stopColor, ps.PreviewPlaying ? 1f : 0.55f))
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("PreMatQuad"),
                    GUILayout.Width(36), GUILayout.Height(28)))
                {
                    ps.EditorStop();
                    EditorUtility.SetDirty(ps);
                }
            }

            GUILayout.Space(8);

            // ── BURST ─────────────────────────────────────────────────────
            using (ColorScope(_burstColor, 1f))
            {
                if (GUILayout.Button("⚡ Burst", GUILayout.Width(70), GUILayout.Height(28)))
                {
                    if (!ps.PreviewPlaying) ps.EditorPlay();
                    ps.EmitBurst();
                    EditorUtility.SetDirty(ps);
                }
            }

            GUILayout.FlexibleSpace();
        }

        EditorGUILayout.Space(4);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // STATS BAR
    // ─────────────────────────────────────────────────────────────────────────
    private void DrawStats(UIParticleSystem ps)
    {
        var rect = GUILayoutUtility.GetRect(0, 22, GUILayout.ExpandWidth(true));
        EditorGUI.DrawRect(rect, new Color(0.12f, 0.12f, 0.15f));

        string state = !ps.PreviewPlaying ? "Stopped" : ps.PreviewPaused ? "Paused" : "Playing";
        Color  stateColor = !ps.PreviewPlaying ? Color.gray : ps.PreviewPaused ? Color.yellow : _playColor;

        var oldColor = GUI.contentColor;
        GUI.contentColor = stateColor;
        GUI.Label(new Rect(rect.x + 8, rect.y + 4, 70, 16), $"● {state}", EditorStyles.miniLabel);
        GUI.contentColor = oldColor;

        GUI.Label(new Rect(rect.x + 85, rect.y + 4, 130, 16),
            $"Particles: {ps.ParticleCount}", EditorStyles.miniLabel);

        GUI.Label(new Rect(rect.x + 210, rect.y + 4, 120, 16),
            $"Time: {ps.PreviewTime:F1}s", EditorStyles.miniLabel);

        // Repaint while playing so stats update live
        if (ps.PreviewPlaying && !ps.PreviewPaused)
            Repaint();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // MODULE DRAWERS
    // ─────────────────────────────────────────────────────────────────────────
    private void DrawMainModule()
    {
        PropField("playOnAwake",     "Play on Awake");
        PropField("loop",            "Loop");
        PropField("duration",        "Duration");
        PropField("startLifetime",   "Start Lifetime");
        PropField("startSpeed",      "Start Speed");
        PropField("startSize",       "Start Size");
        PropField("gravityModifier", "Gravity Modifier");
        PropField("maxParticles",    "Max Particles");
    }

    private void DrawEmissionModule()
    {
        PropField("emission",    "Enabled");
        PropField("rateOverTime","Rate over Time");
    }

    private void DrawBurstModule(UIParticleSystem ps)
    {
        PropField("burstOnPlay",        "Burst on Play");
        PropField("burstCount",         "Burst Count");
        PropField("burstSpeed",         "Burst Speed");
        PropField("burstUpwardBias",    "Upward Bias");
        PropField("burstSpawnRadius",   "Spawn Radius");
        PropField("burstRepeatInterval","Repeat Interval");

        EditorGUILayout.Space(4);
        using (ColorScope(_burstColor, 1f))
        {
            if (GUILayout.Button("⚡  Fire Burst Now", GUILayout.Height(26)))
            {
                if (!ps.PreviewPlaying) ps.EditorPlay();
                ps.EmitBurst();
                EditorUtility.SetDirty(ps);
            }
        }
    }

    private void DrawShapeModule()
    {
        PropField("shape",  "Shape");
        PropField("angle",  "Angle / Size");
        PropField("radius", "Spawn Radius");
    }

    private void DrawColorModule()
    {
        PropField("colorOverLifetime", "Enabled");
        PropField("colorGradient",     "Color Gradient");
    }

    private void DrawSizeModule()
    {
        PropField("sizeOverLifetime", "Enabled");
        PropField("sizeCurve",        "Size Curve");
    }

    private void DrawVelocityModule()
    {
        PropField("velocityOverLifetime", "Enabled");
        PropField("constantForce",        "Constant Force");
        PropField("turbulenceStrength",   "Turbulence");
        PropField("dampingOverTime",      "Damping");
    }

    private void DrawRotationModule()
    {
        PropField("rotationOverLifetime",    "Enabled");
        PropField("rotationSpeed",           "Rotation Speed");
        PropField("randomRotationDirection", "Random Direction");
    }

    private void DrawRendererModule()
    {
        PropField("particleSprite",   "Particle Sprite");
        PropField("particleMaterial", "Material");
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PRESETS
    // ─────────────────────────────────────────────────────────────────────────
    private void DrawPresets(UIParticleSystem ps)
    {
        EditorGUILayout.HelpBox("Presets overwrite current settings and restart the preview.", MessageType.None);
        EditorGUILayout.Space(2);

        using (new EditorGUILayout.HorizontalScope())
        {
            PresetButton("🔥 Fire",     () => { UIParticleSystemPresets.ApplyFire(ps);     Restart(ps); });
            PresetButton("✨ Stars",    () => { UIParticleSystemPresets.ApplyStars(ps);    Restart(ps); });
            PresetButton("💨 Smoke",   () => { UIParticleSystemPresets.ApplySmoke(ps);    Restart(ps); });
        }
        using (new EditorGUILayout.HorizontalScope())
        {
            PresetButton("🔮 Magic",   () => { UIParticleSystemPresets.ApplyMagic(ps);    Restart(ps); });
            PresetButton("🌧 Rain",    () => { UIParticleSystemPresets.ApplyRain(ps);     Restart(ps); });
            PresetButton("🎊 Confetti",() => {
                UIParticleSystemPresets.ApplyConfetti(ps);
                ps.EditorPlay();
                ps.EmitBurst();
                EditorUtility.SetDirty(ps);
            });
        }
    }

    private void PresetButton(string label, System.Action action)
    {
        if (GUILayout.Button(label, GUILayout.Height(26)))
        {
            action();
            serializedObject.Update(); // refresh Inspector to show new values
            Repaint();
        }
    }

    private void Restart(UIParticleSystem ps)
    {
        ps.Clear();
        ps.EditorPlay();
        EditorUtility.SetDirty(ps);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // SECTION HELPER
    // ─────────────────────────────────────────────────────────────────────────
    private void DrawSection(string title, ref bool foldout, System.Action drawBody)
    {
        var rect = GUILayoutUtility.GetRect(0, 24, GUILayout.ExpandWidth(true));
        EditorGUI.DrawRect(rect, new Color(0.22f, 0.22f, 0.27f));

        var arrowRect  = new Rect(rect.x + 6,  rect.y + 5, 14, 14);
        var titleRect  = new Rect(rect.x + 22, rect.y + 4, rect.width - 28, 16);

        var oldColor = GUI.contentColor;
        GUI.contentColor = _accentColor;
        GUI.Label(arrowRect, foldout ? "▼" : "▶", EditorStyles.miniLabel);
        GUI.contentColor = Color.white;
        GUI.Label(titleRect, title, _sectionStyle);
        GUI.contentColor = oldColor;

        if (GUI.Button(rect, GUIContent.none, GUIStyle.none))
            foldout = !foldout;

        if (foldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.Space(3);
            drawBody();
            EditorGUILayout.Space(4);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(1);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // HELPERS
    // ─────────────────────────────────────────────────────────────────────────
    private void PropField(string propName, string label)
    {
        var prop = serializedObject.FindProperty(propName);
        if (prop != null)
            EditorGUILayout.PropertyField(prop, new GUIContent(label));
    }

    private System.IDisposable ColorScope(Color color, float alpha = 1f)
    {
        return new GUIColorScope(new Color(color.r, color.g, color.b, alpha));
    }

    private void InitStyles()
    {
        if (_headerStyle != null) return;

        _headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize  = 14,
            fontStyle = FontStyle.Bold
        };
        _headerStyle.normal.textColor = Color.white;

        _labelStyle = new GUIStyle(EditorStyles.miniLabel);
        _labelStyle.normal.textColor = new Color(0.7f, 0.7f, 0.8f);

        _sectionStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize  = 11,
            fontStyle = FontStyle.Bold
        };
        _sectionStyle.normal.textColor = Color.white;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GUI COLOR SCOPE  (IDisposable wrapper for GUI.color)
    // ─────────────────────────────────────────────────────────────────────────
    private class GUIColorScope : System.IDisposable
    {
        private readonly Color _prev;
        public GUIColorScope(Color color) { _prev = GUI.color; GUI.color = color; }
        public void Dispose() { GUI.color = _prev; }
    }
}
#endif
