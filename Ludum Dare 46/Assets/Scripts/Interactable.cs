using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected PlayerController player;

    public string interactableName;

    public float timeToInteract;
    public bool isInteracting = false;


    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public abstract void Interact();
}