using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }



    [Header("--- UI ---")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("--- Warning Settings ---")]
    [SerializeField] private int warningTime = 5;
    [SerializeField] private Color normalColor = Color.black;
    [SerializeField] private Color warningColor = Color.red;

    public int RemainingTime { get; private set; }
    public bool IsRunning { get; private set; }

    // Event fired when timer reaches zero
    public static event Action OnTimerFinished;

    // Event fired every second
    public static event Action<int> OnTimerTick;

    private Coroutine timerCoroutine;

    private void Awake()
    {
        // Singleton enforcement
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        StartTimer(GameManager.Instance.sessionTime);
    }
    /// <summary>
    /// Starts or restarts the timer.
    /// </summary>
    public void StartTimer(int seconds)
    {

        RestartTimer();
    }

    /// <summary>
    /// Restarts the timer using the current startTimeInSeconds.
    /// </summary>
    public void RestartTimer()
    {
        StopTimer();

        RemainingTime = GameManager.Instance.sessionTime;
        IsRunning = true;

        UpdateTimerUI();
        OnTimerTick?.Invoke(GameManager.Instance.sessionTime);

        timerCoroutine = StartCoroutine(TimerRoutine());
    }

    /// <summary>
    /// Stops the timer without firing completion event.
    /// </summary>
    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        IsRunning = false;
    }

    private IEnumerator TimerRoutine()
    {
        while (RemainingTime > 0)
        {
            yield return new WaitForSeconds(1f);

            RemainingTime--;
            UpdateTimerUI();
            OnTimerTick?.Invoke(RemainingTime);
        }

        IsRunning = false;
        OnTimerFinished?.Invoke();
    }

    private void UpdateTimerUI()
    {
        if (timerText == null)
            return;

        timerText.text = RemainingTime.ToString();

        timerText.color = RemainingTime <= warningTime
            ? warningColor
            : normalColor;
    }
}
