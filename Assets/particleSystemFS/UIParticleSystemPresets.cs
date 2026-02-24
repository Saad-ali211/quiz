using UnityEngine;

/// <summary>
/// 25+ ready-made particle presets for casual and magic games.
///
/// CATEGORIES:
///   Candy Crush / Match-3  — CandyPop, CandyRainbow, JellySplat, CandyClear, SugarStar
///   Celebrations           — Confetti, Fireworks, Ticker, CoinShower, LevelUp
///   Magic / Spells         — MagicCircle, WizardSpell, FairyDust, Vortex, HolyLight, DarkMatter
///   Nature                 — Fire, Smoke, Rain, Snow, Leaves, Bubbles
///   UI Feedback            — Stars, TapPop, ScoreFloat, HeartBurst, ShieldHit
///
/// Usage:
///   UIParticleSystemPresets.ApplyCandyPop(ps);
///   ps.Play();                  // continuous
///   ps.EmitBurst();             // one-shot burst
/// </summary>
public static class UIParticleSystemPresets
{
    // ═════════════════════════════════════════════════════════════════════════
    // ░░  CANDY CRUSH / MATCH-3
    // ═════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// The classic Candy Crush tile-clear pop: bright candy-colored burst,
    /// particles arc out and fall with gravity. Looks great on any match-3 clear.
    /// </summary>
    public static void ApplyCandyPop(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = false;
        ps.startLifetime   = 0.9f;
        ps.gravityModifier = 550f;
        ps.maxParticles    = 60;
        ps.startSize       = 14f;

        ps.emission        = false;
        ps.burstOnPlay     = true;
        ps.burstCount      = 28;
        ps.burstSpeed      = 480f;
        ps.burstUpwardBias = 0.55f;
        ps.burstSpawnRadius = 8f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { C(1f,0.25f,0.55f), C(1f,0.8f,0f), C(0.25f,0.85f,1f) },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 0f, 0.4f, 1f },
            new float[] { 1f, 1f,   0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,1.3f, 0.3f,1f, 1f,0.2f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 300f;
        ps.randomRotationDirection = true;
        ps.velocityOverLifetime    = false;
    }

    /// <summary>
    /// Rainbow cascade — continuous stream of multi-colored gems falling
    /// like a candy waterfall. Great for bonus/rainbow stripe powerup.
    /// </summary>
    public static void ApplyCandyRainbow(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = true;
        ps.startLifetime   = 2f;
        ps.startSpeed      = 20f;
        ps.gravityModifier = 300f;
        ps.maxParticles    = 200;
        ps.startSize       = 18f;

        ps.emission    = true;
        ps.rateOverTime = 40f;

        ps.shape  = UIParticleSystem.EmissionShape.Box;
        ps.radius = 300f;
        ps.angle  = 5f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeRainbowGradient();

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,0.4f, 0.2f,1f, 0.8f,1f, 1f,0f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 120f;
        ps.randomRotationDirection = true;
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.15f;
    }

    /// <summary>
    /// Jelly splat — wobbly gooey splatter when a jelly tile is cleared.
    /// Short lifetime, gravity pulls blobs down fast.
    /// </summary>
    public static void ApplyJellySplat(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = false;
        ps.startLifetime   = 0.7f;
        ps.gravityModifier = 900f;
        ps.maxParticles    = 40;
        ps.startSize       = 20f;

        ps.emission        = false;
        ps.burstOnPlay     = true;
        ps.burstCount      = 18;
        ps.burstSpeed      = 350f;
        ps.burstUpwardBias = 0.7f;
        ps.burstSpawnRadius = 15f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { C(0.9f,0.2f,0.5f), C(1f,0.5f,0.7f) },
            new float[] { 0f, 1f },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 1f, 0.9f, 0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,1.5f, 0.3f,1.8f, 1f,0.3f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 200f;
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.4f;
        ps.dampingOverTime  = 0.08f;
    }

