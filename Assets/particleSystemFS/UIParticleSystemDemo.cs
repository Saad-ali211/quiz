using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Demonstrates how to use UIParticleSystem burst + gravity arcs.
///
/// QUICK SETUP:
///   1. Canvas (Screen Space - Overlay)
///      └── ParticleEmitter (empty GO)
///              └── UIParticleSystem component
///   2. Assign particleSystem reference below
///   3. Wire buttons to the public methods
/// </summary>
public class UIParticleSystemDemo : MonoBehaviour
{
    [Header("References")]
    public UIParticleSystem particleSystem;

    private void Start()
    {
        if (particleSystem == null)
        {
            Debug.LogError("[UIParticleSystemDemo] Assign the UIParticleSystem reference!");
            return;
        }

        // Default: confetti burst — feels great for level complete / coin collect
        SetupConfettiBurst();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // BURST EXAMPLES  — wire these to UI Buttons
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Classic confetti explosion. Particles fly in all directions, gravity
    /// pulls them down in natural arcs. Great for: level complete, win screen.
    /// </summary>
    public void BurstConfetti()
    {
        SetupConfettiBurst();
        particleSystem.EmitBurst();         // fire 80 particles right now
    }

    /// <summary>
    /// Fountain: particles shoot mostly upward, gravity arcs them back down.
    /// Great for: coin collect, reward, score popup.
    /// </summary>
    public void BurstFountain()
    {
        particleSystem.Clear();
        particleSystem.startLifetime    = 1.6f;
        particleSystem.gravityModifier  = 600f;   // strong gravity → fast arc
        particleSystem.burstSpeed       = 700f;
        particleSystem.burstCount       = 40;
        particleSystem.burstUpwardBias  = 0.85f;  // mostly upward
        particleSystem.burstSpawnRadius = 10f;
        particleSystem.startSize        = 14f;
        particleSystem.sizeOverLifetime = true;
        particleSystem.rotationOverLifetime = true;
        particleSystem.rotationSpeed    = 360f;

        // Gold gradient
        Gradient g = new Gradient();
        g.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white,              0f),
                new GradientColorKey(new Color(1f, 0.9f, 0f), 0.3f),
                new GradientColorKey(new Color(1f, 0.5f, 0f), 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 0.5f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        particleSystem.colorGradient = g;
        particleSystem.EmitBurst();
    }

    /// <summary>
    /// Small tap feedback burst. Low count, quick arc. Good for button presses.
    /// </summary>
    public void BurstTapFeedback()
    {
        particleSystem.Clear();
        particleSystem.startLifetime    = 0.7f;
        particleSystem.gravityModifier  = 800f;   // very fast arc = snappy feel
        particleSystem.burstSpeed       = 350f;
        particleSystem.burstCount       = 15;
        particleSystem.burstUpwardBias  = 0.5f;
        particleSystem.burstSpawnRadius = 5f;
        particleSystem.startSize        = 10f;
        particleSystem.sizeOverLifetime = true;
        particleSystem.rotationOverLifetime = false;

        Gradient g = new Gradient();
        g.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white, 0f),
                new GradientColorKey(new Color(0.4f, 0.8f, 1f), 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        particleSystem.colorGradient = g;
        particleSystem.EmitBurst();
    }

    /// <summary>
    /// Big explosion — full 360° with slow gravity so particles fly far.
    /// Good for: boss defeat, big achievement.
    /// </summary>
    public void BurstExplosion()
    {
        particleSystem.Clear();
        particleSystem.startLifetime    = 2.5f;
        particleSystem.gravityModifier  = 250f;   // gentle gravity = wide spread
        particleSystem.burstSpeed       = 900f;
        particleSystem.burstCount       = 120;
        particleSystem.burstUpwardBias  = 0f;     // full 360° sphere
        particleSystem.burstSpawnRadius = 30f;
        particleSystem.startSize        = 18f;
        particleSystem.sizeOverLifetime = true;
        particleSystem.rotationOverLifetime = true;
        particleSystem.rotationSpeed    = 270f;

        // Random vivid colors
        Gradient g = new Gradient();
        g.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white,              0f),
                new GradientColorKey(new Color(1f, 0.3f, 0.3f), 0.4f),
                new GradientColorKey(new Color(1f, 0.8f, 0f), 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0.8f, 0.6f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        particleSystem.colorGradient = g;
        particleSystem.EmitBurst();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // CONTINUOUS PRESETS
    // ─────────────────────────────────────────────────────────────────────────
    public void PlayFire()  { UIParticleSystemPresets.ApplyFire(particleSystem);  particleSystem.Play(); }
    public void PlayStars() { UIParticleSystemPresets.ApplyStars(particleSystem); particleSystem.Play(); }
    public void PlayMagic() { UIParticleSystemPresets.ApplyMagic(particleSystem); particleSystem.Play(); }
    public void PlayRain()  { UIParticleSystemPresets.ApplyRain(particleSystem);  particleSystem.Play(); }

    public void StopParticles()  => particleSystem.Stop();
    public void ClearParticles() => particleSystem.Clear();

    // ─────────────────────────────────────────────────────────────────────────
    // HELPERS
    // ─────────────────────────────────────────────────────────────────────────
    private void SetupConfettiBurst()
    {
        particleSystem.Clear();

        // No continuous emission — pure burst only
        particleSystem.emission         = false;
        particleSystem.loop             = false;

        // Burst settings
        particleSystem.burstCount       = 80;
        particleSystem.burstSpeed       = 600f;
        particleSystem.burstUpwardBias  = 0.4f;  // slight upward lean
        particleSystem.burstSpawnRadius = 20f;   // particles spawn in a small area

        // Physics
        particleSystem.startLifetime   = 2.2f;
        particleSystem.gravityModifier = 450f;   // pulls particles into nice arcs

        // Look
        particleSystem.startSize        = 16f;
        particleSystem.sizeOverLifetime = false; // keep size constant for confetti feel
        particleSystem.rotationOverLifetime = true;
        particleSystem.rotationSpeed    = 360f;
        particleSystem.randomRotationDirection = true;

        // Colorful gradient
        Gradient g = new Gradient();
        g.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(new Color(1f, 0.3f, 0.3f), 0f),
                new GradientColorKey(new Color(1f, 0.9f, 0f),   0.25f),
                new GradientColorKey(new Color(0.3f, 1f, 0.3f), 0.5f),
                new GradientColorKey(new Color(0.3f, 0.6f, 1f), 0.75f),
                new GradientColorKey(new Color(1f, 0.3f, 1f),   1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 0.6f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        particleSystem.colorGradient = g;
    }
}
