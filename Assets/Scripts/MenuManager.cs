using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }


    [Header("--- Screens ---")]
    public GameObject MenuScreen;
    public GameObject GameplayScreen;

    [Header("--- Buttons ---")]
    public Button StartButton;
    public Button AnalyticsButton;
    public Button ExitButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        InitializeButtonsEvents();
        ShowMenu();
    }

    private void InitializeButtonsEvents()
    {
        if (StartButton != null)
            StartButton.onClick.AddListener(OnStartButtonClicked);

        if (AnalyticsButton != null)
            AnalyticsButton.onClick.AddListener(OnAnalyticsButtonClicked);

        if (ExitButton != null)
            ExitButton.onClick.AddListener(OnExitButtonClicked);
    }

    #region BUTTON_CLICKS_EVENTS
    private void OnStartButtonClicked()
    {
        GameManager.Instance.StartGame();
        ShowGameplay();
    }

    private void OnAnalyticsButtonClicked()
    {

    }

    private void OnExitButtonClicked()
    {

        Application.Quit();
    }
    #endregion

    private void ShowMenu()
    {
        if (MenuScreen != null)
            MenuScreen.SetActive(true);

        if (GameplayScreen != null)
            GameplayScreen.SetActive(false);
    }

    private void ShowGameplay()
    {
        if (MenuScreen != null)
            MenuScreen.SetActive(false);

        if (GameplayScreen != null)
            GameplayScreen.SetActive(true);
    }
}