    /// <summary>
    /// Candy cascade clear — particles burst upward from center like
    /// the screen explosion when you clear a big combo in Candy Crush.
    /// </summary>
    public static void ApplyCandyClear(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = false;
        ps.startLifetime   = 1.4f;
        ps.gravityModifier = 350f;
        ps.maxParticles    = 150;
        ps.startSize       = 12f;

        ps.emission        = false;
        ps.burstOnPlay     = true;
        ps.burstCount      = 80;
        ps.burstSpeed      = 650f;
        ps.burstUpwardBias = 0.35f;
        ps.burstSpawnRadius = 40f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { Color.white, C(1f,0.4f,0.8f), C(0.3f,0.9f,1f), C(1f,0.9f,0f) },
            new float[] { 0f, 0.3f, 0.6f, 1f },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 1f, 1f,   0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,0.6f, 0.15f,1f, 1f,0f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 420f;
        ps.randomRotationDirection = true;
        ps.velocityOverLifetime    = false;
    }

    /// <summary>
    /// Sugar star — sparkly star particles radiate outward from a power-up,
    /// like the striped candy or color bomb activation.
    /// </summary>
    public static void ApplySugarStar(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = true;
        ps.startLifetime   = 1.2f;
        ps.startSpeed      = 200f;
        ps.gravityModifier = -10f;
        ps.maxParticles    = 80;
        ps.startSize       = 8f;

        ps.emission    = true;
        ps.rateOverTime = 50f;

        ps.shape  = UIParticleSystem.EmissionShape.Circle;
        ps.radius = 30f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { Color.white, C(1f,0.95f,0.4f), C(1f,0.65f,0f) },
            new float[] { 0f, 0.4f, 1f },
            new float[] { 0f, 0.6f, 1f },
            new float[] { 1f, 1f,   0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,0.3f, 0.2f,1f, 1f,0f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 360f;
        ps.randomRotationDirection = true;
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.2f;
    }

    // ═════════════════════════════════════════════════════════════════════════
    // ░░  CELEBRATIONS
    // ═════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Classic confetti explosion — level complete screen staple.
    /// </summary>
    public static void ApplyConfetti(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = false;
        ps.startLifetime   = 2.8f;
        ps.gravityModifier = 380f;
        ps.maxParticles    = 200;
        ps.startSize       = 15f;

        ps.emission        = false;
        ps.burstOnPlay     = true;
        ps.burstCount      = 100;
        ps.burstSpeed      = 620f;
        ps.burstUpwardBias = 0.45f;
        ps.burstSpawnRadius = 30f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeRainbowGradient();

        ps.sizeOverLifetime = false;
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 360f;
        ps.randomRotationDirection = true;
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.25f;
        ps.dampingOverTime  = 0.01f;
    }

    /// <summary>
    /// Fireworks rocket trail — particles shoot up then explode.
    /// Call EmitBurst() repeatedly for multi-rocket effect.
    /// </summary>
    public static void ApplyFireworks(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = false;
        ps.startLifetime   = 2f;
        ps.gravityModifier = 180f;
        ps.maxParticles    = 120;
        ps.startSize       = 9f;

        ps.emission        = false;
        ps.burstOnPlay     = true;
        ps.burstCount      = 60;
        ps.burstSpeed      = 800f;
        ps.burstUpwardBias = 0f;   // true 360° — firework shell explosion
        ps.burstSpawnRadius = 5f;
        ps.burstRepeatInterval = 0f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { Color.white, C(1f,0.9f,0.3f), C(1f,0.4f,0.1f), C(0.5f,0.1f,0f) },
            new float[] { 0f, 0.15f, 0.5f, 1f },
            new float[] { 0f, 0.2f, 0.7f, 1f },
            new float[] { 1f, 1f,   0.7f, 0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,1f, 0.3f,0.8f, 1f,0.1f);
        ps.rotationOverLifetime = false;
        ps.velocityOverLifetime = false;
    }

    /// <summary>
    /// Coin shower — gold coins rain down from the top.
    /// Great for bank/treasure reward screen.
    /// </summary>
    public static void ApplyCoinShower(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = true;
        ps.startLifetime   = 2.5f;
        ps.startSpeed      = 50f;
        ps.gravityModifier = 350f;
        ps.maxParticles    = 150;
        ps.startSize       = 20f;

        ps.emission    = true;
        ps.rateOverTime = 25f;

        ps.shape  = UIParticleSystem.EmissionShape.Box;
        ps.radius = 350f;
        ps.angle  = 8f;

        // Emit from top — negative Y speed so they fall downward from spawn
        // (set emitter at top of canvas, or use constantForce)
        ps.velocityOverLifetime = false;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { C(1f,0.95f,0.5f), C(1f,0.75f,0f), C(0.8f,0.55f,0f) },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 0f, 0.7f, 1f },
            new float[] { 1f, 1f,   0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,0.3f, 0.2f,1f, 0.8f,1f, 1f,0.5f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 200f;
        ps.randomRotationDirection = true;
    }

    /// <summary>
    /// Level Up fanfare — stars shoot out in a halo ring then fade.
    /// </summary>
    public static void ApplyLevelUp(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = false;
        ps.startLifetime   = 1.8f;
        ps.gravityModifier = -60f;  // float upward
        ps.maxParticles    = 80;
        ps.startSize       = 16f;

        ps.emission        = false;
        ps.burstOnPlay     = true;
        ps.burstCount      = 40;
        ps.burstSpeed      = 300f;
        ps.burstUpwardBias = 0f;    // ring shape
        ps.burstSpawnRadius = 80f;
        ps.burstRepeatInterval = 0.4f; // two waves

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { Color.white, C(1f,0.95f,0.3f), C(1f,0.7f,0f) },
            new float[] { 0f, 0.3f, 1f },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 1f, 1f,   0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,0.5f, 0.2f,1.2f, 1f,0f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 270f;
        ps.randomRotationDirection = true;
    }

    /// <summary>
    /// Ticker tape — thin paper strips stream down from above.
    /// Good for parade / victory scene.
    /// </summary>
    public static void ApplyTickerTape(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = true;
        ps.startLifetime   = 3.5f;
        ps.startSpeed      = 10f;
        ps.gravityModifier = 150f;
        ps.maxParticles    = 200;
        ps.startSize       = 10f;

        ps.emission    = true;
        ps.rateOverTime = 35f;

        ps.shape  = UIParticleSystem.EmissionShape.Box;
        ps.radius = 400f;
        ps.angle  = 5f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { C(1f,1f,1f), C(0.9f,0.9f,1f) },
            new float[] { 0f, 1f },
            new float[] { 0f, 0.8f, 1f },
            new float[] { 1f, 1f,   0f }
        );

        ps.sizeOverLifetime = false;
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 90f;
        ps.randomRotationDirection = true;
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.4f;
        ps.constantForce = new Vector2(30f, 0f); // gentle sideways drift
    }

