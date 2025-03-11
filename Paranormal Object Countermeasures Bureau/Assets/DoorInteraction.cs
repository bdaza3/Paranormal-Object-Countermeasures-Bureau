using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public GameObject door;
    public float openRot = 90f;  // Rotation angle for the door to open
    public float closeRot = 0f;  // Rotation angle for the door to close
    public float speed = 5f;  // Speed of the door rotation
    private bool playerInRange = false;  // Flag to check if the player is in range
    private bool doorIsOpen = false;  // Track if the door is open or closed

    void Update()
    {
        // Check for input only when the player is in range
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();  // Toggle the door between open and closed
        }

        // Only rotate the door if it needs to open or close
        if (doorIsOpen)
        {
            RotateDoor(openRot);  // Rotate the door to open position
        }
        else
        {
            RotateDoor(closeRot);  // Rotate the door to closed position
        }
    }

    // Function to rotate the door
    void RotateDoor(float targetRot)
    {
        Vector3 currentRot = door.transform.localEulerAngles;
        float targetYRotation = targetRot;

        // Smoothly rotate the door towards the target rotation
        Vector3 newRot = new Vector3(currentRot.x, targetYRotation, currentRot.z);
        door.transform.localEulerAngles = Vector3.Lerp(currentRot, newRot, speed * Time.deltaTime);
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
