using UnityEngine;

public class PlayerMovement: MonoBehaviour
{
    // define needed game objects and outside scripts
    CharacterController characterController;
    Animator playerAnimator;
    Transform playerCamera;
    GameObject groundDetectorObject { get { return transform.Find("Ground Detector").gameObject; } }
    GroundDetector groundDetector { get { return groundDetectorObject.GetComponent<GroundDetector>(); } }

    // public variables that can be tuned
    public float walkSpeed = 2.0f;
    public float runSpeed = 6.0f;
    public float jumpHeight = 1f;
    public float gravity = 20.0f;
    public float turnSmoothTime = 0.12f;
    public float speedSmoothTime = 0.035f;
    public bool isGrounded { get { return groundDetector.isGrounded; } }

    // ref variables 
    float speedSmoothVelocity;
    float turnSmoothVelocity;

    // trigger animation variables
    bool basicAttack;
    bool isJumping;
    bool isRunning;
    float currentSpeed;

    // variables needed for walk/run
    private float inputMag;
    private float horizontal;
    private float vertical;
    float playerHeight;
    Vector3 playerVelocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        playerAnimator = GetComponent<Animator>();
        playerCamera = Camera.main.transform;
    }

    void Update()
    {
        playerInput();
        walkRun();
        jump();
        
        playerVelocity.y = playerHeight;
        characterController.Move(playerVelocity * Time.deltaTime);

        animate();
    }

    void playerInput()
    {
        // player movement inputs
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // other player inputs
        isJumping = Input.GetButtonDown("Jump");
        basicAttack = Input.GetButtonDown("Attack");
        isRunning = Input.GetButton("Run");
    }

    void walkRun()
    {
        Vector2 input = new Vector2(horizontal, vertical);
        Vector2 inputDir = input.normalized;
        inputMag = inputDir.magnitude;

        if (inputDir != Vector2.zero)
        {
            //using atan2 because it deals with the denominator being 0.  Arctan2 returns angle in radians. adding the camera's y rotation so that the player moves in the direction the camera is facing
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            // SmoothDampAngle takes current angle and desired angle .  also takes turn smooth time (the amount of seconds it will take to go from the current value to the new value), 
            // and turnSmoothVelocity. ref lets the function modify the value? 
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }
        float targetSpeed = ((isRunning) ? runSpeed : walkSpeed) * inputMag;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        playerVelocity = transform.forward * currentSpeed;
    }

    public void jump()
    {
        if(isGrounded && isJumping)
        {
            playerHeight = Mathf.Sqrt(jumpHeight * 2.0f * gravity);
        }

        playerHeight -= gravity * Time.deltaTime;
    }

    public void animate()
    {
        // check if the character is colliding with anything.  The characterController magnitude will be zero if they are blocked from moving
        currentSpeed = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;
        float speedPercent = ((isRunning) ? currentSpeed/runSpeed: currentSpeed/walkSpeed * 0.5f) * inputMag;
        playerAnimator.SetFloat("speedPercent", speedPercent, speedSmoothTime, Time.deltaTime);
        playerAnimator.SetBool("basicAttack", basicAttack);
    }
}