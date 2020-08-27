using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPreparer : Interactable
{
    public ItemType myType;
    public Item item;
    public bool hasItem = false;

    private void Update() {
        if (hasItem && item != null) {
            item.SetPreparedTime(item.GetPreparedTime() + Time.deltaTime);
        }
    }

    public override void Interact() {   
       
        if (hasItem) {
            if (player.hasItem) {
                isInteracting = false;
                print("Player is already holding an item");
            }
            // Retrieve item from preparer
            else {
                player.ReceiveItem(item);
                string itemName = item.itemName;
                item = null;
                hasItem = false;
                isInteracting = false;

                string action = "Received " + itemName + " from " + interactableName;
                UIManager._instance.enableActionText(action);
            }
        }
        else {   
            // Give item to preparer
            if (player.hasItem && player.item.myType == myType) {
                item = player.GiveItem();
                hasItem = true;
                isInteracting = true;

                string action = "Gave " + item.itemName + " to " + interactableName;
                UIManager._instance.enableActionText(action);
            }
        }
    }
}
