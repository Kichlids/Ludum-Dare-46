using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    private const float GRAVITY = -9.81f;

    private CharacterController cc;

    public GameObject playerModel;
    public Animator myAnim;
    private AudioSource audio;

    private Quaternion cameraRotationDefault;

    [Header("Movement variables")]
    public float speed;
    public float rotationSmoothness;
    public bool isMoving = false;
    public Vector3 moveDirection;
    public bool isDisoriented = false;
    public float cameraRotatingSpeed;

    [Header("Interaction variables")]
    public bool isInteracting = false;
    public Item item = null;
    public bool hasItem = false;

    private void Start() {
        cc = GetComponent<CharacterController>();
        audio = GetComponent<AudioSource>();
        cameraRotationDefault = Camera.main.transform.rotation;

        UIManager._instance.inventoryItem.gameObject.SetActive(false);
        item = null;
    }

    private void Update() {

        if (GameManager._instance.playerHungerCurrent <= 0 || GameManager._instance.playerThirstCurrent <= 0) {
            Destroy(playerModel.gameObject);
            GameManager._instance.isGameLost = true;
            print("Player has died");
            enabled = false;
        }

        if (!isInteracting) {

            Vector3 dir = PlayerInputDirection() * speed;
            dir.y = cc.isGrounded ? 0 : GRAVITY;
            cc.Move(dir * Time.deltaTime);
            playerModel.transform.rotation = PlayerInputRotation();
            InteractWithItem();
        }
        AdjustCameraRotation();
        AdjustMusicPitch();

        myAnim.SetBool("isInteracting", isInteracting);
        myAnim.SetBool("isMoving", isMoving);

        // Drop item
        if (Input.GetKeyDown(KeyCode.Q)) {
            TrashItem();
        }

        if (item != null)
        {
            UIManager._instance.inventoryItem.gameObject.SetActive(true);

            if (item.preparedTime < item.timeUntilPrepared && item.myIcon_Under != null)
            {
                UIManager._instance.inventoryItem.sprite = item.myIcon_Under;
            }
            else if (item.preparedTime > item.timeUntilOverPrepared && item.myIcon_Over != null)
            {
                UIManager._instance.inventoryItem.sprite = item.myIcon_Over;
            }
            else
            {
                UIManager._instance.inventoryItem.sprite = item.myIcon;
            }
            UIManager._instance.inventoryItemText.text = item.itemName;
        }
        else
        {
            UIManager._instance.inventoryItem.gameObject.SetActive(false);
            UIManager._instance.inventoryItemText.text = null;
        }
    }

    private void LateUpdate()
    {
        if (!Input.GetKey(KeyCode.Space))
            isInteracting = false;
    }

    private Vector3 PlayerInputDirection() {

        int vertical = 0;
        int horizontal = 0;

        if (Input.GetKey(KeyCode.W)) {
            vertical = 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            vertical = -1;
        }

        if (Input.GetKey(KeyCode.D)) {
            horizontal = 1;
        }
        if (Input.GetKey(KeyCode.A)) {
            horizontal = -1;
        } 

        moveDirection = new Vector3(horizontal, 0, vertical);
        moveDirection = moveDirection.normalized;

        if (moveDirection.magnitude == 0) {
            isMoving = false;
        }
        else {
            isMoving = true;
        }
        return moveDirection;
    }

    private Quaternion PlayerInputRotation() {

        if (isMoving) {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            return Quaternion.Slerp(playerModel.transform.rotation, targetRotation, rotationSmoothness * Time.deltaTime);
        }
        else {
            return playerModel.transform.rotation;
        }
    }

    private void AdjustCameraRotation() {
        if (isDisoriented) {
            Camera.main.transform.rotation = Quaternion.Euler(cameraRotationDefault.eulerAngles.x, cameraRotationDefault.eulerAngles.y, 
                (GameManager._instance.playerSanityCurrent - GameInfo.playerSanityThreshold) * 0.5f * Mathf.Sin(Time.time * cameraRotatingSpeed));
        }
        else {
            Camera.main.transform.rotation = cameraRotationDefault;
        }
    }

    private void AdjustMusicPitch() {
        if (isDisoriented) {
            audio.pitch = (1 - 0.5f) / GameInfo.playerSanityThreshold * GameManager._instance.playerSanityCurrent + 0.5f;
        }
        else {
            audio.pitch = 1;
        }
    }

    // Check this first, then check to use held item
    private void InteractWithItem() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (GameManager._instance.interactables.Count > 0)
            {

                GameObject closestObject = null;
                float closestDistance = float.MaxValue;

                foreach (GameObject element in GameManager._instance.interactables) {
                    float distance = Vector3.Distance(transform.position, element.transform.position);
                    if (distance < GameInfo.playerInteractableRange && distance < closestDistance) {
                        closestObject = element;
                        closestDistance = distance;
                    }
                }

                if (closestObject != null) {

                    if (closestObject.tag == "Dispenser") {
                        ItemDispenser dispenser = closestObject.GetComponent<ItemDispenser>();
                        dispenser.Interact();
                    }
                    else if (closestObject.tag == "Preparer") {
                        ItemPreparer preparer = closestObject.GetComponent<ItemPreparer>();
                        preparer.Interact();
                    }
                    else if (closestObject.tag == "Resource Dispenser") {
                        ResourceReplenisher replenisher = closestObject.GetComponent<ResourceReplenisher>();
                        replenisher.Interact();
                    }
                    else if (closestObject.tag == "Item Generator")
                    {
                        ItemGenerator generator = closestObject.GetComponent<ItemGenerator>();
                        generator.Interact();
                    }
                    else if (closestObject.tag == "Baby") {
                        Baby baby = closestObject.GetComponent<Baby>();
                        baby.Interact();
                    }

                    isInteracting = true;
                }
                else {
                    print("none found, attempting to use item on self");
                    UseHeldItem();
                }
            }
            else
            {
                isInteracting = false;
            }
        }
    }

    // Check this when not close enough to any other interactable
    public void UseHeldItem()
    {
        if (hasItem && item != null)
        {
            if (item.myTarget == ItemTarget.Adult || item.myTarget == ItemTarget.Both)
            {
                Item item = GiveItem();
                GameManager._instance.Consume(ItemTarget.Adult, item);

                string action = "Player consumed " + item.itemName;
                UIManager._instance.enableActionText(action);
            }
        }
    }

    public void ReceiveItem(Item item) {
        if (item == null) {
            print("null");
            return;
        }
        if (!hasItem) {
            item.EvaluateReadyness();
            this.item = item;
            hasItem = true;
        }
    }

    public Item GiveItem() {
        Item itemToGive = item;

        item = null;
        hasItem = false;

        return itemToGive;
    }

    private void TrashItem() {

        string itemName = item.itemName;

        item = null;
        hasItem = false;

        string action = "Trashed " + itemName;
        UIManager._instance.enableActionText(action);
    }
}
