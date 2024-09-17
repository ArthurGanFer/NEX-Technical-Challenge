using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
//    private float maxSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //animator.SetFloat("speed", rb.velocity.magnitude / maxSpeed);
        if (rb.velocity.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
        } else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
