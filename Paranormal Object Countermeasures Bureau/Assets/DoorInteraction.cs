using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public GameObject door;  
    public GameObject doorPivot;  
    public float openRot = 90f;  
    public float closeRot = 0f;  
    public float speed = 5f;  
    private bool playerInRange = false;  
    private bool doorIsOpen = false;  

    void Update()
    {
        
        if (playerInRange && Input.GetKeyDown(KeyCode.E)) //button press is E
        {
            ToggleDoor();  
        }

        
        if (doorIsOpen)
        {
            RotateDoor(openRot);  
        }
        else
        {
            RotateDoor(closeRot);  
        }
    }

    
    void RotateDoor(float targetRot)
    {
        Vector3 currentRot = doorPivot.transform.localEulerAngles;  
        float targetYRotation = targetRot;

        
        Vector3 newRot = new Vector3(currentRot.x, targetYRotation, currentRot.z);
        doorPivot.transform.localEulerAngles = Vector3.Lerp(currentRot, newRot, speed * Time.deltaTime);
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
