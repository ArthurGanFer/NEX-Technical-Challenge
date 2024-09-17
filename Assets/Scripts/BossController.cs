using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class BossController : MonoBehaviour
{
    public int health;

    public PlayerController player;
    public bool lockAtPlayer;
    private Transform bossHead;
    private Transform bossBody;

    public bool isRotating;
    public float rotationCurrentSpeed;
    public float rotationTopSpeed;
    public float rotationAcceleration;

    public Shooter shooter;
    public bool isShooting;
    public int shotCount;

    // Start is called before the first frame update
    void Start()
    {
        shooter = transform.Find("Head/Face/Shooter").GetComponent<Shooter>();
        bossHead = transform.Find("Head");
        bossBody = transform.Find("Body");
        isRotating = false;
        rotationCurrentSpeed = 0f;
        lockAtPlayer = false;
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockAtPlayer)
            TargetPlayer();

        if (!GameManager.Instance.gameActive)
            return;

        if (isRotating)
            RotateArms();

        if (isShooting)
            shooter.isActivated = true;
    }

    public void WakeUpBoss()
    {
        lockAtPlayer = true;
        isRotating = true;
        isShooting = true;
    }

    void TargetPlayer()
    {
        Vector3 dir = player.transform.position - bossHead.position;
        dir.y = Mathf.Clamp(dir.y, 0, 360); //limit the down rotation
        Quaternion rot = Quaternion.LookRotation(dir);
        bossHead.rotation = Quaternion.Lerp(bossHead.rotation, rot, 5f * Time.deltaTime);
    }

    void RotateArms()
    {
        if (rotationCurrentSpeed < rotationTopSpeed)
            rotationCurrentSpeed += rotationAcceleration * Time.deltaTime;

        bossBody.Rotate(0f, rotationCurrentSpeed * Time.deltaTime, 0f, Space.Self);
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
            Death();
    }

    void Death()
    {
        Destroy(gameObject);
    }

}
