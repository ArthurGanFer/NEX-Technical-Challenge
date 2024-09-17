using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    public UnityEvent gateEvent;
    private int playerLayer;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            gateEvent.Invoke();
            Destroy(gameObject);
        }
    }

}
