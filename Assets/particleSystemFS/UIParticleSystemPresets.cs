using UnityEngine;

/// <summary>
/// Ready-made presets for UIParticleSystem.
/// Call these from your game code, or use the demo script below.
///
/// Usage:
///   UIParticleSystemPresets.ApplyFire(myUIParticleSystem);
///   myUIParticleSystem.Play();
/// </summary>
public static class UIParticleSystemPresets
{
    // ─────────────────────────────────────────────────────────────────────────
    // FIRE
    // ─────────────────────────────────────────────────────────────────────────
    public static void ApplyFire(UIParticleSystem ps)
    {
        ps.loop = true;
        ps.startLifetime = 1.2f;
        ps.startSpeed = 120f;
        ps.startSize = 24f;
        ps.gravityModifier = -80f;
        ps.maxParticles = 200;

        ps.emission = true;
        ps.rateOverTime = 60f;

        ps.shape = UIParticleSystem.EmissionShape.Cone;
        ps.angle = 20f;
        ps.radius = 20f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[]   { new Color(1f, 0.9f, 0.2f), new Color(1f, 0.3f, 0f), new Color(0.2f, 0.1f, 0.1f) },
            new float[]   { 0f, 0.4f, 1f },
            new float[]   { 0f, 0.5f, 1f },
            new float[]   { 1f, 0.8f, 0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve = AnimationCurve.EaseInOut(0f, 0.5f, 1f, 1.5f);

        ps.velocityOverLifetime = true;
        ps.turbulenceStrength = 0.8f;
        ps.dampingOverTime = 0.05f;
        ps.constantForce = Vector2.zero;

        ps.rotationOverLifetime = true;
        ps.rotationSpeed = 45f;
        ps.randomRotationDirection = true;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // STARS / SPARKLES
    // ─────────────────────────────────────────────────────────────────────────
    public static void ApplyStars(UIParticleSystem ps)
    {
        ps.loop = true;
        ps.startLifetime = 2.5f;
        ps.startSpeed = 160f;
        ps.startSize = 12f;
        ps.gravityModifier = 30f;
        ps.maxParticles = 150;

        ps.emission = true;
        ps.rateOverTime = 30f;

        ps.shape = UIParticleSystem.EmissionShape.Cone;
        ps.angle = 60f;
        ps.radius = 10f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[]   { Color.white, new Color(1f, 1f, 0.5f), Color.yellow },
            new float[]   { 0f, 0.5f, 1f },
            new float[]   { 0f, 0.5f, 1f },
            new float[]   { 1f, 1f, 0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

        ps.velocityOverLifetime = false;
        ps.turbulenceStrength = 0f;

        ps.rotationOverLifetime = true;
        ps.rotationSpeed = 180f;
        ps.randomRotationDirection = true;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // CONFETTI
    // ─────────────────────────────────────────────────────────────────────────
    public static void ApplyConfetti(UIParticleSystem ps)
    {
        ps.loop = false;
        ps.startLifetime = 3f;
        ps.startSpeed = 300f;
        ps.startSize = 16f;
        ps.gravityModifier = 250f;
        ps.maxParticles = 300;

        ps.emission = true;
        ps.rateOverTime = 0f;

        ps.shape = UIParticleSystem.EmissionShape.Box;
        ps.angle = 70f;
        ps.radius = 200f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeRandomColorGradient();

        ps.sizeOverLifetime = false;

        ps.velocityOverLifetime = true;
        ps.turbulenceStrength = 0.3f;
        ps.dampingOverTime = 0.01f;
        ps.constantForce = Vector2.zero;

        ps.rotationOverLifetime = true;
        ps.rotationSpeed = 360f;
        ps.randomRotationDirection = true;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // SMOKE
    // ─────────────────────────────────────────────────────────────────────────
    public static void ApplySmoke(UIParticleSystem ps)
    {
        ps.loop = true;
        ps.startLifetime = 4f;
        ps.startSpeed = 40f;
        ps.startSize = 20f;
        ps.gravityModifier = -25f;
        ps.maxParticles = 80;

        ps.emission = true;
        ps.rateOverTime = 15f;

        ps.shape = UIParticleSystem.EmissionShape.Cone;
        ps.angle = 15f;
        ps.radius = 10f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[]   { new Color(0.8f, 0.8f, 0.8f), new Color(0.5f, 0.5f, 0.5f) },
            new float[]   { 0f, 1f },
            new float[]   { 0f, 1f },
            new float[]   { 0.6f, 0f }
        );

        ps.sizeOverLifetime = true;
        Keyframe[] keys = { new Keyframe(0f, 0.5f), new Keyframe(1f, 2f) };
        ps.sizeCurve = new AnimationCurve(keys);

        ps.velocityOverLifetime = true;
        ps.turbulenceStrength = 0.6f;
        ps.dampingOverTime = 0.02f;

        ps.rotationOverLifetime = true;
        ps.rotationSpeed = 20f;
        ps.randomRotationDirection = true;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // MAGIC / PORTAL
    // ─────────────────────────────────────────────────────────────────────────
    public static void ApplyMagic(UIParticleSystem ps)
    {
        ps.loop = true;
        ps.startLifetime = 1.5f;
        ps.startSpeed = 80f;
        ps.startSize = 10f;
        ps.gravityModifier = -15f;
        ps.maxParticles = 200;

        ps.emission = true;
        ps.rateOverTime = 80f;

        ps.shape = UIParticleSystem.EmissionShape.Circle;
        ps.angle = 0f;
        ps.radius = 60f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[]   { new Color(0.8f, 0.4f, 1f), new Color(0.4f, 0.8f, 1f), Color.white },
            new float[]   { 0f, 0.5f, 1f },
            new float[]   { 0f, 0.5f, 1f },
            new float[]   { 1f, 0.8f, 0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

        ps.velocityOverLifetime = true;
        ps.turbulenceStrength = 1f;
        ps.dampingOverTime = 0f;

        ps.rotationOverLifetime = true;
        ps.rotationSpeed = 270f;
        ps.randomRotationDirection = true;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // RAIN
    // ─────────────────────────────────────────────────────────────────────────
    public static void ApplyRain(UIParticleSystem ps)
    {
        ps.loop = true;
        ps.startLifetime = 0.8f;
        ps.startSpeed = 700f;
        ps.startSize = 6f;
        ps.gravityModifier = 500f;
        ps.maxParticles = 300;

        ps.emission = true;
        ps.rateOverTime = 100f;

        ps.shape = UIParticleSystem.EmissionShape.Box;
        ps.angle = 90f;
        ps.radius = 400f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[]   { new Color(0.7f, 0.85f, 1f), new Color(0.5f, 0.75f, 1f) },
            new float[]   { 0f, 1f },
            new float[]   { 0f, 1f },
            new float[]   { 0.8f, 0f }
        );

        ps.sizeOverLifetime = false;
        ps.velocityOverLifetime = false;
        ps.rotationOverLifetime = false;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // HELPERS
    // ─────────────────────────────────────────────────────────────────────────
    private static Gradient MakeGradient(Color[] colors, float[] colorTimes, float[] alphaTimes, float[] alphaValues)
    {
        var g = new Gradient();
        var ck = new GradientColorKey[colors.Length];
        for (int i = 0; i < colors.Length; i++)
            ck[i] = new GradientColorKey(colors[i], colorTimes[i]);

        var ak = new GradientAlphaKey[alphaTimes.Length];
        for (int i = 0; i < alphaTimes.Length; i++)
            ak[i] = new GradientAlphaKey(alphaValues[i], alphaTimes[i]);

        g.SetKeys(ck, ak);
        return g;
    }

    private static Gradient MakeRandomColorGradient()
    {
        // Returns a gradient with a random vivid hue that fades to transparent
        Color c = Color.HSVToRGB(Random.value, 0.8f, 1f);
        return MakeGradient(
            new Color[] { c, c },
            new float[] { 0f, 1f },
            new float[] { 0f, 1f },
            new float[] { 1f, 0f }
        );
    }
}