    // ═════════════════════════════════════════════════════════════════════════
    // ░░  MAGIC / SPELLS
    // ═════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Magic circle — particles orbit in a ring, like a summoning or
    /// charging spell. Place emitter at the spell origin.
    /// </summary>
    public static void ApplyMagicCircle(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = true;
        ps.startLifetime   = 1.5f;
        ps.startSpeed      = 60f;
        ps.gravityModifier = 0f;
        ps.maxParticles    = 120;
        ps.startSize       = 8f;

        ps.emission    = true;
        ps.rateOverTime = 70f;

        ps.shape  = UIParticleSystem.EmissionShape.Circle;
        ps.radius = 70f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { C(0.6f,0.3f,1f), C(0.9f,0.5f,1f), Color.white },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 1f, 0.8f, 0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,0.3f, 0.3f,1f, 1f,0f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 360f;
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.6f;
        ps.dampingOverTime  = 0f;
    }

    /// <summary>
    /// Wizard spell cast — crackling electric bolt particles, blue/white.
    /// Good for lightning, thunder, or ice spells.
    /// </summary>
    public static void ApplyWizardSpell(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = false;
        ps.startLifetime   = 0.6f;
        ps.gravityModifier = -50f;
        ps.maxParticles    = 100;
        ps.startSize       = 7f;

        ps.emission        = false;
        ps.burstOnPlay     = true;
        ps.burstCount      = 60;
        ps.burstSpeed      = 900f;
        ps.burstUpwardBias = 0f;
        ps.burstSpawnRadius = 10f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { Color.white, C(0.5f,0.85f,1f), C(0.1f,0.4f,1f) },
            new float[] { 0f, 0.3f, 1f },
            new float[] { 0f, 0.4f, 1f },
            new float[] { 1f, 1f,   0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,1.5f, 0.2f,0.8f, 1f,0.1f);
        ps.rotationOverLifetime = false;
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 1.8f;
        ps.dampingOverTime  = 0.15f;
    }

