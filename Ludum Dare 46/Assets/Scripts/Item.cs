using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType { Food, Drink }
public enum Readyness { Undercooked, Ready, Overcooked };
public enum ItemTarget { Adult, Baby, Both };

[System.Serializable]
public class Item
{
    public string itemName;
    public ItemType myType;
    public ItemTarget myTarget;
    public Readyness readyness;

    public Sprite myIcon;
    public Sprite myIcon_Over;
    public Sprite myIcon_Under;

    [Header("Values")]
    public float hungerValue;
    public float thirstValue;
    public float sanityValue;
    public float diaperValue;
    public float attentionValue;

    [Header("Cooking Variables")]
    [SerializeField]
    public float preparedTime;
    public float timeUntilPrepared;
    public float timeUntilOverPrepared;

    public float underPreparedPenaltyAdder;
    public float overPreparedPenaltyMultiplier;

    private void Update() {
        readyness = GetReadyness();
    }

    public Item() { }

    public void BuildItemFromScriptableObject(ItemScriptableObject scriptableItem)
    {
        this.itemName = scriptableItem.itemName;
        this.myType = scriptableItem.itemType;
        this.myTarget = scriptableItem.itemTarget;
        this.readyness = scriptableItem.itemReadyness;
        this.hungerValue = scriptableItem.hungerValue;
        this.thirstValue = scriptableItem.thirstValue;
        this.sanityValue = scriptableItem.sanityValue;
        this.diaperValue = scriptableItem.diaperValue;
        this.attentionValue = scriptableItem.attentionValue;
        this.preparedTime = 0;
        this.timeUntilPrepared = scriptableItem.timeUntilPrepared;
        this.timeUntilOverPrepared = scriptableItem.timeUntilOverPrepared;
        this.underPreparedPenaltyAdder = scriptableItem.underPreparedPenaltyAdder;
        this.overPreparedPenaltyMultiplier = scriptableItem.overPreparedPenaltyMultiplier;
        this.myIcon = scriptableItem.myIcon;
        this.myIcon_Over = scriptableItem.myIcon_Over;
        this.myIcon_Under = scriptableItem.myIcon_Under;
    }

    public float GetPreparedTime() {
        return preparedTime;
    }

    public void SetPreparedTime(float time) {
        preparedTime = time;
    }

    public Readyness GetReadyness() {
        if (preparedTime > timeUntilOverPrepared) {
            return Readyness.Overcooked;
        }
        else if (preparedTime > timeUntilPrepared) {
            return Readyness.Ready;
        }
        else {
            return Readyness.Undercooked;
        }
    }

    public void EvaluateReadyness() {
        this.readyness = GetReadyness();
    }
}
