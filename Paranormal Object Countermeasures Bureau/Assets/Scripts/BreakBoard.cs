using UnityEngine;

public class BreakBoard : MonoBehaviour
{
    private bool playerInTrigger = false;
    private Rigidbody rb;
    public GameObject barrier;
    private Collider myCollider;
    private Collider barrierCollider;

    private bool isInRange = false;
    private bool isSwinging = false;
    


    public GameObject board;

    public GameObject player;

    private void Update(){

        // Check if the player is in range and the axe is equipped and left clicks
        if (isInRange && !isSwinging && Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (inventory != null && inventory.axeObtained && inventory.isAxeEquipped)
            {
                Debug.Log("Destroying board");
                isSwinging = true;
                Destroy(board);
            }
            else
            {
                FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("I need a drill to open this vent cover...");           
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            player = null;
        }
    }
}

