using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Demo: shows how to use UIParticleSystem in your game.
///
/// SETUP:
///   1. Create Canvas (Render Mode: Screen Space - Overlay)
///   2. Add empty GameObject inside Canvas → name it "ParticleEmitter"
///   3. Add UIParticleSystem component to it
///   4. Attach THIS script to any GameObject and assign the reference
///   5. Press Play!
/// </summary>
public class UIParticleSystemDemo : MonoBehaviour
{
    [Header("Reference")]
    public UIParticleSystem particleSystem;

    [Header("Demo Controls")]
    public bool autoSwitchPresets = true;
    [Range(2f, 8f)] public float switchInterval = 4f;

    private int _presetIndex = 0;
    private float _timer = 0f;

    private readonly string[] _presetNames = { "Fire", "Stars", "Smoke", "Magic", "Rain" };

    private void Start()
    {
        if (particleSystem == null)
        {
            Debug.LogError("UIParticleSystem reference not assigned!");
            return;
        }

        ApplyPreset(_presetIndex);
        particleSystem.Play();
    }

    private void Update()
    {
        if (!autoSwitchPresets) return;

        _timer += Time.deltaTime;
        if (_timer >= switchInterval)
        {
            _timer = 0f;
            _presetIndex = (_presetIndex + 1) % _presetNames.Length;
            ApplyPreset(_presetIndex);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Call any of these from buttons / game events
    // ─────────────────────────────────────────────────────────────────────────

    public void PlayFire()    { UIParticleSystemPresets.ApplyFire(particleSystem);     particleSystem.Play(); }
    public void PlayStars()   { UIParticleSystemPresets.ApplyStars(particleSystem);    particleSystem.Play(); }
    public void PlaySmoke()   { UIParticleSystemPresets.ApplySmoke(particleSystem);    particleSystem.Play(); }
    public void PlayMagic()   { UIParticleSystemPresets.ApplyMagic(particleSystem);    particleSystem.Play(); }
    public void PlayRain()    { UIParticleSystemPresets.ApplyRain(particleSystem);     particleSystem.Play(); }

    /// <summary>Fire a confetti burst — great for level complete screens!</summary>
    public void BurstConfetti()
    {
        UIParticleSystemPresets.ApplyConfetti(particleSystem);
        particleSystem.Emit(120); // One-shot burst of 120 particles
    }

    public void StopParticles()  => particleSystem.Stop();
    public void ClearParticles() => particleSystem.Clear();

    private void ApplyPreset(int index)
    {
        particleSystem.Clear();
        switch (index)
        {
            case 0: UIParticleSystemPresets.ApplyFire(particleSystem);  break;
            case 1: UIParticleSystemPresets.ApplyStars(particleSystem); break;
            case 2: UIParticleSystemPresets.ApplySmoke(particleSystem); break;
            case 3: UIParticleSystemPresets.ApplyMagic(particleSystem); break;
            case 4: UIParticleSystemPresets.ApplyRain(particleSystem);  break;
        }
        particleSystem.Play();
        Debug.Log($"[UIParticleSystem] Switched to preset: {_presetNames[index]}");
    }
}
