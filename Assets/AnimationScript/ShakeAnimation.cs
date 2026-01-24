using System.Collections;
using UnityEngine;

public class ShakeAnimation : MonoBehaviour
{
    public static ShakeAnimation Instance { get; private set; }

    [Header("--- Shake Settings ---")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeIntensity = 4f;
    [SerializeField] private int shakeFrequency = 4; // How many shakes per duration

    private RectTransform rectTransform;
    private Vector3 originalPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.localPosition;
    }

    public void PlayShakeAnimation()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;
        originalPosition = rectTransform.localPosition;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Smooth sine wave shake left and right
            float progress = elapsedTime / shakeDuration;
            float shakeX = Mathf.Sin(progress * shakeFrequency * Mathf.PI * 2) * shakeIntensity;

            rectTransform.localPosition = originalPosition + new Vector3(shakeX, 0, 0);

            yield return null;
        }

        // Reset to original position
        rectTransform.localPosition = originalPosition;
    }
}