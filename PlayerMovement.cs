using UnityEngine;

public class PlayerMovement: MonoBehaviour
{
    // define needed game objects and outside scripts
    CharacterController characterController;
    Animator playerAnimator;
    Transform playerCamera;

    // ground detector vars
    GameObject groundDetectorObject { get { return transform.Find("ground_detector").gameObject; } }
    GroundDetector groundDetector { get { return groundDetectorObject.GetComponent<GroundDetector>(); } }
    public bool isGrounded { get { return groundDetector.isGrounded; } }

    // public variables that can be tuned
    public float walkSpeed = 2.0f;
    public float runSpeed = 6.0f;
    public float jumpHeight = 1f;
    public float gravity = 20.0f;
    public float turnSmoothTime = 0.12f;
    public float speedSmoothTime = 0.035f;

    // ref variables 
    float speedSmoothVelocity;
    float turnSmoothVelocity;

    // trigger animation variables
    bool basicAttack;
    bool heavyAttack;
    bool isJumping;
    bool isRunning;
    public bool isAttacking;
    float currentSpeed;
    int heavyAttackCycle = 0;

    float heavyComboTimer = 0.0f;

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
        attack();
        jump();
        
        playerVelocity.y = playerHeight;
        characterController.Move(playerVelocity * Time.deltaTime);

        animate();
    }

    void playerInput()
    {

        // player inputs
        isJumping = Input.GetButtonDown("Jump");
        basicAttack = Input.GetButtonDown("Attack");
        isRunning = Input.GetButton("Run");
        heavyAttack = Input.GetButtonDown("Heavy Attack");

        //animator triggers
        isAttacking = playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack");

        // player movement inputs
        horizontal = isAttacking ? 0.0f : Input.GetAxisRaw("Horizontal");
        vertical = isAttacking ? 0.0f : Input.GetAxisRaw("Vertical");
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
        playerVelocity = transform.forward * targetSpeed;
    }

    public void jump()
    {
        if(isGrounded && isJumping)
        {
            playerHeight = Mathf.Sqrt(jumpHeight * 2.0f * gravity);
        }

        playerHeight -= gravity * Time.deltaTime;
    }

    private void comboTimer()
    {
        if(heavyAttack)
        {
            heavyComboTimer = 1.1f;
        }
        else
        {
            heavyComboTimer = (heavyComboTimer < 0) ? 0 : (heavyComboTimer - Time.deltaTime);
        }
    }

    private void attack()
    {
        comboTimer();

        if(heavyAttack && heavyComboTimer != 0)
        {
            heavyAttackCycle = heavyAttackCycle < 3 ? heavyAttackCycle += 1 : 0;
        }
        else if(heavyComboTimer == 0)
        {
            heavyAttackCycle = 0;
        }
    }

    public void animate()
    {
        // check if the character is colliding with anything.  The characterController magnitude will be zero if they are blocked from moving
        currentSpeed = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;
        float speedPercent = ((isRunning) ? currentSpeed/runSpeed: currentSpeed/walkSpeed * 0.5f) * inputMag;
        playerAnimator.SetFloat("speedPercent", speedPercent, speedSmoothTime, Time.deltaTime);
        playerAnimator.SetBool("basicAttack", basicAttack);
        playerAnimator.SetInteger("heavyAttack", heavyAttackCycle);
    }
}