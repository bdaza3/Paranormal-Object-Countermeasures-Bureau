using UnityEngine;

public class NoteInteraction : MonoBehaviour
{
    public GameObject note; 
    public Transform notePosition; 
    public float interactionRange = 1f; 
    public KeyCode interactKey = KeyCode.E; 

    private bool isInteracting = false; 
    private bool canMove = true; 

    private Camera playerCamera;
    private Rigidbody rb;
    private PlayerMovement playerMovement; // Reference to the movement script

    private void Start()
    {
        playerCamera = Camera.main; 
        rb = GetComponent<Rigidbody>(); 
        playerMovement = GetComponent<PlayerMovement>(); // Get the movement script
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, note.transform.position) <= interactionRange)
        {
            if (Input.GetKeyDown(interactKey) && !isInteracting)
            {
                StartInteraction();
            }
        }

        if (isInteracting && Input.GetKeyDown(interactKey))
        {
            ContinueInteraction();
        }
    }

    private void StartInteraction()
    {
        canMove = false;
        rb.isKinematic = true; 

        if (playerMovement != null)
            playerMovement.enabled = false; // Disable movement script

        note.transform.position = notePosition.position;
        note.transform.rotation = Quaternion.Euler(90f, playerCamera.transform.rotation.eulerAngles.y, 0f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isInteracting = true;
    }

    private void ContinueInteraction()
    {
        canMove = true;
        rb.isKinematic = false; 

        if (playerMovement != null)
            playerMovement.enabled = true; // Re-enable movement script

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        note.SetActive(false);

        isInteracting = false;
    }
}

