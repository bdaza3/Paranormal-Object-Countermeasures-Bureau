using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class NoteAppear : MonoBehaviour
{
    [SerializeField] private RawImage noteImage; 

    public AudioSource miscEventsAudioSource; //reference to the misc events audio source

    [SerializeField] public AudioClip pageflip;

    private bool isInRange = false; 
    private bool isNoteVisible = false; 

    public bool redRoomNote = false; 

    public bool facultyNote = false; 

    private PlayerMovement playerMovement; 
    private Rigidbody playerRigidbody;

    public GameObject note; //reference to the current note GameObject

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
            FindAnyObjectByType<ThoughtDialogueManager>().ShowHoverText("(E) Read note");
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<ThoughtDialogueManager>().ShowHoverText("");
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
        miscEventsAudioSource.PlayOneShot(pageflip); //play the page flip sound
        isNoteVisible = !isNoteVisible;
        noteImage.enabled = isNoteVisible;
        if (isNoteVisible)
            FindAnyObjectByType<ThoughtDialogueManager>().ShowHoverText("(E) Close note");
        if (isNoteVisible && redRoomNote){
            ThoughtDialogueManager noteManager = FindFirstObjectByType<ThoughtDialogueManager>();
            noteManager.redRoomNote = true; //set the note to be visible
            noteManager.ShowNote("I didn't even notice. It all happened so fast. When I came to, after the loud impact, the whole classroom was screaming. The windows were stained red, and I saw both of them. I never thought Matsuzaka would really do it. But the other girl... I couldn't even recognise her face.");
            noteManager.redRoomNote = false; //set the note to be invisible again so that dialogue can show
        }
        if (isNoteVisible && facultyNote){
            ThoughtDialogueManager noteManager = FindFirstObjectByType<ThoughtDialogueManager>();
            noteManager.facultyNote = true; //set the note to be visible
            noteManager.ShowNote("At approximately 1:47pm on October 2nd, two students, [REDACTED] and [REDACTED], were found deceased in front of classroom 1-C on the west side of the school. Emergency services were immediately notified and the scene was sealed off per established procedures. Upon arrival, paramedics found both students unresponsive and determined the cause of death to be [REDACTED]. The school has launched an internal investigation into this matter, including interviewing faculty and staff and providing counselling. Given the serious nature of this incident and the declining student population, it has been decided that the school will be closed and suspended as of November 20th. We ask all faculty and staff to pay close attention to the mental health of students and to report any abnormal behavior to the administration immediately.");
            noteManager.facultyNote = false;
        }
        
        
        NoteText.SetActive(isNoteVisible); 
        if (!isNoteVisible){
            FindAnyObjectByType<ThoughtDialogueManager>().ShowHoverText("");
        }

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

        if (!isNoteVisible && facultyNote){//if player is exiting faculty note, remove it and trigger bell
            note.SetActive(false);
        }
    }
}

