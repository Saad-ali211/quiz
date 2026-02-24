using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Custom UI Particle System — works natively inside Canvas (Screen Space Overlay).
/// [ExecuteAlways] allows live preview directly in the Editor without entering Play Mode.
///
/// EDITOR PREVIEW:
///   Select the GameObject in the Hierarchy — the custom Inspector shows
///   Play / Pause / Stop / Burst buttons that drive the simulation in Edit Mode.
///   All property changes reflect instantly on the live preview.
/// </summary>
[ExecuteAlways]
public class UIParticleSystem : MonoBehaviour
{
    // ─────────────────────────────────────────────
    // MAIN MODULE
    // ─────────────────────────────────────────────
    [Header("Main Module")]
    public bool playOnAwake = true;
    public bool loop = true;
    [Range(0.1f, 20f)]     public float duration      = 5f;
    [Range(0.1f, 10f)]     public float startLifetime = 2f;
    [Range(0f, 800f)]      public float startSpeed    = 150f;
    [Range(1f, 100f)]      public float startSize     = 20f;
    [Range(-1200f, 1200f)] public float gravityModifier = 400f;
    [Range(1, 2000)]       public int   maxParticles  = 300;

    // ─────────────────────────────────────────────
    // EMISSION
    // ─────────────────────────────────────────────
    [Header("Emission")]
    public bool  emission    = true;
    [Range(0f, 300f)] public float rateOverTime = 20f;

    // ─────────────────────────────────────────────
    // BURST MODULE
    // ─────────────────────────────────────────────
    [Header("Burst")]
    [Tooltip("Automatically fire a burst when Play() is called")]
    public bool burstOnPlay = false;

    [Range(1, 500)]
    [Tooltip("Number of particles per burst")]
    public int burstCount = 80;

    [Range(0f, 200f)]
    [Tooltip("Random radius around the emitter where burst particles spawn (pixels)")]
    public float burstSpawnRadius = 0f;

    [Range(100f, 1200f)]
    [Tooltip("Outward speed of burst particles — gravity will arc them back down")]
    public float burstSpeed = 500f;

    [Range(0f, 1f)]
    [Tooltip("0 = full 360° sphere  |  1 = upper hemisphere only (fountain style)")]
    public float burstUpwardBias = 0.3f;

    [Tooltip("Repeat burst every X seconds automatically (0 = disabled)")]
    public float burstRepeatInterval = 0f;

    // ─────────────────────────────────────────────
    // SHAPE
    // ─────────────────────────────────────────────
    [Header("Shape (Continuous Emission)")]
    public EmissionShape shape = EmissionShape.Cone;
    [Range(1f, 180f)] public float angle  = 30f;
    [Range(0f, 300f)] public float radius = 0f;

    public enum EmissionShape { Point, Cone, Circle, Box, Hemisphere }

    // ─────────────────────────────────────────────
    // COLOR OVER LIFETIME
    // ─────────────────────────────────────────────
    [Header("Color over Lifetime")]
    public bool     colorOverLifetime = true;
    public Gradient colorGradient;

    // ─────────────────────────────────────────────
    // SIZE OVER LIFETIME
    // ─────────────────────────────────────────────
    [Header("Size over Lifetime")]
    public bool           sizeOverLifetime = true;
    public AnimationCurve sizeCurve        = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    // ─────────────────────────────────────────────
    // VELOCITY OVER LIFETIME
    // ─────────────────────────────────────────────
    [Header("Velocity over Lifetime")]
    public bool    velocityOverLifetime = false;
    public Vector2 constantForce        = Vector2.zero;
    [Range(0f, 5f)] public float turbulenceStrength = 0f;
    [Range(0f, 1f)] public float dampingOverTime    = 0f;

    // ─────────────────────────────────────────────
    // ROTATION OVER LIFETIME
    // ─────────────────────────────────────────────
    [Header("Rotation over Lifetime")]
    public bool  rotationOverLifetime     = true;
    [Range(-720f, 720f)] public float rotationSpeed = 180f;
    public bool  randomRotationDirection  = true;

    // ─────────────────────────────────────────────
    // RENDERER
    // ─────────────────────────────────────────────
    [Header("Renderer")]
    public Sprite   particleSprite;
    public Material particleMaterial;

    // ─────────────────────────────────────────────
    // EDITOR PREVIEW STATE  (not serialized)
    // ─────────────────────────────────────────────
    [System.NonSerialized] public bool  PreviewPlaying = false;
    [System.NonSerialized] public bool  PreviewPaused  = false;
    [System.NonSerialized] public float PreviewTime    = 0f;

