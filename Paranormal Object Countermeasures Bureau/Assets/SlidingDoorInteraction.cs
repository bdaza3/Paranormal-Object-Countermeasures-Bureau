using UnityEngine;

public class SlidingDoorInteraction : MonoBehaviour
{
    public GameObject door;
    public float openPosition = 2f;  // Distance for the door to slide open
    public float closedPosition = 0f;  // Position where the door is closed (starting position)
    public float speed = 2f;  // Speed of the door sliding movement
    private bool playerInRange = false;  // Flag to check if the player is in range
    private bool doorIsOpen = false;  // Track if the door is open or closed

    private Vector3 doorOriginalPosition;  // Original position of the door

    void Start()
    {
        // Store the door's original position
        doorOriginalPosition = door.transform.position;
    }

    void Update()
    {
        // Check for input only when the player is in range
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();  // Toggle the door between open and closed
        }

        // Move the door based on whether it's open or closed
        if (doorIsOpen)
        {
            MoveDoor(openPosition);  // Move the door to the open position
        }
        else
        {
            MoveDoor(closedPosition);  // Move the door to the closed position
        }
    }

    // Function to move the door (sliding movement)
    void MoveDoor(float targetPosition)
    {
        Vector3 targetPositionVec = doorOriginalPosition + new Vector3(targetPosition, 0f, 0f);  // Slide along the X-axis
        door.transform.position = Vector3.Lerp(door.transform.position, targetPositionVec, speed * Time.deltaTime);
    }

    // Function to toggle the door state (open/close)
    void ToggleDoor()
    {
        doorIsOpen = !doorIsOpen;  // Toggle the door state (open or closed)
    }

    // Detect when the player enters or exits the trigger zone
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;  // Player is in range to interact
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;  // Player is out of range, no interaction possible
        }
    }
}