    /// <summary>
    /// Fairy dust — tiny glittering particles trail upward, soft and dreamy.
    /// Perfect for fairy godmother, wish, or healing effects.
    /// </summary>
    public static void ApplyFairyDust(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = true;
        ps.startLifetime   = 2.5f;
        ps.startSpeed      = 60f;
        ps.gravityModifier = -40f;  // float upward
        ps.maxParticles    = 150;
        ps.startSize       = 6f;

        ps.emission    = true;
        ps.rateOverTime = 45f;

        ps.shape  = UIParticleSystem.EmissionShape.Hemisphere;
        ps.radius = 30f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { Color.white, C(1f,0.7f,1f), C(0.7f,0.5f,1f), C(0.4f,0.8f,1f) },
            new float[] { 0f, 0.3f, 0.6f, 1f },
            new float[] { 0f, 0.4f, 1f },
            new float[] { 1f, 1f,   0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,0.2f, 0.3f,1f, 0.7f,0.8f, 1f,0f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 180f;
        ps.randomRotationDirection = true;
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.5f;
        ps.constantForce = new Vector2(15f, 0f);
    }

    /// <summary>
    /// Vortex / black hole — particles swirl inward, darkening as they go.
    /// Use for dark spell, portal, or being absorbed effect.
    /// </summary>
    public static void ApplyVortex(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = true;
        ps.startLifetime   = 2f;
        ps.startSpeed      = 100f;
        ps.gravityModifier = 0f;
        ps.maxParticles    = 200;
        ps.startSize       = 10f;

        ps.emission    = true;
        ps.rateOverTime = 80f;

        ps.shape  = UIParticleSystem.EmissionShape.Circle;
        ps.radius = 120f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { C(0.5f,0.2f,1f), C(0.3f,0.1f,0.7f), C(0.05f,0f,0.15f) },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 0.8f,0.6f, 0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,1f, 1f,0.1f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 540f;
        ps.randomRotationDirection = false;
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.3f;
        ps.constantForce = new Vector2(-60f, 0f); // inward pull (adjust per emitter placement)
        ps.dampingOverTime  = 0.03f;
    }

    /// <summary>
    /// Holy light / angel — soft white/gold rays burst upward.
    /// Great for resurrection, blessing, or holy power.
    /// </summary>
    public static void ApplyHolyLight(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = false;
        ps.startLifetime   = 2.2f;
        ps.gravityModifier = -120f; // float upward
        ps.maxParticles    = 80;
        ps.startSize       = 18f;

        ps.emission        = false;
        ps.burstOnPlay     = true;
        ps.burstCount      = 50;
        ps.burstSpeed      = 280f;
        ps.burstUpwardBias = 0.9f;  // strongly upward
        ps.burstSpawnRadius = 20f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { Color.white, C(1f,0.98f,0.7f), C(1f,0.9f,0.4f) },
            new float[] { 0f, 0.4f, 1f },
            new float[] { 0f, 0.3f, 1f },
            new float[] { 0.9f,1f,  0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,0.4f, 0.2f,1.2f, 1f,0f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 90f;
        ps.randomRotationDirection = true;
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.2f;
    }

    /// <summary>
    /// Dark matter / shadow explosion — dark purple particles burst and
    /// dissolve. Use for dark magic, shadow strike, curse.
    /// </summary>
    public static void ApplyDarkMatter(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = false;
        ps.startLifetime   = 1.5f;
        ps.gravityModifier = -30f;
        ps.maxParticles    = 100;
        ps.startSize       = 22f;

        ps.emission        = false;
        ps.burstOnPlay     = true;
        ps.burstCount      = 50;
        ps.burstSpeed      = 400f;
        ps.burstUpwardBias = 0f;
        ps.burstSpawnRadius = 15f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { C(0.4f,0f,0.6f), C(0.15f,0f,0.3f), C(0.05f,0f,0.1f) },
            new float[] { 0f, 0.4f, 1f },
            new float[] { 0f, 0.4f, 1f },
            new float[] { 0.9f,0.6f, 0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,0.5f, 0.3f,1.4f, 1f,0.2f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 200f;
        ps.randomRotationDirection = true;
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 1.2f;
        ps.dampingOverTime  = 0.04f;
    }

    // ═════════════════════════════════════════════════════════════════════════
    // ░░  NATURE
    // ═════════════════════════════════════════════════════════════════════════

    /// <summary>Fire flame — classic fire rising upward.</summary>
    public static void ApplyFire(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = true;
        ps.startLifetime   = 1.2f;
        ps.startSpeed      = 120f;
        ps.gravityModifier = -80f;
        ps.maxParticles    = 200;
        ps.startSize       = 24f;

        ps.emission    = true;
        ps.rateOverTime = 60f;

        ps.shape  = UIParticleSystem.EmissionShape.Cone;
        ps.angle  = 20f;
        ps.radius = 20f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { C(1f,0.9f,0.2f), C(1f,0.3f,0f), C(0.2f,0.1f,0.1f) },
            new float[] { 0f, 0.4f, 1f },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 1f, 0.8f, 0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,0.5f, 1f,1.5f);
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.8f;
        ps.dampingOverTime  = 0.05f;
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 45f;
        ps.randomRotationDirection = true;
    }

    /// <summary>Smoke — soft grey billowing cloud rising upward.</summary>
    public static void ApplySmoke(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = true;
        ps.startLifetime   = 4f;
        ps.startSpeed      = 40f;
        ps.gravityModifier = -25f;
        ps.maxParticles    = 80;
        ps.startSize       = 20f;

        ps.emission    = true;
        ps.rateOverTime = 15f;

        ps.shape  = UIParticleSystem.EmissionShape.Cone;
        ps.angle  = 15f;
        ps.radius = 10f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { C(0.8f,0.8f,0.8f), C(0.5f,0.5f,0.5f) },
            new float[] { 0f, 1f },
            new float[] { 0f, 1f },
            new float[] { 0.6f, 0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,0.5f, 1f,2f);
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.6f;
        ps.dampingOverTime  = 0.02f;
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 20f;
        ps.randomRotationDirection = true;
    }

    /// <summary>Rain — fast blue-grey droplets falling from above.</summary>
    public static void ApplyRain(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = true;
        ps.startLifetime   = 0.8f;
        ps.startSpeed      = 700f;
        ps.gravityModifier = 500f;
        ps.maxParticles    = 300;
        ps.startSize       = 6f;

        ps.emission    = true;
        ps.rateOverTime = 100f;

        ps.shape  = UIParticleSystem.EmissionShape.Box;
        ps.angle  = 90f;
        ps.radius = 400f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { C(0.7f,0.85f,1f), C(0.5f,0.75f,1f) },
            new float[] { 0f, 1f },
            new float[] { 0f, 1f },
            new float[] { 0.8f, 0f }
        );

        ps.sizeOverLifetime = false;
        ps.velocityOverLifetime = false;
        ps.rotationOverLifetime = false;
    }

    /// <summary>Snow — slow soft white flakes drifting downward.</summary>
    public static void ApplySnow(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = true;
        ps.startLifetime   = 6f;
        ps.startSpeed      = 20f;
        ps.gravityModifier = 35f;
        ps.maxParticles    = 200;
        ps.startSize       = 8f;

        ps.emission    = true;
        ps.rateOverTime = 20f;

        ps.shape  = UIParticleSystem.EmissionShape.Box;
        ps.radius = 400f;
        ps.angle  = 5f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { Color.white, C(0.85f,0.92f,1f) },
            new float[] { 0f, 1f },
            new float[] { 0f, 0.8f, 1f },
            new float[] { 0.9f, 0.9f, 0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,0.3f, 0.3f,1f, 0.9f,1f, 1f,0.3f);
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.3f;
        ps.constantForce = new Vector2(20f, 0f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 30f;
        ps.randomRotationDirection = true;
    }

    /// <summary>Falling leaves — organic drifting leaf shapes.</summary>
    public static void ApplyLeaves(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = true;
        ps.startLifetime   = 5f;
        ps.startSpeed      = 15f;
        ps.gravityModifier = 60f;
        ps.maxParticles    = 80;
        ps.startSize       = 16f;

        ps.emission    = true;
        ps.rateOverTime = 10f;

        ps.shape  = UIParticleSystem.EmissionShape.Box;
        ps.radius = 400f;
        ps.angle  = 5f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { C(0.6f,0.85f,0.2f), C(0.9f,0.6f,0.1f), C(0.8f,0.25f,0.05f) },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 0f, 0.8f, 1f },
            new float[] { 1f, 1f,   0f }
        );

        ps.sizeOverLifetime = false;
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.6f;
        ps.constantForce = new Vector2(25f, 0f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 80f;
        ps.randomRotationDirection = true;
    }

    /// <summary>Bubbles — soft transparent circles floating upward.</summary>
    public static void ApplyBubbles(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = true;
        ps.startLifetime   = 4f;
        ps.startSpeed      = 50f;
        ps.gravityModifier = -60f;
        ps.maxParticles    = 80;
        ps.startSize       = 20f;

        ps.emission    = true;
        ps.rateOverTime = 12f;

        ps.shape  = UIParticleSystem.EmissionShape.Hemisphere;
        ps.radius = 80f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { C(0.6f,0.85f,1f), C(0.8f,0.95f,1f), Color.white },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 0f, 0.5f, 0.9f, 1f },
            new float[] { 0.3f,0.5f, 0.4f,0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,0.4f, 0.3f,1f, 0.9f,1.1f, 1f,0.9f);
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.25f;
        ps.rotationOverLifetime = false;
    }

