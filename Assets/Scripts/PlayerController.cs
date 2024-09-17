using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int health;
    private GameObject characterRenderer;

    //input fields
    private PlayerActionAsset playerActionAsset;
    private InputAction move;

    //movement fields
    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;
    private Animator animator;

    [SerializeField]
    private Shooter shooter;

    private bool isGrounded;

    private bool isImmune;

    private void Awake()
    {
        health = 5;
        characterRenderer = transform.Find("Renderer").gameObject;
        rb = GetComponent<Rigidbody>();
        playerActionAsset = new PlayerActionAsset();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerActionAsset.Player.Jump.started += DoJump;
        playerActionAsset.Player.Attack.performed += DoAttack;
        playerActionAsset.Player.Attack.canceled += StopAttack;
        move = playerActionAsset.Player.Move;
        playerActionAsset.Player.Enable();
    }

    private void OnDisable()
    {
        playerActionAsset.Player.Jump.started -= DoJump;
        playerActionAsset.Player.Attack.performed -= DoAttack;
        playerActionAsset.Player.Attack.canceled += StopAttack;
        playerActionAsset.Player.Disable();
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.gameActive)
            return;

        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        if (forceDirection.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;

        isGrounded = IsGrounded();
        LookAt();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void LookAt()
    {
        if (!move.enabled)
            return;

        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
        {
            animator.SetBool("isGrounded", true);
            return true;
        }
        else
        {
            animator.SetBool("isGrounded", false);
            return false;
        }
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
        }
    }

    private void DoAttack(InputAction.CallbackContext obj)
    {
        animator.SetBool("isShooting", true);
        shooter.isActivated = true;
    }
    private void StopAttack(InputAction.CallbackContext obj)
    {
        animator.SetBool("isShooting", false);
        shooter.isActivated = false;
    }

    public void TakeDamage()
    {
        if (isImmune)
        {
            return;
        }
        health--;
        if (health <= 0)
        {
            Destroy(characterRenderer);
            move.Disable();
            GameManager.Instance.GameOver();
            return;
        }
        forceDirection += Vector3.up * jumpForce;
        forceDirection += Vector3.forward * -5 * jumpForce;
        StartCoroutine(DamageRoutine());
    }

    private IEnumerator DamageRoutine()
    {
        int repeats = 0;
        move.Disable();
        yield return new WaitForSeconds(1f);
        move.Enable();
        isImmune = true;
        while (repeats <= 5) {
            characterRenderer.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            characterRenderer.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            repeats++;
        }
        isImmune = false;
    }

}
