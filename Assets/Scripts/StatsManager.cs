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
    public int scores;

    private const int CORRECT_POINTS = 5;
    private const int WRONG_POINTS = 2;

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
        scores += CORRECT_POINTS;  // Add 5 points
                                   // Debug.Log($"Correct Answer! Total Score: {scores}");
    }

    private void HandleWrongAnswer(int questionIndex)
    {
        wrongAnswers++;
        unCorrectQuestions.text = wrongAnswers.ToString("D2");
        scores -= WRONG_POINTS;  // Subtract 2 points

        // Prevent score from going below 0
        scores = Mathf.Max(0, scores);

        //Debug.Log($"Wrong Answer! Total Score: {scores}");
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        QuestionUIManager.OnCorrectAnswer -= HandleCorrectAnswer;
        QuestionUIManager.OnWrongAnswer -= HandleWrongAnswer;
    }
}