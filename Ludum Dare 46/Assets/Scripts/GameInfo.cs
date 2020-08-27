using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo
{
    [Header("Player Variables")]
    public static float playerHungerMax = 100;
    public static float playerThirstMax = 100;
    public static float playerSanityMax = 100;

    public static float playerSanityThreshold = 30f;

    public static float playerHungerDecreaseRate = .45f;
    public static float playerThirstDecreaseRate = .7f;
    public static float playerSanityDecreaseRate = 1;
           
    public static float playerInteractableRange = 3f;
           
    [Header("Baby Variables")]
    public static float babyHungerMax = 100;
    public static float babyThirstMax = 100;
    public static float babyDiaperMax = 100;
    public static float babyAttentionMax = 100;
           
    public static float babyHungerDecreaseRate = .65f;
    public static float babyThirstDecreaseRate = 1;
    public static float babyDiaperDecreaseRate = .65f;
    public static float babyAttentionDecreaseRate = .5f;
    
    public static float evaluationThresholdGood = 80;
    public static float evaluationThresholdAverage = 40;
    public static string evaluationGoodAnalysis = "Congratulations! Thanks to your actions, the baby grew up to be an important member " +
                                                    "of society!";
    public static string evaluationAverageAnalysis = "You may have been a babysitting noob, but the baby overcame your shortcomings and became a fully functioning human.";
    public static string evaluationBadAnalysis = "The trauma you inflicted impacted the baby's future negatively. The baby now makes living by doing shady businesses in dark alleyways";
}
