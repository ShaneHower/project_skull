using UnityEngine;
using System.Collections;

// This script moves the character controller forward
// and sideways based on the arrow keys.
// It also jumps when pressing space.
// Make sure to attach a character controller to the same game object.
// It is recommended that you make only one call to Move or SimpleMove per frame.

public class PlayerMovement: MonoBehaviour
{
    CharacterController characterController;
    Animator playerAnimator;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;

    private float horizontal;
    private float vertical;

    private float mouseX;
    private float mouseY;

    // rotation speeds
    public float speedH;
    public float speedV;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();

        speedH = 300f;
        speedV = 300f;
    }

    void Update()
    {
        moveCamera();

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        characterMovement(horizontal, vertical);
        triggerAnimator(horizontal, vertical);
    }

    void characterMovement(float horizontal, float vertical)
    {

        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes
            moveDirection = new Vector3(horizontal, 0.0f, vertical);
            moveDirection *= speed;

            moveDirection = Camera.main.transform.TransformDirection(moveDirection);
            moveDirection.y = 0.0f;

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    public void moveCamera()
    {
        mouseY += speedH * Input.GetAxis("Mouse X") * Time.deltaTime;
        mouseX -= speedV * Input.GetAxis("Mouse Y") * Time.deltaTime;

        transform.Rotate(mouseY, mouseX, 0f); // calculate new rotation
        Vector3 currentRotation = new Vector3(mouseX, mouseY, 0.0f);

        currentRotation.x = Mathf.Clamp(currentRotation.x, -20.0f, 20.0f); // clamp x rotation
        transform.eulerAngles = currentRotation;
    }

    public void triggerAnimator(float horizontal, float vertical)
    {
        playerAnimator.SetFloat("forward_back", vertical);
        playerAnimator.SetFloat("right_left", horizontal);
        playerAnimator.SetBool("OnGround", characterController.isGrounded);
    }
}