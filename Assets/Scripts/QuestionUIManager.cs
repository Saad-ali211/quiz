using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class QuestionUIManager : MonoBehaviour
{
    [Header("--- Question with options Texts ---")]
    public TextMeshProUGUI questionStatement;
    public Button option1;
    public Button option2;
    public Button option3;
    public Button option4;

    [Header("--- Question Number ---")]
    public TextMeshProUGUI questionNumber;

    [Header("--- Question Heading ---")]
    public TextMeshProUGUI questionHeading;


    // ------ Events ------
    public static event Action<int> OnCorrectAnswer;
    public static event Action<int> OnWrongAnswer;

    // ------ current question ------
    public int currentQuestionIndex = -1;

    // ------ counts of correct / uncorrect questions ------
    public int correctQuestionsCount = 0;
    public int unCorrectQuestionsCount = 0;

    private Button[] optionButtons;

    private void Start()
    {
        // Store all buttons in array for easy access
        optionButtons = new Button[] { option1, option2, option3, option4 };

        // Add single click listener to all buttons with parameter
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int optionIndex = i; // Local copy for closure
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(optionIndex));
        }

        ShowNextQuestion();
    }

    // when user clicks on option
    private void OnOptionSelected(int selectedOptionIndex)
    {
        Timer.Instance.StopTimer();

        // show the correct green image for correct answer
        switch (QuestionManager.Instance.Questions[currentQuestionIndex].correctIndex)
        {
            case 0:
                option1.transform.Find("correct").gameObject.SetActive(true);
                option1.transform.Find("Uncorrect").gameObject.SetActive(false);
                break;
            case 1:
                option2.transform.Find("correct").gameObject.SetActive(true);
                option2.transform.Find("Uncorrect").gameObject.SetActive(false);
                break;
            case 2:
                option3.transform.Find("correct").gameObject.SetActive(true);
                option3.transform.Find("Uncorrect").gameObject.SetActive(false);
                break;
            case 3:
                option4.transform.Find("correct").gameObject.SetActive(true);
                option4.transform.Find("Uncorrect").gameObject.SetActive(false);
                break;
            default:
                break;
        }


        if (QuestionManager.Instance.Questions[currentQuestionIndex].correctIndex != selectedOptionIndex)
        {
            // wrong answer
            switch (selectedOptionIndex)
            {
                case 0:
                    option1.transform.Find("Uncorrect").gameObject.SetActive(true);
                    option1.GetComponent<Image>().color = new Color(1f, 0.847f, 0.847f);
                    break;
                case 1:
                    option2.transform.Find("Uncorrect").gameObject.SetActive(true);
                    option2.GetComponent<Image>().color = new Color(1f, 0.847f, 0.847f);
                    break;
                case 2:
                    option3.transform.Find("Uncorrect").gameObject.SetActive(true);
                    option3.GetComponent<Image>().color = new Color(1f, 0.847f, 0.847f);
                    break;
                case 3:
                    option4.transform.Find("Uncorrect").gameObject.SetActive(true);
                    option4.GetComponent<Image>().color = new Color(1f, 0.847f, 0.847f);
                    break;
                default:
                    break;
            }
            // fire event
            OnWrongAnswer?.Invoke(currentQuestionIndex);
        }
        else
        {

            // correct answer
            OnCorrectAnswer?.Invoke(currentQuestionIndex);
        }

        StartCoroutine(ShowNextQuestionAfterDelay(2.0f));
    }

    private void HideAllCorrectUncorrectImages()
    {
        // hide all the feedback images
        option1.transform.Find("correct").gameObject.SetActive(false);
        option1.transform.Find("Uncorrect").gameObject.SetActive(false);

        option2.transform.Find("correct").gameObject.SetActive(false);
        option2.transform.Find("Uncorrect").gameObject.SetActive(false);

        option3.transform.Find("correct").gameObject.SetActive(false);
        option3.transform.Find("Uncorrect").gameObject.SetActive(false);

        option4.transform.Find("correct").gameObject.SetActive(false);
        option4.transform.Find("Uncorrect").gameObject.SetActive(false);

        // reset the main item color
        option1.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        option2.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        option3.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        option4.GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }

    private IEnumerator ShowNextQuestionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowNextQuestion();

    }


    private void ShowNextQuestion()
    {
        // Increment AFTER displaying the question
        currentQuestionIndex++;

        // Check if quiz is complete BEFORE trying to access questions
        if (currentQuestionIndex >= QuestionManager.Instance.Questions.Length)
        {
            Debug.Log("Quiz Complete!");

            return;
        }


        // hide feedback images
        HideAllCorrectUncorrectImages();

        //Reset the timer
        Timer.Instance.RestartTimer();

        //get the question and populate the ui
        QuizQuestion question = QuestionManager.Instance.GetQuestionByIndex(currentQuestionIndex);
        questionStatement.text = question.question;
        option1.transform.Find("OptionStatement").GetComponent<TextMeshProUGUI>().text = "A. " + question.options[0];
        option2.transform.Find("OptionStatement").GetComponent<TextMeshProUGUI>().text = "B. " + question.options[1];
        option3.transform.Find("OptionStatement").GetComponent<TextMeshProUGUI>().text = "C. " + question.options[2];
        option4.transform.Find("OptionStatement").GetComponent<TextMeshProUGUI>().text = "D. " + question.options[3];

        // update the question number heading
        questionHeading.GetComponent<TextMeshProUGUI>().text = "Question " + (currentQuestionIndex + 1);


        print("currentQuestionIndex : " + currentQuestionIndex);
    }
}
