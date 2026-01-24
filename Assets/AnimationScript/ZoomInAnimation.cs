using System.Collections;
using UnityEngine;

public class ZoomInAnimation : MonoBehaviour
{
    [Header("--- Animation Settings ---")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        PlayZoomInAnimation();
    }

    public void PlayZoomInAnimation()
    {
        // Set initial scale to zero
        rectTransform.localScale = Vector3.zero;

        // Start the animation coroutine
        StartCoroutine(ZoomInCoroutine());
    }

    private IEnumerator ZoomInCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / animationDuration);

            // Use animation curve for smooth easing
            float curveValue = animationCurve.Evaluate(progress);

            // Lerp from 0 to 1
            rectTransform.localScale = Vector3.one * curveValue;

            yield return null;
        }

        // Ensure final scale is exactly 1,1,1
        rectTransform.localScale = Vector3.one;
    }
}