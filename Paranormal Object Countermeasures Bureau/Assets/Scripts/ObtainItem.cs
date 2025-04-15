using UnityEngine;

public class ObtainItem : MonoBehaviour
{
    private bool isInRange = false;
    private GameObject player;

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

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUpItem();
        }
    }

    private void PickUpItem()
    {
        if (player != null)
        {
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                if (CompareTag("DrillItem")) 
                {
                    inventory.drillObtained = true; 
                    Debug.Log("Drill picked up!");
                    FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("This could be useful to open the vent covers...",
                        () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Drill open the vent covers to access classroom 1-C")
                    );
                }
                if (CompareTag("KeyItem")) 
                {
                    inventory.keyObtained = true; 
                    Debug.Log("Key picked up!");
                    FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("I should be able to use this to open the door...", 
                        () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Open the door to the maintenance closet")
                    );
                }
                if (CompareTag("AxeItem")) 
                {
                    inventory.axeObtained = true; 
                    Debug.Log("Axe picked up!");
                    FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("I could use this to break down the boards...", 
                        () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Break down the boards to access the stairwell")
                    );                    
                }

                gameObject.SetActive(false); 
            }
        }
    }
}

