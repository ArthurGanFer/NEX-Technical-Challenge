using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public bool isOpen;
    public float speed;
    private float topLimit;
    private float botLimit;
    private BoxCollider objCollider;

    // Start is called before the first frame update
    void Start()
    {
        objCollider = GetComponent<BoxCollider>();
        isOpen = true;
        botLimit = transform.position.y;
        topLimit = transform.position.y + 4;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isOpen && transform.position.y <= topLimit)
        {
            objCollider.enabled = true;
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        } else if (isOpen && transform.position.y > botLimit)
        {
            objCollider.enabled = false;
            transform.Translate(Vector3.up * -speed * Time.deltaTime);
        }
    }

    public void OpenGate()
    {
        isOpen = true;
    }

    public void CloseGate()
    {
        isOpen = false;
    }

}
