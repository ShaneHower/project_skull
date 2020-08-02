using UnityEngine;
using System.Collections;

public class PlayerMovement: MonoBehaviour
{
    CharacterController characterController;
    Animator playerAnimator;

    public float speed = 10.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 rotateDirection = Vector3.zero;

    private float horizontal;
    private float vertical;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        characterMovement(horizontal, vertical);
        triggerAnimator(horizontal, vertical);
    }

    void characterMovement(float horizontal, float vertical)
    {

        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes
            moveDirection = new Vector3(horizontal, 0.0f, vertical) * speed;

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;


        rotateCharacter(moveDirection);
        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
        
    }

    void rotateCharacter(Vector3 moveDirection)
    {
        // Character faces direction they are moving
        if (moveDirection.x != 0.0f || moveDirection.z != 0.0f)
        {
            rotateDirection = new Vector3(moveDirection.x, 0.0f, moveDirection.z);
            Debug.Log(rotateDirection);
            transform.rotation = Quaternion.LookRotation(rotateDirection);
        }
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