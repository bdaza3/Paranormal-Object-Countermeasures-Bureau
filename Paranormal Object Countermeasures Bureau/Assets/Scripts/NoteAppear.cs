using UnityEngine;
using UnityEngine.UI; 

public class NoteAppear : MonoBehaviour
{
    [SerializeField] private RawImage noteImage; 
    private bool isInRange = false; 
    private bool isNoteVisible = false; 

    private PlayerMovement playerMovement; 
    private Rigidbody playerRigidbody;

    public GameObject NoteText; //reference to the GameObject that contains the note text

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            playerRigidbody = player.GetComponent<Rigidbody>();
        }
        
        noteImage.enabled = false;
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
        if (isNoteVisible)
            FindFirstObjectByType<ThoughtDialogueManager>().ShowNote("I didn't even notice. It all happened so fast. When I came to, after the loud impact, the whole classroom was screaming. The windows were stained red, and I saw both of them. I never thought Matsuzaka would really do it. But the other girl... I couldn't even recognise her face.");

        NoteText.SetActive(isNoteVisible); 

        if (playerMovement != null)
        {
            playerMovement.enabled = !isNoteVisible;
        }

        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = isNoteVisible;
        }
        
        Cursor.lockState = isNoteVisible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isNoteVisible;
    }
}