    // ─────────────────────────────────────────────
    // INTERNALS
    // ─────────────────────────────────────────────
    private readonly List<UIParticle>  _active = new List<UIParticle>();
    private readonly Queue<UIParticle> _pool   = new Queue<UIParticle>();

    private float  _emitAccum  = 0f;
    private float  _elapsed    = 0f;
    private float  _burstTimer = 0f;
    private bool   _playing    = false;
    private double _lastEditorTime = 0;
    private Sprite _defaultSprite;

    // ═════════════════════════════════════════════════════════════════════════
    // LIFECYCLE
    // ═════════════════════════════════════════════════════════════════════════
    private void Awake()
    {
        EnsureDefaultGradient();

#if UNITY_EDITOR
        // Don't auto-play in edit mode — the Editor controls it
        if (!Application.isPlaying) return;
#endif
        if (playOnAwake) Play();
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.EditorApplication.update += EditorUpdate;
            return;
        }
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.EditorApplication.update -= EditorUpdate;
            EditorStopAndClear();
            return;
        }
#endif
        Stop();
    }

    // Called every frame in Play Mode
    private void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return; // Edit mode handled by EditorUpdate
#endif
        Tick(Time.deltaTime);
    }

    // ═════════════════════════════════════════════════════════════════════════
    // EDITOR UPDATE  — drives simulation in Edit Mode via EditorApplication.update
    // ═════════════════════════════════════════════════════════════════════════
#if UNITY_EDITOR
    private void EditorUpdate()
    {
        if (this == null || !gameObject) { UnityEditor.EditorApplication.update -= EditorUpdate; return; }
        if (!PreviewPlaying || PreviewPaused) { _lastEditorTime = UnityEditor.EditorApplication.timeSinceStartup; return; }

        double now = UnityEditor.EditorApplication.timeSinceStartup;
        float  dt  = Mathf.Min((float)(now - _lastEditorTime), 0.05f);
        _lastEditorTime = now;

        Tick(dt);

        // Force Scene view and Inspector to repaint so you can see the particles move
        UnityEditor.SceneView.RepaintAll();
        UnityEditor.EditorUtility.SetDirty(this);
    }

    public void EditorPlay()
    {
        EnsureDefaultGradient();
        PreviewPlaying = true;
        PreviewPaused  = false;
        _playing       = true;
        _elapsed       = 0f;
        _emitAccum     = 0f;
        _burstTimer    = 0f;
        _lastEditorTime = UnityEditor.EditorApplication.timeSinceStartup;
        if (burstOnPlay) EmitBurst();
    }

    public void EditorPause()
    {
        PreviewPaused = !PreviewPaused;
    }

    public void EditorStop()
    {
        PreviewPlaying = false;
        PreviewPaused  = false;
        _playing       = false;
        EditorStopAndClear();
    }

    private void EditorStopAndClear()
    {
        for (int i = _active.Count - 1; i >= 0; i--)
            ReturnToPool(_active[i]);
        _active.Clear();
        UnityEditor.SceneView.RepaintAll();
    }
