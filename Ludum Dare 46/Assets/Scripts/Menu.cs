using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Jobs;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    public static Menu _instance;

    public GameObject UIPanel01;
    public GameObject UIPanel02;

    public GameObject RulesPanel01;
    public GameObject RulesPanel02;
    public GameObject RulesPanel03;
    public GameObject RulesPanel04;

    public GameObject Credits;

    public bool isPlayingAsBoy;
    public string babyName;

    public Button boyButton;
    public Button girlButton;
    public Button exitButton;
    public TMP_InputField babyNameField;
    public TextMeshProUGUI errorMsg;

    private void Awake() {
        _instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UIPanel01.SetActive(true);
        UIPanel02.SetActive(false);

        RulesPanel01.SetActive(false);
        RulesPanel02.SetActive(false);
        RulesPanel03.SetActive(false);
        RulesPanel04.SetActive(false);

        Credits.SetActive(false);
    }

    public void OnPlayButton()
    {
        UIPanel01.SetActive(false);
        UIPanel02.SetActive(true);

        RulesPanel01.SetActive(false);
        RulesPanel02.SetActive(false);
        RulesPanel03.SetActive(false);
        RulesPanel04.SetActive(false);

        Credits.SetActive(false);
    }

    public void OnBoyButton() {
        isPlayingAsBoy = true;

        if (CheckForCompletePlayerInput()) {
            LaunchGame();
        }
    }

    public void OnGirlButton() {
        isPlayingAsBoy = false;

        if (CheckForCompletePlayerInput()) {
            LaunchGame();
        }
    }

    public bool CheckForCompletePlayerInput() {
        if (babyNameField.text == "") {
            errorMsg.text = "Enter baby's name";
            return false;
        }
        else {
            babyName = babyNameField.text;
            errorMsg.text = "Launching...";
            return true;
        }
    }

    private void LaunchGame() {
        print("Launching game");

        SceneManager.LoadScene(1);
    }

    public void OnExitButton() {
        Application.Quit();
    }

    public void ShowRules01()
    {
        UIPanel01.SetActive(false);
        UIPanel02.SetActive(false);

        RulesPanel01.SetActive(true);
        RulesPanel02.SetActive(false);
        RulesPanel03.SetActive(false);
        RulesPanel04.SetActive(false);

        Credits.SetActive(false);
    }

    public void ShowRules02()
    {
        UIPanel01.SetActive(false);
        UIPanel02.SetActive(false);

        RulesPanel01.SetActive(false);
        RulesPanel02.SetActive(true);
        RulesPanel03.SetActive(false);
        RulesPanel04.SetActive(false);

        Credits.SetActive(false);
    }

    public void ShowRules03()
    {
        UIPanel01.SetActive(false);
        UIPanel02.SetActive(false);

        RulesPanel01.SetActive(false);
        RulesPanel02.SetActive(false);
        RulesPanel03.SetActive(true);
        RulesPanel04.SetActive(false);

        Credits.SetActive(false);
    }

    public void ShowRules04()
    {
        UIPanel01.SetActive(false);
        UIPanel02.SetActive(false);

        RulesPanel01.SetActive(false);
        RulesPanel02.SetActive(false);
        RulesPanel03.SetActive(false);
        RulesPanel04.SetActive(true);

        Credits.SetActive(false);
    }

    public void ExitRules()
    {
        UIPanel01.SetActive(true);
        UIPanel02.SetActive(false);

        RulesPanel01.SetActive(false);
        RulesPanel02.SetActive(false);
        RulesPanel03.SetActive(false);
        RulesPanel04.SetActive(false);

        Credits.SetActive(false);
    }

    public void ShowCredits()
    {
        UIPanel01.SetActive(false);
        UIPanel02.SetActive(false);

        RulesPanel01.SetActive(false);
        RulesPanel02.SetActive(false);
        RulesPanel03.SetActive(false);
        RulesPanel04.SetActive(false);

        Credits.SetActive(true);
    }

    public void HideCredits()
    {
        UIPanel01.SetActive(true);
        UIPanel02.SetActive(false);

        RulesPanel01.SetActive(false);
        RulesPanel02.SetActive(false);
        RulesPanel03.SetActive(false);
        RulesPanel04.SetActive(false);

        Credits.SetActive(false);
    }
}
