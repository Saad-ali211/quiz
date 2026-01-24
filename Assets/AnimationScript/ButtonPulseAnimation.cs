using System.Collections;
using UnityEngine;

public class ButtonPulseAnimation : MonoBehaviour
{
    [Header("--- Pulse Settings ---")]
    [SerializeField] private float pulseDuration = 1f; // Time for one full pulse cycle
    [SerializeField] private float minScale = 0.9f;
    [SerializeField] private float maxScale = 1.1f;

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private bool isAnimating = true;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        Debug.Log("Button Pulse Animation Started");
        StartCoroutine(PulseCoroutine());
    }

    private IEnumerator PulseCoroutine()
    {
        while (isAnimating)
        {
            // Zoom in
            yield return StartCoroutine(ZoomTo(maxScale, pulseDuration / 2));

            // Zoom out
            yield return StartCoroutine(ZoomTo(minScale, pulseDuration / 2));
        }
    }

    private IEnumerator ZoomTo(float targetScale, float duration)
    {
        float elapsedTime = 0f;
        float startScale = rectTransform.localScale.x;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);
            float newScale = Mathf.Lerp(startScale, targetScale, progress);

            rectTransform.localScale = originalScale * newScale;

            yield return null;
        }

        rectTransform.localScale = originalScale * targetScale;
    }

    public void StopPulse()
    {
        isAnimating = false;
        rectTransform.localScale = originalScale;
    }
}