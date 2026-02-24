using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Custom UI Particle System that works natively inside a Canvas (Screen Space Overlay).
/// Attach this to an empty GameObject inside your Canvas hierarchy.
/// Particles are pooled UI Images — no Camera or sorting layer tricks needed.
/// </summary>
public class UIParticleSystem : MonoBehaviour
{
    // ─────────────────────────────────────────────
    // MAIN MODULE
    // ─────────────────────────────────────────────
    [Header("Main Module")]
    public bool playOnAwake = true;
    public bool loop = true;
    [Range(0.1f, 20f)] public float duration = 5f;
    [Range(0.1f, 10f)] public float startLifetime = 2f;
    [Range(0f, 500f)]  public float startSpeed = 150f;
    [Range(1f, 100f)]  public float startSize = 20f;
    [Range(-600f, 600f)] public float gravityModifier = 100f;
    [Range(1, 1000)]   public int maxParticles = 200;

    // ─────────────────────────────────────────────
    // EMISSION
    // ─────────────────────────────────────────────
    [Header("Emission")]
    public bool emission = true;
    [Range(1f, 200f)]  public float rateOverTime = 20f;

    // ─────────────────────────────────────────────
    // SHAPE
    // ─────────────────────────────────────────────
    [Header("Shape")]
    public EmissionShape shape = EmissionShape.Cone;
    [Range(1f, 180f)]  public float angle = 30f;   // Cone half-angle OR Circle radius
    [Range(0f, 300f)]  public float radius = 0f;   // Spawn offset radius (Box/Circle size)

    public enum EmissionShape { Point, Cone, Circle, Box, Hemisphere }

    // ─────────────────────────────────────────────
    // COLOR OVER LIFETIME
    // ─────────────────────────────────────────────
    [Header("Color over Lifetime")]
    public bool colorOverLifetime = true;
    public Gradient colorGradient;

    // ─────────────────────────────────────────────
    // SIZE OVER LIFETIME
    // ─────────────────────────────────────────────
    [Header("Size over Lifetime")]
    public bool sizeOverLifetime = true;
    public AnimationCurve sizeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    // ─────────────────────────────────────────────
    // VELOCITY OVER LIFETIME
    // ─────────────────────────────────────────────
    [Header("Velocity over Lifetime")]
    public bool velocityOverLifetime = false;
    public Vector2 constantForce = Vector2.zero;
    [Range(0f, 5f)] public float turbulenceStrength = 0f;
    [Range(0f, 1f)] public float dampingOverTime = 0f;

    // ─────────────────────────────────────────────
    // ROTATION OVER LIFETIME
    // ─────────────────────────────────────────────
    [Header("Rotation over Lifetime")]
    public bool rotationOverLifetime = true;
    [Range(-360f, 360f)] public float rotationSpeed = 90f;
    public bool randomRotationDirection = true;

    // ─────────────────────────────────────────────
    // RENDERER
    // ─────────────────────────────────────────────
    [Header("Renderer")]
    public Sprite particleSprite;    // Leave null for default circle
    public Material particleMaterial; // Leave null to use Image default

    // ─────────────────────────────────────────────
    // INTERNAL
    // ─────────────────────────────────────────────
    private RectTransform _rectTransform;
    private Canvas _rootCanvas;

    private readonly List<UIParticle> _activeParticles = new List<UIParticle>();
    private readonly Queue<UIParticle> _pool = new Queue<UIParticle>();

    private float _emitAccum = 0f;
    private float _elapsed = 0f;
    private bool _playing = false;

    // ─────────────────────────────────────────────────────────────────────────
    // LIFECYCLE
    // ─────────────────────────────────────────────────────────────────────────
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rootCanvas = GetComponentInParent<Canvas>();

        // Default gradient if none assigned
        if (colorGradient == null || colorGradient.colorKeys.Length == 0)
        {
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

        if (playOnAwake) Play();
    }

