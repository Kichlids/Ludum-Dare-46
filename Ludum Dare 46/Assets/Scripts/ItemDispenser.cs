using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDispenser : Interactable
{
    public ItemScriptableObject myItem;
    float interactTimeCurrent;
    float itemRespawnTime = 3;
    float itemRespawnStart = 0;

    void Update()
    {
        if (Time.time >= itemRespawnStart + itemRespawnTime)
        {
            GetComponent<VisualPopupImage>().isShowing = true;
        }
        else
        {
            GetComponent<VisualPopupImage>().isShowing = false;
        }

        if (Input.GetKey(KeyCode.Space) && isInteracting && Time.time >= itemRespawnStart + itemRespawnTime)
        {
            interactTimeCurrent += Time.deltaTime;

            if (interactTimeCurrent >= timeToInteract)
            {
                Item item = new Item();
                item.BuildItemFromScriptableObject(myItem);

                player.ReceiveItem(item);
                isInteracting = false;
                interactTimeCurrent = 0;

                itemRespawnStart = Time.time;

                string action = "Received " + item.itemName + " from " + interactableName;
                UIManager._instance.enableActionText(action);
            }

            UIManager._instance.UpdateInteractingUI(interactTimeCurrent, timeToInteract);
        }
        else if (!Input.GetKey(KeyCode.Space))
        {
            isInteracting = false;
            interactTimeCurrent = 0;
        }
    }

    public override void Interact()
    {
        if (!player.hasItem)
        {
            isInteracting = true;
        }
        else
        {
            isInteracting = false;
            interactTimeCurrent = 0;

            print("Player is already holding: " + player.item.itemName);
        }
    }
}