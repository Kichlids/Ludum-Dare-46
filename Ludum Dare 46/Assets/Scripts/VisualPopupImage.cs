using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PopUpType { WhileInteracting, WhileNearby}

public class VisualPopupImage : MonoBehaviour
{
    public PopUpType myType;

    GameObject player;

    public GameObject imagePrefab;
    private float maxRotationDegrees = 30f;
    private float rotationSpeed = 5f;
    private float time = 0;

    public float popupDistance = 5;

    public bool isShowing = true;

    private Interactable interactable;

    private void Start() {
        interactable = GetComponent<Interactable>();
        player = GameObject.FindGameObjectWithTag("Player");
        isShowing = true;
    }

    private void Update()
    {
        if (player != null && isShowing) {
            if (myType == PopUpType.WhileInteracting) {
                if (interactable.isInteracting) {
                    ItemPreparer preparerComp;
                    if (preparerComp = GetComponent<ItemPreparer>()) {
                        if (preparerComp.item.preparedTime < preparerComp.item.timeUntilPrepared && preparerComp.item.myIcon_Under != null) {
                            imagePrefab.GetComponent<SpriteRenderer>().sprite = preparerComp.item.myIcon_Under;
                        }
                        else if (preparerComp.item.preparedTime > preparerComp.item.timeUntilOverPrepared && preparerComp.item.myIcon_Over != null) {
                            imagePrefab.GetComponent<SpriteRenderer>().sprite = preparerComp.item.myIcon_Over;
                        }
                        else {
                            imagePrefab.GetComponent<SpriteRenderer>().sprite = preparerComp.item.myIcon;
                        }
                    }

                    imagePrefab.gameObject.SetActive(true);
                    imagePrefab.transform.rotation = Quaternion.Euler(0, time * rotationSpeed * 6, maxRotationDegrees * Mathf.Sin(time * rotationSpeed));
                    time += Time.deltaTime;
                }
                else {
                    imagePrefab.gameObject.SetActive(false);
                    time = 0;
                }
            }
            else if (myType == PopUpType.WhileNearby) {
                if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= popupDistance) {
                    imagePrefab.gameObject.SetActive(true);
                    imagePrefab.transform.rotation = Quaternion.Euler(0, time * rotationSpeed * 6, maxRotationDegrees * Mathf.Sin(time * rotationSpeed));
                    time += Time.deltaTime;
                }
                else {
                    imagePrefab.gameObject.SetActive(false);
                    time = 0;
                }
            }
        }
        else
        {
            imagePrefab.gameObject.SetActive(isShowing);
        }
    }
}
