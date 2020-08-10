using UnityEngine;
using System.Collections;

public class PlayerMovement: MonoBehaviour
{
    CharacterController characterController;
    Animator playerAnimator;
    Transform playerCamera;

    public float walkSpeed = 10.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

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

        characterMovement(horizontal, vertical);
        triggerAnimator(horizontal, vertical);
    }

    void characterMovement(float horizontal, float vertical)
    {

        Vector2 input = new Vector2(horizontal, vertical);
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            //using atan2 because it deals with the denominator being 0.  Arctan2 returns angle in radians. adding the camera's y rotation so that the player moves in the direction the camera is facing
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            // SmoothDampAngle takes current angle and desired angle .  also takes turn smooth time (the amount of seconds it will take to go from the current value to the new value), 
            // and turnSmoothVelocity. ref lets the function modify the value? 
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        float speed = walkSpeed * inputDir.magnitude;
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

    }

    public void triggerAnimator(float horizontal, float vertical)
    {
        if(horizontal != 0 || vertical != 0)
        {
            playerAnimator.SetInteger("walk", 1);
        }
        else
        {
            playerAnimator.SetInteger("walk", 0);
        }

        playerAnimator.SetBool("OnGround", characterController.isGrounded);
    }
}