using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    //movement
    public float walkSpeed = 8f;
    public float runSpeed = 25f;
    public float crouchSpeed = 2.5f;
    private float currentSpeed;

    //mouse look
    public float lookSpeedX = 3f;
    public float lookSpeedY = 3f;

    //crouch
    public float standHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchCameraHeight = 1f;
    public float standCameraHeight = 1.8f;
    public float crouchTransitionSpeed = 6f;

    //jump
    public float jumpForce = 1.5f; // Force applied when jumping
    private bool canJump = true; // Flag to control jump cooldown

    public float gravity = -9.81f;
    private Vector3 velocity;

    //SOUNDS
    public AudioSource WalkAudioSource;
    public AudioSource VentAudioSource;
    public AudioSource BreathingAudioSource;

    [SerializeField] private AudioClip footstepSound;
    [SerializeField] private AudioClip runningSound;
    [SerializeField] private AudioClip ventSound;

    private CharacterController characterController;
    private Camera playerCamera;

    private float rotationX = 0f;
    private bool isCrouching = false;

    private float currTime;
    private float startTime = 0;
    // private float cooldownTime = 0;
    // private float cooldownStart = 0;
    public float staminaInterval = 3f;
    public float cooldownInterval = 2f;
    private bool tired = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentSpeed = walkSpeed;

        WalkAudioSource = GetComponent<AudioSource>();
        //defaultCamPos = playerCamera.transform.localPosition;

    }

    private void Update()
    {
        HandleLook();
        HandleCrouch();
        HandleMovement();
        ApplyGravity();
        HandleJump(); 
        LookAround();
        //HandleHeadBob();
    }

    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;

        transform.Rotate(Vector3.up * mouseX);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    private void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;


        float currentMoveSpeed = walkSpeed; //default speed
        if (Input.GetKey(KeyCode.LeftShift)) //if shift is pressed
        {
            currentMoveSpeed = runSpeed; //run speed
            SoundManager.MakeSound(transform.position, 15f); //running sound trigger
        }
        else
        {
            SoundManager.MakeSound(transform.position, 8f); //walking sound trigger
        }
        Vector3 moveDirection = new Vector3(mouseX, 0f, mouseY).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float moveSpeedAdjusted = currentMoveSpeed * Time.deltaTime;
            transform.Translate(moveDirection * moveSpeedAdjusted); 
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isMoving = horizontal != 0 || vertical != 0;

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && !tired) //running
        {

                currTime += Time.deltaTime;
                currentSpeed = runSpeed;
                if (WalkAudioSource.resource != runningSound) { WalkAudioSource.resource = runningSound; }
                if (isMoving) SoundManager.MakeSound(transform.position, 15f);

            if(currTime - startTime >= staminaInterval){
                tired = true;
                currentSpeed = walkSpeed - 1;
                currTime = 0;
                startTime = 0;
                if(BreathingAudioSource.isPlaying){
                    BreathingAudioSource.UnPause();
                }else{
                    BreathingAudioSource.Play();
                }
                Debug.Log("cooldown started!");
            }
        }
        else if (isCrouching) //crouching
        {
            currentSpeed = crouchSpeed;
            if (isMoving) SoundManager.MakeSound(transform.position, 2f);
            PlayerInventory playerInventory = GetComponent<PlayerInventory>();
            if (playerInventory.inVent){
                WalkAudioSource.volume = 0.7f; //lower volume of footsteps in vent
                if (WalkAudioSource.resource != ventSound) { WalkAudioSource.resource = ventSound;}
            }
            if (!playerInventory.inVent)
            {
                WalkAudioSource.resource = null;
            }

        }
        else //walking
        {
            currentSpeed = walkSpeed;
            if (WalkAudioSource.resource != footstepSound) { WalkAudioSource.resource = footstepSound; }
            if (isMoving) SoundManager.MakeSound(transform.position, 8f);
            if(tired){
                currTime += Time.deltaTime;
                if (currTime - startTime >= cooldownInterval)
                {
                    BreathingAudioSource.Pause();
                    Debug.Log("cooldown ended!");
                    tired = false;
                    currTime = 0;
                    startTime = 0;
                }
            }
        }

        if(!isMoving){WalkAudioSource.resource = null;}

        if(!WalkAudioSource.isPlaying && isMoving){WalkAudioSource.Play();}

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        move.Normalize(); //normalize the movement vector to prevent faster diagonal movement
        characterController.Move(move * currentSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded && canJump)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); // Apply jump force
            canJump = false; // Disable jumping
            Invoke(nameof(ResetJump), 2f); // Re-enable jumping after 2 seconds
        }
    }

    private void ResetJump()
    {
        canJump = true; // Re-enable jumping
    }

    private void ApplyGravity() //apply gravity
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isCrouching)
            {
                if (CanStandUp())
                {
                    isCrouching = false; //standing up
                    characterController.height = standHeight; //change height of controller
                }
                else
                {
                    Debug.Log("Not enough space above");
                }
            }
            else
            {
                isCrouching = true; //crouch
                characterController.height = crouchHeight; //change height of controller
            }
        }

        float targetCamY = isCrouching ? crouchCameraHeight : standCameraHeight;
        Vector3 camPos = playerCamera.transform.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetCamY, Time.deltaTime * crouchTransitionSpeed);
        playerCamera.transform.localPosition = camPos;
    }

    private bool CanStandUp()
    {
        Vector3 start = transform.position + Vector3.up * (characterController.height / 2f);
        float checkDistance = standHeight - crouchHeight;

        return !Physics.Raycast(start, Vector3.up, checkDistance);
    }
}