#endif

    // ═════════════════════════════════════════════════════════════════════════
    // CORE TICK  — shared between Play Mode and Edit Mode preview
    // ═════════════════════════════════════════════════════════════════════════
    private void Tick(float dt)
    {
        if (!_playing) return;

        _elapsed += dt;
        PreviewTime = _elapsed;

        // Continuous emission
        if (emission && rateOverTime > 0f)
        {
            _emitAccum += rateOverTime * dt;
            while (_emitAccum >= 1f && _active.Count < maxParticles)
            {
                SpawnParticle(isBurst: false);
                _emitAccum -= 1f;
            }
        }

        // Timed repeat burst
        if (burstRepeatInterval > 0f)
        {
            _burstTimer += dt;
            if (_burstTimer >= burstRepeatInterval)
            {
                _burstTimer = 0f;
                EmitBurst();
            }
        }

        // Update active particles
        for (int i = _active.Count - 1; i >= 0; i--)
        {
            UIParticle p = _active[i];
            p.age += dt;

            if (p.age >= p.lifetime)
            {
                ReturnToPool(p);
                _active.RemoveAt(i);
                continue;
            }

            UpdateParticle(p, dt);
        }

        if (!loop && _elapsed >= duration && _active.Count == 0)
            Stop();
    }

    // ═════════════════════════════════════════════════════════════════════════
    // PUBLIC API
    // ═════════════════════════════════════════════════════════════════════════
    public void Play()
    {
        _playing    = true;
        _elapsed    = 0f;
        _emitAccum  = 0f;
        _burstTimer = 0f;
        if (burstOnPlay) EmitBurst();
    }

    public void Stop()
    {
        _playing = false;
    }

    public void Clear()
    {
        for (int i = _active.Count - 1; i >= 0; i--)
            ReturnToPool(_active[i]);
        _active.Clear();
    }

    /// <summary>
    /// Fire a burst of particles that explode outward in all directions.
    /// Gravity pulls them back down in natural parabolic arcs.
    /// </summary>
    public void EmitBurst(int count = -1)
    {
        int n = count < 0 ? burstCount : count;
        for (int i = 0; i < n; i++)
        {
            if (_active.Count >= maxParticles) break;
            SpawnParticle(isBurst: true);
        }
    }

    public void Emit(int count) => EmitBurst(count);

    public bool IsPlaying     => _playing;
    public int  ParticleCount => _active.Count;

    // ═════════════════════════════════════════════════════════════════════════
    // SPAWN
    // ═════════════════════════════════════════════════════════════════════════
    private void SpawnParticle(bool isBurst)
    {
        UIParticle p = GetFromPool();

        Vector2 spawnPos, velocity;

        if (isBurst) GetBurstEmission(out spawnPos, out velocity);
        else         GetShapeEmission(out spawnPos, out velocity);

        p.rectTransform.anchoredPosition = spawnPos;
        p.velocity  = velocity;
        p.lifetime  = startLifetime * Random.Range(0.7f, 1.4f);
        p.age       = 0f;
        p.baseSize  = startSize * Random.Range(0.6f, 1.4f);
        p.rotDir    = (randomRotationDirection && Random.value > 0.5f) ? -1f : 1f;
        p.isBurst   = isBurst;

        p.rectTransform.localEulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
        p.rectTransform.sizeDelta        = new Vector2(p.baseSize, p.baseSize);
        p.gameObject.SetActive(true);
        _active.Add(p);
    }

    // ═════════════════════════════════════════════════════════════════════════
    // UPDATE PARTICLE
    // ═════════════════════════════════════════════════════════════════════════
    private void UpdateParticle(UIParticle p, float dt)
    {
        float t = p.age / p.lifetime;

        p.velocity.y -= gravityModifier * dt;

        if (velocityOverLifetime)
        {
            p.velocity += constantForce * dt;

            if (turbulenceStrength > 0f)
            {
                float nx = (Mathf.PerlinNoise(p.age * 1.3f, p.rectTransform.anchoredPosition.y * 0.01f) - 0.5f) * 2f;
                float ny = (Mathf.PerlinNoise(p.rectTransform.anchoredPosition.x * 0.01f, p.age * 1.7f) - 0.5f) * 2f;
                p.velocity += new Vector2(nx, ny) * turbulenceStrength * 200f * dt;
            }

            p.velocity *= Mathf.Pow(1f - dampingOverTime, dt * 60f);
        }

        p.rectTransform.anchoredPosition += p.velocity * dt;

        if (rotationOverLifetime)
        {
            float a = p.rectTransform.localEulerAngles.z + rotationSpeed * p.rotDir * dt;
            p.rectTransform.localEulerAngles = new Vector3(0f, 0f, a);
        }

        if (sizeOverLifetime)
        {
            float s = p.baseSize * sizeCurve.Evaluate(t);
            p.rectTransform.sizeDelta = new Vector2(s, s);
        }

        if (colorOverLifetime)
            p.image.color = colorGradient.Evaluate(t);
    }

    // ═════════════════════════════════════════════════════════════════════════
    // BURST EMISSION
    // ═════════════════════════════════════════════════════════════════════════
    private void GetBurstEmission(out Vector2 spawnPos, out Vector2 velocity)
    {
        float spawnAngle = Random.Range(0f, Mathf.PI * 2f);
        float spawnR     = Random.Range(0f, burstSpawnRadius);
        spawnPos = new Vector2(Mathf.Cos(spawnAngle) * spawnR, Mathf.Sin(spawnAngle) * spawnR);

        float dirAngle;
        if (burstUpwardBias <= 0f)
        {
            dirAngle = Random.Range(0f, Mathf.PI * 2f);
        }
        else
        {
            float fullCircle = Random.Range(0f, Mathf.PI * 2f);
            float upperHemi  = Random.Range(0f, Mathf.PI);
            dirAngle = Mathf.Lerp(fullCircle, upperHemi, burstUpwardBias);
        }

        float spd = burstSpeed * Random.Range(0.4f, 1.3f);
        velocity  = new Vector2(Mathf.Cos(dirAngle), Mathf.Sin(dirAngle)) * spd;
    }

    // ═════════════════════════════════════════════════════════════════════════
    // SHAPE EMISSION
    // ═════════════════════════════════════════════════════════════════════════
    private void GetShapeEmission(out Vector2 spawnPos, out Vector2 velocity)
    {
        float spd = startSpeed * Random.Range(0.8f, 1.2f);

        switch (shape)
        {
            case EmissionShape.Point:
                spawnPos = Vector2.zero;
                velocity = new Vector2(Mathf.Cos(Random.Range(0f, Mathf.PI * 2f)),
                                       Mathf.Sin(Random.Range(0f, Mathf.PI * 2f))) * spd;
                return;

            case EmissionShape.Cone:
                float a = (90f + Random.Range(-angle * 0.5f, angle * 0.5f)) * Mathf.Deg2Rad;
                spawnPos = new Vector2(Random.Range(-radius, radius), 0f);
                velocity = new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * spd;
                return;

            case EmissionShape.Circle:
                float ca  = Random.Range(0f, Mathf.PI * 2f);
                float cr  = Mathf.Max(radius, 1f);
                spawnPos  = new Vector2(Mathf.Cos(ca) * cr, Mathf.Sin(ca) * cr);
                float va  = Random.Range(0f, Mathf.PI * 2f);
                velocity  = new Vector2(Mathf.Cos(va), Mathf.Sin(va)) * spd;
                return;

            case EmissionShape.Box:
                float hw  = Mathf.Max(radius, 10f);
                spawnPos  = new Vector2(Random.Range(-hw, hw), Random.Range(-hw * 0.4f, hw * 0.4f));
                float ba  = (90f + Random.Range(-15f, 15f)) * Mathf.Deg2Rad;
                velocity  = new Vector2(Mathf.Cos(ba), Mathf.Sin(ba)) * spd;
                return;

            case EmissionShape.Hemisphere:
                float ha  = Random.Range(0f, Mathf.PI);
                float hr  = Mathf.Max(radius, 10f);
                spawnPos  = new Vector2(Mathf.Cos(ha) * hr, 0f);
                velocity  = new Vector2(Mathf.Cos(ha) * 0.5f, Mathf.Sin(ha)) * spd;
                return;
        }

        spawnPos = Vector2.zero;
        velocity = Vector2.up * spd;
    }

    // ═════════════════════════════════════════════════════════════════════════
    // OBJECT POOL
    // ═════════════════════════════════════════════════════════════════════════
    private UIParticle GetFromPool()
    {
        if (_pool.Count > 0) { var p = _pool.Dequeue(); p.gameObject.SetActive(true); return p; }
        return CreateParticle();
    }

    private void ReturnToPool(UIParticle p)
    {
        p.gameObject.SetActive(false);
        _pool.Enqueue(p);
    }

    private UIParticle CreateParticle()
    {
        var go = new GameObject("UIParticle", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(transform, false);

        var rt       = go.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(startSize, startSize);

        var img    = go.GetComponent<Image>();
        img.sprite = particleSprite != null ? particleSprite : GetDefaultCircleSprite();
        if (particleMaterial != null) img.material = particleMaterial;
        img.color  = Color.white;

        var p = new UIParticle { gameObject = go, rectTransform = rt, image = img };
        go.SetActive(false);

        // Hide from hierarchy clutter in Edit Mode
#if UNITY_EDITOR
        go.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;
#endif
        return p;
    }

    private Sprite GetDefaultCircleSprite()
    {
        if (_defaultSprite != null) return _defaultSprite;

        const int size = 64;
        var tex        = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        var pixels     = new Color[size * size];
        var center     = Vector2.one * (size * 0.5f);

        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            float dist  = Vector2.Distance(new Vector2(x, y), center);
            float alpha = Mathf.Clamp01(1f - dist / (size * 0.5f));
            alpha       = alpha * alpha;
            pixels[y * size + x] = new Color(1f, 1f, 1f, alpha);
        }

        tex.SetPixels(pixels);
        tex.Apply();
        _defaultSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _defaultSprite;
    }

    private void EnsureDefaultGradient()
    {
        if (colorGradient != null && colorGradient.colorKeys.Length > 0) return;
        colorGradient = new Gradient();
        colorGradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white, 0f),
                new GradientColorKey(Color.white, 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0f, 1f)
            }
        );
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// PARTICLE DATA
// ─────────────────────────────────────────────────────────────────────────────
[System.Serializable]
public class UIParticle
{
    public GameObject    gameObject;
    public RectTransform rectTransform;
    public Image         image;

    public float   age;
    public float   lifetime;
    public float   baseSize;
    public Vector2 velocity;
    public float   rotDir;
    public bool    isBurst;
}
