using UnityEngine;
using UnityEngine.UI; 

public class NoteAppear : MonoBehaviour
{
    [SerializeField] private RawImage noteImage; 
    private bool isInRange = false; 
    private bool isNoteVisible = false; 

    public bool redRoomNote = false; // Set this to true if the note is in the red room

    public bool facultyNote = false; // Set this to true if the note is in the faculty room

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
        if (isNoteVisible && redRoomNote)
            FindFirstObjectByType<ThoughtDialogueManager>().ShowNote("I didn't even notice. It all happened so fast. When I came to, after the loud impact, the whole classroom was screaming. The windows were stained red, and I saw both of them. I never thought Matsuzaka would really do it. But the other girl... I couldn't even recognise her face.");
        if (isNoteVisible && facultyNote)
            FindFirstObjectByType<ThoughtDialogueManager>().ShowNote("At approximately 1:47pm on October 2nd, two students, [REDACTED] and [REDACTED], were found deceased in front of classroom 1-C on the west side of the school. Emergency services were immediately notified and the scene was sealed off per established procedures. Upon arrival, paramedics found both students unresponsive and determined the cause of death to be [REDACTED]. The school has launched an internal investigation into this matter, including interviewing faculty and staff and providing counselling. Given the serious nature of this incident and the declining student population, it has been decided that the school will be closed and suspended as of November 20th. We ask all faculty and staff to pay close attention to the mental health of students and to report any abnormal behavior to the administration immediately.");
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

