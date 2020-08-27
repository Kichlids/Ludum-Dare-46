using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item")]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    public ItemTarget itemTarget;
    public ItemType itemType;
    public Readyness itemReadyness;

    public Sprite myIcon;
    public Sprite myIcon_Over;
    public Sprite myIcon_Under;

    [Header("Values")]
    public float hungerValue;
    public float thirstValue;
    public float sanityValue;
    public float diaperValue;
    public float attentionValue;

    public float timeUntilPrepared;
    public float timeUntilOverPrepared;

    public float underPreparedPenaltyAdder;
    public float overPreparedPenaltyMultiplier;
}
