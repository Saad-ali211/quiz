using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StatisticsViewManager : MonoBehaviour
{
    public static StatisticsViewManager Instance { get; private set; }

    public TextMeshProUGUI correctAnswersCount;
    public TextMeshProUGUI InCorrectAnswersCount;
    public TextMeshProUGUI MainScores;

    public Button ReplayButton;
    public Button ExitButton;

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
        // Add button listeners
        ReplayButton.onClick.AddListener(OnReplayButtonClicked);
        ExitButton.onClick.AddListener(OnExitButtonClicked);

        //update stats
        UpdateStatistics();
    }

    private void OnReplayButtonClicked()
    {

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnExitButtonClicked()
    {

        Application.Quit();

    }

    public void UpdateStatistics()
    {
        correctAnswersCount.text = StatsManager.Instance.correctAnswers.ToString("D2");
        InCorrectAnswersCount.text = StatsManager.Instance.wrongAnswers.ToString("D2");
        MainScores.text = StatsManager.Instance.scores.ToString("D2");
    }
}