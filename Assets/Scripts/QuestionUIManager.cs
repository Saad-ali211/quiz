using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public int currentQuestionIndex = 0;
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

    private void OnOptionSelected(int optionIndex)
    {
        if (QuestionManager.Instance.Questions[currentQuestionIndex].correctIndex == optionIndex)
        {
            switch (optionIndex)
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
        }
        else
        {


        }
        StartCoroutine(ShowNextQuestionAfterDelay(2.0f));
    }

    private IEnumerator ShowNextQuestionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowNextQuestion();

    }


    private void ShowNextQuestion()
    {
        QuizQuestion question = QuestionManager.Instance.GetQuestionByIndex(currentQuestionIndex);
        questionStatement.text = question.question;
        option1.transform.Find("OptionStatement").GetComponent<TextMeshProUGUI>().text = "A. " + question.options[0];
        option2.transform.Find("OptionStatement").GetComponent<TextMeshProUGUI>().text = "B. " + question.options[1];
        option3.transform.Find("OptionStatement").GetComponent<TextMeshProUGUI>().text = "C. " + question.options[2];
        option4.transform.Find("OptionStatement").GetComponent<TextMeshProUGUI>().text = "D. " + question.options[3];

        if (currentQuestionIndex <= QuestionManager.Instance.Questions.Length)
            currentQuestionIndex++;
        else
            currentQuestionIndex = 0;
    }
}
