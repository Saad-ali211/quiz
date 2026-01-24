using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }

    //store loaded questions
    public QuizQuestion[] Questions { get; private set; }

    //Make it singleton
    private void Awake()
    {
        // Singleton enforcement
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        LoadQuestions();
    }

    //Load the questions from json file
    private void LoadQuestions()
    {
        // Load the JSON file from Resources folder
        TextAsset jsonData = Resources.Load<TextAsset>("quiz_questions"); // file path: Assets/Resources/quiz_questions.json
        if (jsonData == null)
        {
            Debug.LogError("quiz_questions.json not found in Resources folder!");
            return;
        }
        // JsonUtility cannot parse raw arrays, so wrap the JSON in an object dynamically
        string wrappedJson = "{\"questions\":" + jsonData.text + "}";
        QuizQuestionList questionList = JsonUtility.FromJson<QuizQuestionList>(wrappedJson);

        Questions = questionList.questions;
    }

    //Get a question by index - called from other script
    public QuizQuestion GetQuestionByIndex(int index)
    {
        if (Questions == null || Questions.Length == 0)
        {
            Debug.LogWarning("No questions loaded.");
            return null;
        }

        if (index < 0 || index >= Questions.Length)
        {
            Debug.LogWarning($"Index {index} is out of range. Valid range: 0-{Questions.Length - 1}");
            return null;
        }

        return Questions[index];
    }
}


// ----------- C# class to represent a single quiz question --------------------
[System.Serializable]
public class QuizQuestion
{
    public string question;
    public string[] options;
    public int correctIndex;
    public string category;
    public string difficulty;
}

// --------- Wrapper class to allow JsonUtility to parse array -----------------
[System.Serializable]
public class QuizQuestionList
{
    public QuizQuestion[] questions;
}