    private void Update()
    {
        if (!_playing) return;

        _elapsed += Time.deltaTime;

        // Emission
        if (emission)
        {
            _emitAccum += rateOverTime * Time.deltaTime;
            while (_emitAccum >= 1f && _activeParticles.Count < maxParticles)
            {
                SpawnParticle();
                _emitAccum -= 1f;
            }
        }

        // Update active particles
        for (int i = _activeParticles.Count - 1; i >= 0; i--)
        {
            UIParticle p = _activeParticles[i];
            p.age += Time.deltaTime;

            if (p.age >= p.lifetime)
            {
                ReturnToPool(p);
                _activeParticles.RemoveAt(i);
                continue;
            }

            UpdateParticle(p);
        }

        // Loop / stop
        if (!loop && _elapsed >= duration && _activeParticles.Count == 0)
            Stop();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PUBLIC API — mirrors Unity ParticleSystem
    // ─────────────────────────────────────────────────────────────────────────
    public void Play()
    {
        _playing = true;
        _elapsed = 0f;
        _emitAccum = 0f;
    }

    public void Stop()
    {
        _playing = false;
    }

    public void Clear()
    {
        for (int i = _activeParticles.Count - 1; i >= 0; i--)
            ReturnToPool(_activeParticles[i]);
        _activeParticles.Clear();
    }

    /// <summary>Emit a one-shot burst of particles (like Unity Burst emission).</summary>
    public void Emit(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_activeParticles.Count >= maxParticles) break;
            SpawnParticle();
        }
    }

    public bool IsPlaying => _playing;
    public int ParticleCount => _activeParticles.Count;

    // ─────────────────────────────────────────────────────────────────────────
    // SPAWN
    // ─────────────────────────────────────────────────────────────────────────
    private void SpawnParticle()
    {
        UIParticle p = GetFromPool();

        // Position & velocity from shape
        Vector2 localPos;
        Vector2 velocity;
        GetShapeEmission(out localPos, out velocity);

        p.rectTransform.anchoredPosition = localPos;
        p.velocity = velocity;

        // Lifetime & base size
        p.lifetime = startLifetime * Random.Range(0.75f, 1.25f);
        p.age = 0f;
        p.baseSize = startSize * Random.Range(0.7f, 1.3f);

        // Rotation
        p.rotDir = (randomRotationDirection && Random.value > 0.5f) ? -1f : 1f;
        p.rectTransform.localEulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));

        p.gameObject.SetActive(true);
        _activeParticles.Add(p);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // UPDATE PARTICLE
    // ─────────────────────────────────────────────────────────────────────────
    private void UpdateParticle(UIParticle p)
    {
        float t = p.age / p.lifetime;
        float dt = Time.deltaTime;

        // Gravity
        p.velocity.y -= gravityModifier * dt;

        // Velocity over lifetime
        if (velocityOverLifetime)
        {
            p.velocity += constantForce * dt;

            if (turbulenceStrength > 0f)
            {
                float noiseX = (Mathf.PerlinNoise(p.age * 1.3f, p.rectTransform.anchoredPosition.y * 0.01f) - 0.5f) * 2f;
                float noiseY = (Mathf.PerlinNoise(p.rectTransform.anchoredPosition.x * 0.01f, p.age * 1.7f) - 0.5f) * 2f;
                p.velocity += new Vector2(noiseX, noiseY) * turbulenceStrength * 200f * dt;
            }

            float damp = Mathf.Pow(1f - dampingOverTime, dt * 60f);
            p.velocity *= damp;
        }

        // Move
        p.rectTransform.anchoredPosition += p.velocity * dt;

        // Rotation
        if (rotationOverLifetime)
        {
            float angle = p.rectTransform.localEulerAngles.z;
            angle += rotationSpeed * p.rotDir * dt;
            p.rectTransform.localEulerAngles = new Vector3(0f, 0f, angle);
        }

        // Size
        float size = p.baseSize;
        if (sizeOverLifetime)
            size *= sizeCurve.Evaluate(t);
        p.rectTransform.sizeDelta = new Vector2(size, size);

        // Color / Alpha
        if (colorOverLifetime)
            p.image.color = colorGradient.Evaluate(t);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // SHAPE EMISSION
    // ─────────────────────────────────────────────────────────────────────────
    private void GetShapeEmission(out Vector2 localPos, out Vector2 velocity)
    {
        float spd = startSpeed * Random.Range(0.8f, 1.2f);

        switch (shape)
        {
            case EmissionShape.Point:
            {
                localPos = Vector2.zero;
                float a = Random.Range(0f, Mathf.PI * 2f);
                velocity = new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * spd;
                break;
            }

            case EmissionShape.Cone:
            {
                float halfRad = angle * Mathf.Deg2Rad * 0.5f;
                float a = (90f + Random.Range(-angle * 0.5f, angle * 0.5f)) * Mathf.Deg2Rad;
                localPos = new Vector2(Random.Range(-radius, radius), 0f);
                velocity = new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * spd;
                break;
            }

            case EmissionShape.Circle:
            {
                float a = Random.Range(0f, Mathf.PI * 2f);
                float r = radius > 0f ? radius : 1f;
                localPos = new Vector2(Mathf.Cos(a) * r, Mathf.Sin(a) * r);
                float va = Random.Range(0f, Mathf.PI * 2f);
                velocity = new Vector2(Mathf.Cos(va), Mathf.Sin(va)) * spd;
                break;
            }

            case EmissionShape.Box:
            {
                float hw = Mathf.Max(radius, 10f);
                localPos = new Vector2(Random.Range(-hw, hw), Random.Range(-hw * 0.4f, hw * 0.4f));
                float a = (90f + Random.Range(-15f, 15f)) * Mathf.Deg2Rad;
                velocity = new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * spd;
                break;
            }

            case EmissionShape.Hemisphere:
            {
                float a = Random.Range(0f, Mathf.PI);
                float r = Mathf.Max(radius, 10f);
                localPos = new Vector2(Mathf.Cos(a) * r, 0f);
                velocity = new Vector2(Mathf.Cos(a) * 0.5f, Mathf.Sin(a)) * spd;
                break;
            }

            default:
                localPos = Vector2.zero;
                velocity = Vector2.up * spd;
                break;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // OBJECT POOL
    // ─────────────────────────────────────────────────────────────────────────
    private UIParticle GetFromPool()
    {
        if (_pool.Count > 0)
        {
            UIParticle p = _pool.Dequeue();
            p.gameObject.SetActive(true);
            return p;
        }
        return CreateParticle();
    }

    private void ReturnToPool(UIParticle p)
    {
        p.gameObject.SetActive(false);
    }

    private UIParticle CreateParticle()
    {
        // Create a child GameObject with Image + RectTransform
        GameObject go = new GameObject("UIParticle", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(transform, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(startSize, startSize);

        Image img = go.GetComponent<Image>();
        if (particleSprite != null)
            img.sprite = particleSprite;
        else
            img.sprite = GetDefaultCircleSprite();

        if (particleMaterial != null)
            img.material = particleMaterial;

        // Use Additive-like look with alpha
        img.color = Color.white;

        UIParticle particle = new UIParticle
        {
            gameObject = go,
            rectTransform = rt,
            image = img
        };

        go.SetActive(false);
        return particle;
    }

    /// <summary>
    /// Generates a soft circle sprite at runtime — no texture asset needed.
    /// For a glowing look, assign a soft-circle sprite from your project instead.
    /// </summary>
    private Sprite _defaultSprite;
    private Sprite GetDefaultCircleSprite()
    {
        if (_defaultSprite != null) return _defaultSprite;

        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        Color[] pixels = new Color[size * size];
        Vector2 center = new Vector2(size * 0.5f, size * 0.5f);
        float r = size * 0.5f;

        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            float dist = Vector2.Distance(new Vector2(x, y), center);
            float alpha = Mathf.Clamp01(1f - (dist / r));
            alpha = alpha * alpha; // Soft falloff
            pixels[y * size + x] = new Color(1f, 1f, 1f, alpha);
        }

        tex.SetPixels(pixels);
        tex.Apply();
        _defaultSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return _defaultSprite;
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// DATA CLASS — one per active particle
// ─────────────────────────────────────────────────────────────────────────────
[System.Serializable]
public class UIParticle
{
    public GameObject gameObject;
    public RectTransform rectTransform;
    public Image image;

    public float age;
    public float lifetime;
    public float baseSize;
    public Vector2 velocity;
    public float rotDir;
}
