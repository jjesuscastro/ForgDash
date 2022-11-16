using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public KeyCode lowHit;
    public KeyCode highHit;
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(lowHit))
        {
            animator.SetTrigger("hit");
        }
        else if (Input.GetKeyDown(highHit))
        {
            animator.SetTrigger("highHit");
        }
    }

    private void FixedUpdate()
    {
        animator.ResetTrigger("hit");
    }
}
