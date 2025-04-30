using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public GameObject door;  
    public GameObject doorPivot;  
    public float openRot = 90f;  
    public float closeRot = 0f;  
    public float speed = 5f;  
    private bool playerInRange = false;  
    private bool doorIsOpen = true;  

    public bool isStuckLocked; 

    //get box collider of door
    public BoxCollider doorCollider;

    public bool keyNeeded; 

private bool isRotating = false;
private float targetRot;

void Update()
{
    if (playerInRange && Input.GetKeyDown(KeyCode.E))
    {
        if (isStuckLocked)
        {
            FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("This door doesn't seem to open at all...");
            return;
        }

        if (keyNeeded)
        {
            PlayerInventory playerInventory = FindFirstObjectByType<PlayerInventory>();
            if (playerInventory.keyObtained)
            {
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

    if (isRotating)
    {
        doorCollider.enabled = false;
        Quaternion current = doorPivot.transform.localRotation;
        Quaternion target = Quaternion.Euler(0, targetRot, 0);
        doorPivot.transform.localRotation = Quaternion.Slerp(current, target, Time.deltaTime * speed);

        if (Quaternion.Angle(current, target) < 0.1f)
        {
            doorPivot.transform.localRotation = target;
            isRotating = false;
            doorCollider.enabled = true; 
        }
    }
}

void ToggleDoor()
{
    doorIsOpen = !doorIsOpen;
    targetRot = doorIsOpen ? openRot : closeRot;
    isRotating = true;
}


    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<ThoughtDialogueManager>().ShowHoverText("(E) Open door");
            playerInRange = true;  
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<ThoughtDialogueManager>().ShowHoverText("");
            playerInRange = false;  
        }
    }
}
