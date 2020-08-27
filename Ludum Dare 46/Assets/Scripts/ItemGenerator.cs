using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : Interactable
{
    public ItemScriptableObject[] myAvailableItems;
    public ItemScriptableObject myCurrentItem;
    float interactTimeCurrent;

    public float itemRespawnTime = 3;
    float itemRespawnStartTime = 0;

    bool itemAvailable = false;

    void Update()
    {
        if (itemAvailable)
            GetComponent<VisualPopupImage>().isShowing = true;
        else
            GetComponent<VisualPopupImage>().isShowing = false;

        if (!itemAvailable && Time.time >= itemRespawnStartTime + itemRespawnTime)
        {
            ChooseNewAvailableItem();
        }
        else if (Input.GetKey(KeyCode.Space) && isInteracting && itemAvailable)
        {
            interactTimeCurrent += Time.deltaTime;

            if (interactTimeCurrent >= timeToInteract)
            {
                Item item = new Item();
                item.BuildItemFromScriptableObject(myCurrentItem);

                player.ReceiveItem(item);
                isInteracting = false;
                interactTimeCurrent = 0;

                itemRespawnStartTime = Time.time;
                itemAvailable = false;

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
        print(player.name);

        if (!player.hasItem && itemAvailable)
        {
            print("player interacting");
            isInteracting = true;
        }
        else
        {
            isInteracting = false;
            interactTimeCurrent = 0;

            print("Player is already holding: " + player.item.itemName);
        }
    }

    private void ChooseNewAvailableItem()
    {
        int index = Random.Range(0, myAvailableItems.Length);

        myCurrentItem = myAvailableItems[index];
        itemAvailable = true;
    }
}
