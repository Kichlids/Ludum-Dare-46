using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBaby : MonoBehaviour
{
    public Animator myAnim;
    bool isCrying = false;

    // Start is called before the first frame update
    void Start()
    {
        isCrying = false;
        myAnim.SetBool("isCrying", false);
        InvokeRepeating("AnimCheck", 0, 2f);
    }

    // Update is called once per frame
    void AnimCheck()
    {
        if ((Random.Range(0, 100) < 50))
        {
            isCrying = !isCrying;
            myAnim.SetBool("isCrying", !isCrying);
        }
    }
}
