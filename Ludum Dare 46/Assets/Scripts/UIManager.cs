using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;

    private GameManager gameManager;

    [Header("Player variables")]
    float playerHungerPercent;
    float playerThirstPercent;
    float playerSanityPercent;

    [Header("Baby variables")]
    float babyHungerPercent;
    float babyThirstPercent;
    float babyDiaperPercent;
    float babyAttentionPercent;

    [Header("Inventory")]
    public Image inventorySlot;
    public Image inventoryItem;
    public TextMeshProUGUI inventoryItemText;

    [Header("Interaction")]
    public Image interactFillBar;
    bool isInteracting = false;
    public TextMeshProUGUI actionText;
    public bool actionActive = false;
    public float maxTimeActionActive = 5f;
    public float timeActionActive = 0;

    [Header("UI Components")]
    public Slider playerHungerSlider;
    public Slider playerThirstSlider;
    public Slider playerSanitySlider;

    public Slider babyHungerSlider;
    public Slider babyThirstSlider;
    public Slider babyDiaperSlider;
    public Slider babyAttentionSlider;

    public Slider progressSlider;

    [Header("Pause Screen")]
    public GameObject PauseScreen;

    [Header("Death Screen")]
    public GameObject deathScreen;

    [Header("EndScreen")]
    public GameObject endScreen;
    public TextMeshProUGUI playerAvgHunger;
    public TextMeshProUGUI playerAvgThirst;
    public TextMeshProUGUI playerAvgSanity;
    public Slider playerAvgHungerSlider;
    public Slider playerAvgThirstSlider;
    public Slider playerAvgSanitySlider;

    public TextMeshProUGUI babyAvgHunger;
    public TextMeshProUGUI babyAvgThirst;
    public TextMeshProUGUI babyAvgDiaper;
    public TextMeshProUGUI babyAvgAttention;
    public Slider babyAvgHungerSlider;
    public Slider babyAvgThirstSlider;
    public Slider babyAvgDiaperSlider;
    public Slider babyAvgAttentionSlider;

    public TextMeshProUGUI overallScore;
    public TextMeshProUGUI analysis;

    private bool isGameLost = false;

    private bool isGameOver = false;
    private float timeAfterGameOver = 0;
    public float endScreenLerpRatio = 2f;


    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        gameManager = GameManager._instance;
        interactFillBar.fillAmount = 0;
        actionText.gameObject.SetActive(false);
        deathScreen.gameObject.SetActive(false);
        deathScreen.GetComponent<CanvasGroup>().alpha = 0;
        endScreen.gameObject.SetActive(false);
        endScreen.GetComponent<CanvasGroup>().alpha = 0;
    }

    void Update()
    {
        playerHungerPercent = gameManager.playerHungerCurrent / GameInfo.playerHungerMax;
        playerThirstPercent = gameManager.playerThirstCurrent / GameInfo.playerThirstMax;
        playerSanityPercent = gameManager.playerSanityCurrent / GameInfo.playerSanityMax;

        babyHungerPercent = gameManager.babyHungerCurrent / GameInfo.babyHungerMax;
        babyThirstPercent = gameManager.babyThirstCurrent / GameInfo.babyThirstMax;
        babyDiaperPercent = gameManager.babyDiaperCurrent / GameInfo.babyDiaperMax;
        babyAttentionPercent = gameManager.babyAttentionCurrent / GameInfo.babyAttentionMax;
        
        playerHungerSlider.value = playerHungerPercent;
        playerThirstSlider.value = playerThirstPercent;
        playerSanitySlider.value = playerSanityPercent;

        babyHungerSlider.value = babyHungerPercent;
        babyThirstSlider.value = babyThirstPercent;
        babyDiaperSlider.value = babyDiaperPercent;
        babyAttentionSlider.value = babyAttentionPercent;

        progressSlider.value = GameManager._instance.time / GameManager._instance.gameDuration;

        if (actionText.gameObject.activeSelf) {
            timeActionActive += Time.deltaTime;

            if (timeActionActive > maxTimeActionActive) {
                timeActionActive = 0;
                actionText.text = "";
                actionText.gameObject.SetActive(false);
            }
        }
       

        if (isGameLost) {
            timeAfterGameOver += Time.deltaTime / endScreenLerpRatio;
            deathScreen.SetActive(true);
            deathScreen.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, timeAfterGameOver);
        }
        else if (isGameOver) {
            timeAfterGameOver += Time.deltaTime / endScreenLerpRatio;
            endScreen.SetActive(true);
            endScreen.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, timeAfterGameOver);
        }
    }

    public void enableActionText(string txt) {
        timeActionActive = 0;
        actionText.text = txt;
        actionText.gameObject.SetActive(true);
    }

    private void LateUpdate()
    {
        if (!isInteracting)
            interactFillBar.fillAmount = 0;

        isInteracting = false;
    }

    public void UpdateInteractingUI(float curr, float max)
    {
        isInteracting = true;
        interactFillBar.gameObject.SetActive(true);
        interactFillBar.fillAmount = curr / max;
    }

    public void ShowGameLostPanel() {
        isGameLost = true;
    }

    public void EvaluateEnding() {

        List<float> playerHungerSample = GameManager._instance.playerHungerSample;
        List<float> playerThirstSample = GameManager._instance.playerThirstSample;
        List<float> playerSanitySample = GameManager._instance.playerSanitySample;

        List<float> babyHungerSample = GameManager._instance.babyHungerSample;
        List<float> babyThirstSample = GameManager._instance.babyThirstSample;
        List<float> babyDiaperSample = GameManager._instance.babyDiaperSample;
        List<float> babyAttentionSample = GameManager._instance.babyAttentionSample;

        float playerHungerSum = 0;
        float playerThirstSum = 0;
        float playerSanitySum = 0;
        float babyHungerSum = 0;
        float babyThirstSum = 0;
        float babyDiaperSum = 0;
        float babyAttentionSum = 0;

        int sampleCount = playerHungerSample.Count;

        for (int i = 0; i < sampleCount; i++) {
            playerHungerSum += playerHungerSample[i];
            playerThirstSum += playerThirstSample[i];
            playerSanitySum += playerSanitySample[i];

            babyHungerSum += babyHungerSample[i];
            babyThirstSum += babyThirstSample[i];
            babyDiaperSum += babyDiaperSample[i];
            babyAttentionSum += babyAttentionSample[i];
        }

        float playerHungerAvg = playerHungerSum / sampleCount;
        float playerThirstAvg = playerThirstSum / sampleCount;
        float playerSanityAvg = playerSanitySum / sampleCount;

        float babyHungerAvg = babyHungerSum / sampleCount;
        float babyThirstAvg = babyThirstSum / sampleCount;
        float babyDiaperAvg = babyDiaperSum / sampleCount;
        float babyAttentionAvg = babyAttentionSum / sampleCount;

        playerAvgHunger.text = "Player Hunger: " + Mathf.Round(playerHungerAvg);
        playerAvgThirst.text = "Player Thirst: " + Mathf.Round(playerThirstAvg);
        playerAvgSanity.text = "Player Sanity: " + Mathf.Round(playerSanityAvg);
        playerAvgHungerSlider.value = playerHungerAvg / GameInfo.playerHungerMax;
        playerAvgThirstSlider.value = playerThirstAvg / GameInfo.playerThirstMax;
        playerAvgSanitySlider.value = playerSanityAvg / GameInfo.playerSanityMax;

        babyAvgHunger.text = Mathf.Round(babyHungerAvg) + ": Baby Hunger";
        babyAvgThirst.text = Mathf.Round(babyThirstAvg) + ": Baby Thirst";
        babyAvgDiaper.text = Mathf.Round(babyDiaperAvg) + ": Baby Diaper";
        babyAvgAttention.text = Mathf.Round(babyAttentionAvg) + ": Baby Attention";
        babyAvgHungerSlider.value = babyHungerAvg / GameInfo.babyHungerMax;
        babyAvgThirstSlider.value = babyThirstAvg / GameInfo.babyThirstMax;
        babyAvgDiaperSlider.value = babyDiaperAvg / GameInfo.babyDiaperMax;
        babyAvgAttentionSlider.value = babyAttentionAvg / GameInfo.babyAttentionMax;

        float avg = (babyHungerAvg + babyThirstAvg + playerSanityAvg +
            babyHungerAvg + babyThirstAvg + babyDiaperAvg + babyAttentionAvg) / 7;

        overallScore.text = "Overall Score: " + Mathf.Round(avg).ToString();

        if (avg >= GameInfo.evaluationThresholdGood) analysis.text = GameInfo.evaluationGoodAnalysis;
        else if (avg >= GameInfo.evaluationThresholdAverage) analysis.text = GameInfo.evaluationAverageAnalysis;
        else analysis.text = GameInfo.evaluationBadAnalysis;

        isGameOver = true;
    }

    public void OnQuitButton() {
        Application.Quit();
    }
    
    public void OnReturnToMenuButton() {
        SceneManager.LoadScene(0);
    }

    public void OnReplayButton()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
