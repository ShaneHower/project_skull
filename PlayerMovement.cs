using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement: MonoBehaviour
{
    CharacterController characterController;
    Animator playerAnimator;
    Transform playerCamera;

    public float walkSpeed = 2.0f;
    public float runSpeed = 6.0f;
    public float jumpSpeed = 7.0f;
    public float gravity = 20.0f;
    public float turnSmoothTime = 0.12f;
    public float speedSmoothTime = 0.05f;

    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;
    float turnSmoothVelocity;

    bool basicAttack;

    private float horizontal;
    private float vertical;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        playerCamera = Camera.main.transform;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Move(horizontal, vertical);
        attack();
    }

    void Move(float horizontal, float vertical)
    {

        Vector2 input = new Vector2(horizontal, vertical);
        Vector2 inputDir = input.normalized;
        float inputMag = inputDir.magnitude;

        Debug.Log(inputDir);
        if (inputDir != Vector2.zero)
        {
            //using atan2 because it deals with the denominator being 0.  Arctan2 returns angle in radians. adding the camera's y rotation so that the player moves in the direction the camera is facing
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            // SmoothDampAngle takes current angle and desired angle .  also takes turn smooth time (the amount of seconds it will take to go from the current value to the new value), 
            // and turnSmoothVelocity. ref lets the function modify the value? 
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        bool isRunning = Input.GetButton("Run");
        float targetSpeed = ((isRunning) ? runSpeed : walkSpeed) * inputMag;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        Vector3 velocity = transform.forward * currentSpeed;

        jump();
        velocity.y = velocityY;

        characterController.Move(velocity * Time.deltaTime);
        animate(inputMag, isRunning);
    }

    public void jump()
    {
        if (characterController.isGrounded)
        {
            if(Input.GetButtonDown("Jump"))
            {
                velocityY = jumpSpeed;
            }
        }

        velocityY -= gravity * Time.deltaTime;
    }

    public void attack()
    {
        basicAttack = Input.GetButtonDown("Attack");
    }

    public void animate(float inputMag, bool isRunning)
    {
        // check if the character is colliding with anything.  The characterController magnitude will be zero if they are blocked from moving
        currentSpeed = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;
        float speedPercent = ((isRunning) ? currentSpeed/runSpeed: currentSpeed/walkSpeed * 0.5f) * inputMag;
        playerAnimator.SetFloat("speedPercent", speedPercent, speedSmoothTime, Time.deltaTime);
        playerAnimator.SetBool("basicAttack", basicAttack);
    }
}