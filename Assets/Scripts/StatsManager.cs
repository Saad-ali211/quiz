using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; private set; }

    [Header("--- Correct Questions Text ---")]
    public TextMeshProUGUI correctQuestions;

    [Header("--- Un-Correct Questions Text ---")]
    public TextMeshProUGUI unCorrectQuestions;

    public int correctAnswers;
    public int wrongAnswers;

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
        // Subscribe to events from QuestionUIManager
        QuestionUIManager.OnCorrectAnswer += HandleCorrectAnswer;
        QuestionUIManager.OnWrongAnswer += HandleWrongAnswer;
    }

    private void HandleCorrectAnswer(int questionIndex)
    {
        correctAnswers++;
        correctQuestions.text = correctAnswers.ToString("D2");
    }

    private void HandleWrongAnswer(int questionIndex)
    {
        wrongAnswers++;
        unCorrectQuestions.text = wrongAnswers.ToString("D2");
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        QuestionUIManager.OnCorrectAnswer -= HandleCorrectAnswer;
        QuestionUIManager.OnWrongAnswer -= HandleWrongAnswer;
    }
}