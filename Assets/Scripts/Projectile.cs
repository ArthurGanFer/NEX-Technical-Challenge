using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask ignoreLayer;
    //[SerializeField] private ParticleSystem destroyParticle;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreLayer == (ignoreLayer | (1 << collision.gameObject.layer)))
        {
            return;
        }
        Destroy(gameObject);
    }

}
