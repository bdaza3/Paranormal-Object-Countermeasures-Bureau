using UnityEngine;

public class NoteInteraction : MonoBehaviour
{
    public GameObject note; // The note object
    public Transform notePosition; // The position where the note will be moved to in front of the player
    public float interactionRange = 1f; // Range within which the player can interact with the note
    public KeyCode interactKey = KeyCode.E; // Key to press to interact with the note

    private bool isInteracting = false; // Track if the player is currently viewing the note
    private bool canMove = true; // Flag to check if the player can move

    private Camera playerCamera;
    private Rigidbody rb;

    private void Start()
    {
        playerCamera = Camera.main; // Get the player's camera
        rb = GetComponent<Rigidbody>(); // Get the player's Rigidbody for movement
    }

    private void Update()
    {
        // Check if the player is within interaction range
        if (Vector3.Distance(transform.position, note.transform.position) <= interactionRange)
        {
            // If the player presses the 'E' key and the note is not already in view, show the note
            if (Input.GetKeyDown(interactKey) && !isInteracting)
            {
                StartInteraction();
            }
        }

        // If the player is interacting with the note, wait for any key to continue
        if (isInteracting && Input.anyKeyDown)
        {
            ContinueInteraction();
        }
    }

    private void StartInteraction()
    {
        // Lock the player's movement (by disabling physics and player input)
        canMove = false;
        rb.isKinematic = true; // Prevent the player from moving

        // Move the note in front of the player (camera position + some offset)
        note.transform.position = notePosition.position;

        // Apply rotation to face the player
        note.transform.rotation = Quaternion.Euler(90f, playerCamera.transform.rotation.eulerAngles.y, 0f);

        // Optionally, you can disable the mouse cursor visibility here:
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set interacting state to true
        isInteracting = true;
    }

    private void ContinueInteraction()
    {
        // Allow player to move again
        canMove = true;
        rb.isKinematic = false; // Enable physics again

        // Optionally, make the cursor visible again and unlock it
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Hide the note (or reset its position back to the initial place)
        note.SetActive(false);

        // Allow the player to move again
        isInteracting = false;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            // Player movement logic can be here (e.g., WASD movement, jumping, etc.)
            // You can re-enable player movement code here.
        }
    }
}
