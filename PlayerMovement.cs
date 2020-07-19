﻿using UnityEngine;
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
    Camera playerCamera;
    CameraUtility cameraUtility;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;

    private float horizontal;
    private float vertical;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        playerCamera = GetComponent<Camera>();

        cameraUtility = new CameraUtility(playerCamera);
    }

    void Update()
    {
        //moveCamera();

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        characterMovement(horizontal, vertical);
        triggerAnimator(horizontal, vertical);

        cameraUtility.followObject(characterController);
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

    public void triggerAnimator(float horizontal, float vertical)
    {
        playerAnimator.SetFloat("forward_back", vertical);
        playerAnimator.SetFloat("right_left", horizontal);
        playerAnimator.SetBool("OnGround", characterController.isGrounded);
    }
}