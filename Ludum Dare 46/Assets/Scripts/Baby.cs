using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : Interactable
{
    public bool babyName;
    public Animator myAnim;

    private void Update() {
        if (GameManager._instance.babyHungerCurrent <= 0 || GameManager._instance.babyThirstCurrent <= 0) {
            GameManager._instance.isGameLost = true;
            print("Baby is dead");
            enabled = false;
        }

        if (GameManager._instance.babyHungerCurrent / GameInfo.babyHungerMax <= 0.66 ||
            GameManager._instance.babyThirstCurrent / GameInfo.babyThirstMax <= 0.66 ||
            GameManager._instance.babyDiaperCurrent / GameInfo.babyDiaperMax <= 0.5 ||
            GameManager._instance.babyAttentionCurrent / GameInfo.babyAttentionMax <= 0.5)
        {
            myAnim.SetBool("isCrying", true);
        }
        else
        {
            myAnim.SetBool("isCrying", false);
        }
    }

    
    public override void Interact() {

        if (player.hasItem) {
            if (player.item.myTarget == ItemTarget.Baby || player.item.myTarget == ItemTarget.Both) {

                string itemName = player.item.itemName;

                player.item.EvaluateReadyness();
                GameManager._instance.Consume(ItemTarget.Baby, player.GiveItem());

                string action = interactableName + " consumed " + itemName;
                UIManager._instance.enableActionText(action);
            }
            else {
                print("This item is for an adult");
            }
        }
        else {
            print("Player has no item to interact");
        }

    }
}
