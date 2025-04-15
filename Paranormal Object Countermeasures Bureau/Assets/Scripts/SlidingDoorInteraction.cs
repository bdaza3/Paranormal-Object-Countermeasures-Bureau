using UnityEngine;

public class SlidingDoorInteraction : MonoBehaviour
{
    public GameObject door;
    public float openPosition = 2.5f;  
    public float closedPosition = 0f;  
    public float speed = 2.5f;  
    private bool playerInRange = false;  
    private bool doorIsOpen = false;  

    public GameObject player;
    public bool isStuckLocked; 

    public bool keyNeeded; 

    private Vector3 doorOriginalPosition;


    void Start()
    {
        doorOriginalPosition = door.transform.position;
    }

void Update()
{
    PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();

    if (playerInRange && Input.GetKeyDown(KeyCode.E))
    {
        if (isStuckLocked)
        {
            FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("This door doesn't seem to open at all...");           
            return;
        }

        if (keyNeeded)
        {
            if (playerInventory.keyObtained)
            {
                FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("That did it...");
                ToggleDoor();
            }
            else
            {
                FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("I need a key to open this door...");           
                return;
            }
        }
        else
        {
        ToggleDoor();
        }
    }

    if (doorIsOpen)
    {
        MoveDoor(openPosition);
    }
    else
    {
        MoveDoor(closedPosition);
    }
}

    void MoveDoor(float targetPosition)
    {
        Vector3 targetPositionVec = doorOriginalPosition + door.transform.right * targetPosition;
        door.transform.position = Vector3.Lerp(door.transform.position, targetPositionVec, speed * Time.deltaTime);
    }

    void ToggleDoor()
    {
        doorIsOpen = !doorIsOpen;  
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;  
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;  
        }
    }
}