    // ═════════════════════════════════════════════════════════════════════════
    // ░░  UI FEEDBACK
    // ═════════════════════════════════════════════════════════════════════════

    /// <summary>Stars — three classic gold stars radiating out. Win screen staple.</summary>
    public static void ApplyStars(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = true;
        ps.startLifetime   = 2.5f;
        ps.startSpeed      = 160f;
        ps.gravityModifier = 30f;
        ps.maxParticles    = 150;
        ps.startSize       = 12f;

        ps.emission    = true;
        ps.rateOverTime = 30f;

        ps.shape  = UIParticleSystem.EmissionShape.Cone;
        ps.angle  = 60f;
        ps.radius = 10f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { Color.white, C(1f,1f,0.5f), Color.yellow },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 1f, 1f,   0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,1f, 1f,0f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 180f;
        ps.randomRotationDirection = true;
    }

    /// <summary>Tap pop — tiny instant burst on button press. Snappy and satisfying.</summary>
    public static void ApplyTapPop(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = false;
        ps.startLifetime   = 0.5f;
        ps.gravityModifier = 700f;
        ps.maxParticles    = 20;
        ps.startSize       = 9f;

        ps.emission        = false;
        ps.burstOnPlay     = true;
        ps.burstCount      = 12;
        ps.burstSpeed      = 320f;
        ps.burstUpwardBias = 0.5f;
        ps.burstSpawnRadius = 4f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { Color.white, C(0.4f,0.8f,1f) },
            new float[] { 0f, 1f },
            new float[] { 0f, 1f },
            new float[] { 1f, 0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,1.2f, 0.3f,0.8f, 1f,0f);
        ps.rotationOverLifetime = false;
        ps.velocityOverLifetime = false;
    }

