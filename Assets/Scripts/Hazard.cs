using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private int playerLayer;
    private int enemyLayer;
    public bool friendly;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!friendly && collision.gameObject.layer == playerLayer)
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }

        if (friendly && collision.gameObject.layer == enemyLayer)
        {
            collision.gameObject.GetComponentInParent<BossController>().TakeDamage();
        }
    }
}
