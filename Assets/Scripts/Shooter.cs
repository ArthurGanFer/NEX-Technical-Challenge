using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public bool isActivated;
    public Projectile projectile;
    public float delay = 0.5f;
    public float timer;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.Instance.gameActive)
            return;

        timer += Time.deltaTime;

        if (isActivated)
        {
            if (timer > delay)
            {
                Instantiate(projectile, transform.position, transform.rotation);
                timer = 0;
            }
        }

        if (target != null)
        {
            transform.LookAt(target);
        }
    }
}