    /// <summary>Heart burst — pink hearts pop out for likes/love reactions.</summary>
    public static void ApplyHeartBurst(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = false;
        ps.startLifetime   = 1.8f;
        ps.gravityModifier = -80f;  // float upward
        ps.maxParticles    = 30;
        ps.startSize       = 18f;

        ps.emission        = false;
        ps.burstOnPlay     = true;
        ps.burstCount      = 18;
        ps.burstSpeed      = 250f;
        ps.burstUpwardBias = 0.75f;
        ps.burstSpawnRadius = 10f;

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { C(1f,0.4f,0.55f), C(1f,0.6f,0.7f), Color.white },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 1f, 0.8f, 0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,0.5f, 0.2f,1.2f, 1f,0.3f);
        ps.rotationOverLifetime    = true;
        ps.rotationSpeed    = 60f;
        ps.randomRotationDirection = true;
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.2f;
    }

    /// <summary>Shield hit — ring of white sparks deflecting outward on impact.</summary>
    public static void ApplyShieldHit(UIParticleSystem ps)
    {
        Reset(ps);
        ps.loop            = false;
        ps.startLifetime   = 0.6f;
        ps.gravityModifier = 0f;
        ps.maxParticles    = 50;
        ps.startSize       = 8f;

        ps.emission        = false;
        ps.burstOnPlay     = true;
        ps.burstCount      = 30;
        ps.burstSpeed      = 600f;
        ps.burstUpwardBias = 0f;    // 360° ring
        ps.burstSpawnRadius = 50f;  // spawn on the shield ring radius

        ps.colorOverLifetime = true;
        ps.colorGradient = MakeGradient(
            new Color[] { Color.white, C(0.6f,0.9f,1f), C(0.2f,0.6f,1f) },
            new float[] { 0f, 0.3f, 1f },
            new float[] { 0f, 0.5f, 1f },
            new float[] { 1f, 1f,   0f }
        );

        ps.sizeOverLifetime = true;
        ps.sizeCurve        = Curve(0f,1f, 0.4f,0.6f, 1f,0f);
        ps.rotationOverLifetime = false;
        ps.velocityOverLifetime    = true;
        ps.turbulenceStrength = 0.3f;
        ps.dampingOverTime  = 0.12f;
    }

    // ═════════════════════════════════════════════════════════════════════════
    // ░░  HELPERS
    // ═════════════════════════════════════════════════════════════════════════

    /// <summary>Reset all fields to safe defaults before applying a preset,
    /// so old values from a previous preset never bleed through.</summary>
    private static void Reset(UIParticleSystem ps)
    {
        ps.loop              = true;
        ps.duration          = 5f;
        ps.startLifetime     = 2f;
        ps.startSpeed        = 150f;
        ps.startSize         = 20f;
        ps.gravityModifier   = 100f;
        ps.maxParticles      = 200;

        ps.emission          = true;
        ps.rateOverTime      = 20f;

        ps.burstOnPlay       = false;
        ps.burstCount        = 40;
        ps.burstSpeed        = 500f;
        ps.burstUpwardBias   = 0.3f;
        ps.burstSpawnRadius  = 0f;
        ps.burstRepeatInterval = 0f;

        ps.shape             = UIParticleSystem.EmissionShape.Cone;
        ps.angle             = 30f;
        ps.radius            = 0f;

        ps.colorOverLifetime = true;
        ps.sizeOverLifetime  = true;
        ps.sizeCurve         = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

        ps.velocityOverLifetime = false;
        ps.constantForce     = Vector2.zero;
        ps.turbulenceStrength = 0f;
        ps.dampingOverTime   = 0f;

        ps.rotationOverLifetime    = true;
        ps.rotationSpeed     = 90f;
        ps.randomRotationDirection = true;
    }

    private static Color C(float r, float g, float b) => new Color(r, g, b);

    private static AnimationCurve Curve(params float[] pairs)
    {
        var keys = new Keyframe[pairs.Length / 2];
        for (int i = 0; i < keys.Length; i++)
            keys[i] = new Keyframe(pairs[i * 2], pairs[i * 2 + 1]);
        return new AnimationCurve(keys);
    }

    private static Gradient MakeGradient(Color[] colors, float[] colorTimes,
                                          float[] alphaTimes, float[] alphaValues)
    {
        var g  = new Gradient();
        var ck = new GradientColorKey[colors.Length];
        for (int i = 0; i < colors.Length; i++)
            ck[i] = new GradientColorKey(colors[i], colorTimes[i]);

        var ak = new GradientAlphaKey[alphaTimes.Length];
        for (int i = 0; i < alphaTimes.Length; i++)
            ak[i] = new GradientAlphaKey(alphaValues[i], alphaTimes[i]);

        g.SetKeys(ck, ak);
        return g;
    }

    private static Gradient MakeRainbowGradient()
    {
        var g = new Gradient();
        g.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(C(1f,0.25f,0.25f), 0f),
                new GradientColorKey(C(1f,0.65f,0f),    0.17f),
                new GradientColorKey(C(1f,1f,0f),        0.33f),
                new GradientColorKey(C(0.3f,1f,0.3f),   0.5f),
                new GradientColorKey(C(0.25f,0.55f,1f), 0.67f),
                new GradientColorKey(C(0.6f,0.25f,1f),  0.83f),
                new GradientColorKey(C(1f,0.4f,0.8f),   1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 0.6f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        return g;
    }
}
