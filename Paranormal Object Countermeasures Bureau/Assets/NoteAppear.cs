using UnityEngine;
using UnityEngine.UI; // Make sure to include this if you're working with UI components like RawImage

public class NoteAppear : MonoBehaviour
{
    [SerializeField]
    private RawImage noteImage; // The RawImage UI component for the note
    private bool isInRange = false; // Flag to check if the player is in range to interact
    private bool isNoteVisible = false; // Flag to track if the note is currently visible

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true; // Player is in range to interact with the note
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false; // Player has left the range
        }
    }

    void Update()
    {
        // Check for input only when the player is in range
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleNoteVisibility(); // Toggle the note's visibility when E is pressed
        }
    }

    // Function to toggle the visibility of the note
    private void ToggleNoteVisibility()
    {
        if (isNoteVisible)
        {
            // Hide the note
            noteImage.enabled = false;
        }
        else
        {
            // Show the note
            noteImage.enabled = true;
        }

        // Toggle the visibility state
        isNoteVisible = !isNoteVisible;
    }
}
