using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Default movement speed (walking speed)
    public float runSpeed = 9f;  // Running speed (faster than walking)
    public float lookSpeedX = 3f; // Mouse sensitivity for horizontal look
    public float lookSpeedY = 3f; // Mouse sensitivity for vertical look

    private Camera playerCamera;
    private float rotationX = 0f; // For vertical rotation (looking up/down)

    private void Start()
    {
        playerCamera = Camera.main; // Get the main camera attached to the scene
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor during gameplay
    }

    private void Update()
    {
        MoveCharacter(); // Handle movement
        LookAround(); // Handle mouse look
    }

    private void MoveCharacter()
    {
        // Get input for horizontal and vertical movement
        float horizontal = Input.GetAxis("Horizontal"); // A/D or left/right arrow keys
        float vertical = Input.GetAxis("Vertical"); // W/S or up/down arrow keys

        // Check if the Left Shift key is held down for running
        float currentMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

        // Calculate the direction to move in
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f) // Ensure that the player is moving
        {
            // Adjust the speed and move the player object
            float moveSpeedAdjusted = currentMoveSpeed * Time.deltaTime; // Adjust movement speed based on frame rate
            transform.Translate(moveDirection * moveSpeedAdjusted); // Move the player object
        }
    }

    private void LookAround()
    {
        // Get mouse movement for look
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX; // Horizontal mouse movement
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY; // Vertical mouse movement

        // Rotate the player horizontally (around the Y-axis)
        transform.Rotate(Vector3.up * mouseX);

        // Update the vertical rotation for the camera (looking up/down)
        rotationX -= mouseY; // We subtract mouseY to invert the control (feel free to adjust the behavior)
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Clamp the rotation so it doesn't flip
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}