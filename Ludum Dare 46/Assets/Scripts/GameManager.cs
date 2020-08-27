using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public PlayerController player;

    public GameObject boyCharacter;
    public GameObject girlCharacter;

    public float gameDuration;
    public float time = 0;

    [Header("All Interactable Objects")]
    public List<GameObject> interactables = new List<GameObject>();
    //public GameObject[] interactables;

    [Header("Player variables")]
    public float playerHungerCurrent;
    public float playerThirstCurrent;
    public float playerSanityCurrent;

    [Header("Baby variables")]
    public float babyHungerCurrent;
    public float babyThirstCurrent;
    public float babyDiaperCurrent;
    public float babyAttentionCurrent;

    public bool isPaused = false;

    public List<float> playerHungerSample = new List<float>();
    public List<float> playerThirstSample = new List<float>();
    public List<float> playerSanitySample = new List<float>();

    public List<float> babyHungerSample = new List<float>();
    public List<float> babyThirstSample = new List<float>();
    public List<float> babyDiaperSample = new List<float>();
    public List<float> babyAttentionSample = new List<float>();

    public bool isGameOver = false;
    public bool isGameLost = false;


    void Awake()
    {
        _instance = this;

        if (Menu._instance.isPlayingAsBoy) {
            player = Instantiate(boyCharacter, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();
        }
        else {
            player = Instantiate(girlCharacter, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();
        }

        GameObject.FindGameObjectWithTag("Baby").GetComponent<Interactable>().interactableName = Menu._instance.babyName;

        Interactable[] inters = FindObjectsOfType(typeof(Interactable)) as Interactable[];
        foreach(Interactable inter in inters) {
            interactables.Add(inter.gameObject);
        }
    }

    void Start()
    {
        playerHungerCurrent = GameInfo.playerHungerMax;
        playerThirstCurrent = GameInfo.playerThirstMax;
        playerSanityCurrent = GameInfo.playerSanityMax;

        babyHungerCurrent = GameInfo.babyHungerMax;
        babyThirstCurrent = GameInfo.babyThirstMax;
        babyDiaperCurrent = GameInfo.babyDiaperMax;
        babyAttentionCurrent = GameInfo.babyAttentionMax;



        InvokeRepeating("TakePlayerBabyStatSamples", 1f, 1f);
    }

    void Update() {

        time += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGameStatus();
        }

        float playerCurrentHungerVal = playerHungerCurrent - Time.deltaTime * GameInfo.playerHungerDecreaseRate;
        playerHungerCurrent = Mathf.Clamp(playerCurrentHungerVal, 0, GameInfo.playerHungerMax);
        float playerCurrentThirstVal = playerThirstCurrent - Time.deltaTime * GameInfo.playerThirstDecreaseRate;
        playerThirstCurrent = Mathf.Clamp(playerCurrentThirstVal, 0, GameInfo.playerThirstMax);
        float playerCurrentSanityVal = playerSanityCurrent - Time.deltaTime * GameInfo.playerSanityDecreaseRate;
        playerSanityCurrent = Mathf.Clamp(playerCurrentSanityVal, 0, GameInfo.playerSanityMax);

        float babyCurrentHungerVal = babyHungerCurrent - Time.deltaTime * GameInfo.babyHungerDecreaseRate;
        babyHungerCurrent = Mathf.Clamp(babyCurrentHungerVal, 0, GameInfo.babyHungerMax);
        float babyCurrentThirstVal = babyThirstCurrent - Time.deltaTime * GameInfo.babyThirstDecreaseRate;
        babyThirstCurrent = Mathf.Clamp(babyCurrentThirstVal, 0, GameInfo.babyThirstMax);
        float babyCurrentDiaperVal = babyDiaperCurrent - Time.deltaTime * GameInfo.babyDiaperDecreaseRate;
        babyDiaperCurrent = Mathf.Clamp(babyCurrentDiaperVal, 0, GameInfo.babyDiaperMax);
        float babyAttentionVal = babyAttentionCurrent - Time.deltaTime * GameInfo.babyAttentionDecreaseRate;
        babyAttentionCurrent = Mathf.Clamp(babyAttentionVal, 0, GameInfo.babyAttentionMax);

        player.isDisoriented = playerSanityCurrent <= GameInfo.playerSanityThreshold;

        if (isGameLost) {
            // Show game lost screen
            UIManager._instance.ShowGameLostPanel();
            enabled = false;
        }

        if (time >= gameDuration) {
            isGameOver = true;
            print("Game is over");
            UIManager._instance.EvaluateEnding();
            enabled = false;
        }
    }

    public void Consume(ItemTarget target, Item item) {

        if (item.GetReadyness() == Readyness.Overcooked) {
            item.hungerValue *= item.overPreparedPenaltyMultiplier;
            item.thirstValue *= item.overPreparedPenaltyMultiplier;
            item.sanityValue *= item.overPreparedPenaltyMultiplier;
        }
        else if (item.GetReadyness() == Readyness.Undercooked) {
            item.hungerValue += item.underPreparedPenaltyAdder;
            item.thirstValue += item.underPreparedPenaltyAdder;
            item.sanityValue += item.underPreparedPenaltyAdder;
        }

        if (target == ItemTarget.Adult) {
            float currentHungerVal = playerHungerCurrent + item.hungerValue;
            playerHungerCurrent = Mathf.Clamp(currentHungerVal, 0, GameInfo.playerHungerMax);
            float currentThirstVal = playerThirstCurrent + item.thirstValue;
            playerThirstCurrent = Mathf.Clamp(currentThirstVal, 0, GameInfo.playerThirstMax);
            float currentSanityVal = playerSanityCurrent + item.sanityValue;
            playerSanityCurrent = Mathf.Clamp(currentSanityVal, 0, GameInfo.playerSanityMax);
        }
        else if (target == ItemTarget.Baby) {
            float currentHungerVal = babyHungerCurrent + item.hungerValue;
            babyHungerCurrent = Mathf.Clamp(currentHungerVal, 0, GameInfo.babyHungerMax);
            float currentThirstVal = babyThirstCurrent + item.thirstValue;
            babyThirstCurrent = Mathf.Clamp(currentThirstVal, 0, GameInfo.babyThirstMax);
            float currentDiaperVal = babyDiaperCurrent + item.diaperValue;
            babyDiaperCurrent = Mathf.Clamp(currentDiaperVal, 0, GameInfo.babyDiaperMax);
            float currentAttentionVal = babyAttentionCurrent + item.attentionValue;
            babyAttentionCurrent = Mathf.Clamp(currentAttentionVal, 0, GameInfo.babyAttentionMax);
        }
    }

    public void Consume(ItemTarget target, ResourceScriptableObject resource) {
        if (target == ItemTarget.Adult) {
            float currentHungerVal = playerHungerCurrent + resource.hungerGainRate * Time.deltaTime;
            playerHungerCurrent = Mathf.Clamp(currentHungerVal, 0, GameInfo.playerHungerMax);
            float currentThirstVal = playerThirstCurrent + resource.thirstGainRate * Time.deltaTime;
            playerThirstCurrent = Mathf.Clamp(currentThirstVal, 0, GameInfo.playerThirstMax);
            float currentSanityVal = playerSanityCurrent + resource.sanityGainRate * Time.deltaTime;
            playerSanityCurrent = Mathf.Clamp(currentSanityVal, 0, GameInfo.playerSanityMax);
        }
        else {
            print("Consume resource baby not implemented");
        }
    }

    private void TogglePauseGameStatus() {
        isPaused = !isPaused;
        UIManager._instance.PauseScreen.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    private void TakePlayerBabyStatSamples() {
        playerHungerSample.Add(playerHungerCurrent);
        playerThirstSample.Add(playerThirstCurrent);
        playerSanitySample.Add(playerSanityCurrent);

        babyHungerSample.Add(babyHungerCurrent);
        babyThirstSample.Add(babyThirstCurrent);
        babyDiaperSample.Add(babyDiaperCurrent);
        babyAttentionSample.Add(babyAttentionCurrent);
    }
}
