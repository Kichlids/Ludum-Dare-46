using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceReplenisher : Interactable
{
    public ResourceScriptableObject myResource;

    float waitPeriod = 1f;
    float lastUseTime = 0;

    private void Update()
    {
        if (isInteracting && Input.GetKey(KeyCode.Space))
        {
            GameManager._instance.Consume(ItemTarget.Adult, myResource);
            player.playerModel.transform.position = transform.position + new Vector3(0, 1, 0);
            player.playerModel.transform.rotation = transform.rotation;
            player.myAnim.SetBool("isSitting", true);
        }
    }

    void LateUpdate()
    {
        if (isInteracting && !Input.GetKey(KeyCode.Space))
        {
            isInteracting = false;
            lastUseTime = Time.time;
            player.playerModel.transform.localPosition = new Vector3(0, 0, 0);
            player.playerModel.transform.localRotation = new Quaternion(0, 0, 0, 0);
            player.myAnim.SetBool("isSitting", false);
        }
    }

    public override void Interact() {
        if (Time.time >= lastUseTime + waitPeriod)
        {
            isInteracting = true;
        }
    }
}
