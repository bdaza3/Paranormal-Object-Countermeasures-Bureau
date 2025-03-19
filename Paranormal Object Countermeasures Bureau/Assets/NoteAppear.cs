using UnityEngine;
using UnityEngine.UI; 

public class NoteAppear : MonoBehaviour
{
    [SerializeField] private RawImage noteImage; 
    private bool isInRange = false; 
    private bool isNoteVisible = false; 

    private PlayerMovement playerMovement; 
    private Rigidbody playerRigidbody;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            playerRigidbody = player.GetComponent<Rigidbody>();
        }
        
        noteImage.enabled = false; // Ensure the note starts hidden
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleNoteVisibility();
        }
    }

    private void ToggleNoteVisibility()
    {
        isNoteVisible = !isNoteVisible;
        noteImage.enabled = isNoteVisible;

        if (playerMovement != null)
        {
            playerMovement.enabled = !isNoteVisible; // Disable movement when note is visible
        }

        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = isNoteVisible; // Freeze physics when note is visible
        }
        
        Cursor.lockState = isNoteVisible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isNoteVisible;
    }
}

