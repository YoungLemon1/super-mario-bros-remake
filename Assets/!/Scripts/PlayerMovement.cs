using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Camera mainCam;
    private const float LEFT_BORDER = 14.3f;
    private Rigidbody2D rb;
    private Coroutine marioRunningCoroutine;
    private float axis;
    private bool wasInAir;
    private Collider2D bottomCollider;
    private bool isGrounded;
    private bool isMoving;
    private bool isRestarting;
    private LayerMask groundLayers;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius;
    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float acceleration = 5f;
    [SerializeField] float maxSpeed = 10f;

    private void Start()
    {
        enabled = true;
        isRestarting = false;
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        bottomCollider = GameObject.FindGameObjectWithTag("BottomCollider").GetComponent<Collider2D>();
        isGrounded = true;
        wasInAir = false;
        groundLayers = Physics2D.AllLayers;
        groundLayers &= ~(1 << LayerMask.NameToLayer("Enemies"));
    }

    private void Update()
    {
        SetGameState();
        SetRigidBodyStates();
        HandleMarioJump();
        HandleMarioMovement();
        HandleCameraScrolling();
    }

    private void StopRunningCoroutine()
    {
        StopCoroutine(marioRunningCoroutine);
        marioRunningCoroutine = null;
    }

    #region
    private void SetGameState()
    {
        if (!PlayerState.Instance.IsAlive && !isRestarting)
        {
            CustomAnimator.Instance.AnimateMarioDead();
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;
            isRestarting = true;
            StopRunningCoroutine();

            Invoke(nameof(RestartScene), 2f); // Restart the scene after a delay
        }
        else
            rb.isKinematic = false;
    }

    private void SetRigidBodyStates()
    {
        isGrounded = bottomCollider.IsTouchingLayers(groundLayers);
        isMoving = (axis == 0 && marioRunningCoroutine != null);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /*
    private IEnumerator PerformJumpWithVariableForce()
    {
        const float MAX_TIME = 0.1f;
        float startTime = Time.time;
        bool forceApplied = false;

        // Wait for either the jump button release or 1 second, whichever comes first
        while (!forceApplied && Time.time - startTime < MAX_TIME)
        {
            if (!Input.GetButton("Jump") || Time.time - startTime >= MAX_TIME)
            {
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                forceApplied = true;
            }
            yield return null; // Wait until the next frame to check again
        }

        // If the loop exits because the button was held for the full duration without release
        if (!forceApplied)
        {
            var finalJumpForce = jumpForce; // Apply maximum allowed force
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, finalJumpForce), ForceMode2D.Impulse);
        }

        wasInAir = true;
    }
    */
    #endregion
    #region
    private void HandleCameraScrolling()
    {

        if (!PlayerState.Instance.IsAlive)
        {
            return;
        }

        if (rb.position.x <= mainCam.transform.position.x - LEFT_BORDER)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            rb.position = new Vector2(mainCam.transform.position.x - LEFT_BORDER, rb.position.y);
        }
    }
    private void HandleMarioMovement()
    {
        axis = Input.GetAxis("Horizontal");
        float targetVelocityX = axis * moveSpeed;

        // Interpolate between the current velocity and the target velocity to create acceleration
        float xVelocity = Mathf.Lerp(rb.velocity.x, targetVelocityX, Time.fixedDeltaTime * acceleration);

        // Get the current position

        // Ensure the player's x-position is never less than the left border of the camera
        float clampedX = Mathf.Clamp(xVelocity, -maxSpeed, maxSpeed);

        // Apply the interpolated velocity to the Rigidbody2D only if it doesn't move the player past the left border
        rb.velocity = new Vector2(clampedX, rb.velocity.y);

        // Flip the character's sprite depending on the direction, if there is movement
        if (axis != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(axis), transform.localScale.y, transform.localScale.z);
            if (isGrounded)
                marioRunningCoroutine ??= StartCoroutine(CustomAnimator.Instance.AnimateMarioRunning());
            else
            {
                if (marioRunningCoroutine != null)
                {
                    StopRunningCoroutine();
                }
            }

        }

        else if (isMoving)
        {
            StopRunningCoroutine();
            if (isGrounded)
                CustomAnimator.Instance.AnimateMarioIdle();
        }
    }

    private void HandleMarioJump()
    {
        //state 1: mario starts jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("Jump");
            if (isMoving)
            {
                StopRunningCoroutine();
            }
            CustomAnimator.Instance.AnimateMarioJump();
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        //state 2: mario is already started jumping and is in midair
        else if (!isGrounded)
        {
            wasInAir = true;
        }
        //state 3: mario has just landed from his jump
        else if (isGrounded && wasInAir && Mathf.Abs(rb.velocity.y) <= 0.01f)
        {
            wasInAir = false;
            CustomAnimator.Instance.AnimateMarioIdle();
        }
    }
    #endregion
}
