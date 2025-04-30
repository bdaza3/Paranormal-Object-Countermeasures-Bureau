using UnityEngine;
using TMPro;
using System.Collections;
using JetBrains.Annotations;

public class ObtainItem : MonoBehaviour
{
    private bool isInRange = false;

    private bool took = false;
    private GameObject player;

    public GameObject MonsterHallway; //spawnable monster in hallway after key
    public GameObject Monster1B; //spawnable monster after vent

    public bool pickedup = false; //if the item has been picked up

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindFirstObjectByType<ThoughtDialogueManager>().ShowHoverText("(E or Left Click) Pick up item");
            isInRange = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindFirstObjectByType<ThoughtDialogueManager>().ShowHoverText("");
            isInRange = false;
            player = null;
        }
    }

    private void Update()
    {   
        //left click to pick up item or press e
        if (isInRange && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)))
        {
            PickUpItem();
        }

        
        // Start a coroutine to clear the hover text after 3 seconds
        if (pickedup == true){
            FindFirstObjectByType<ThoughtDialogueManager>().ShowHoverText("");
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
                    FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("This could be useful to open the vent cover in classroom 1-E",
                        () => FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("From there I can access classroom 1-C and get the key to the maintenance closet...",
                        () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Drill open the vent cover in classroom 1-E on the east side of the school.")
                    )
            );
                }
                if (CompareTag("KeyItem")) 
                {
                    inventory.keyObtained = true; 
                    Debug.Log("Key picked up!");
                    FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("I should be able to use this to open the locked door...", 
                        () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Unlock the maintenance closet to the right of the bathrooms in the main hallway ")
                    );
                    Monster1B.SetActive(true); //spawn monster after vent
                }
                if (CompareTag("AxeItem")) 
                {
                    inventory.axeObtained = true; 
                    Debug.Log("Axe picked up!");
                    FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("This'll be useful to break down the boards blocking the stairwell...", 
                        () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Break down the wooden boards near the lockers to access the second floor.")
                    );     
                    MonsterHallway.SetActive(true); //spawn monster in hallway after key               
                }
                if (CompareTag("LighterItem")) 
                {
                    inventory.lighterObtained = true; 
                    Debug.Log("Lighter picked up!");
                    FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("This lighter could come in handy in dark areas...",
                        () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Use the lighter to start a fire and escape.")
                    );
                }
                if (CompareTag("FuelCan")) 
                {
                    inventory.fuelcanObtained = true; 
                    Debug.Log("Fuel can picked up!");
                    FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("I should be able to use this to start a fire...",
                        () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Use the fuel can to start a fire and escape.")
                    );
                }
                if (CompareTag("cloth"))
                {
                    inventory.clothObtained = true; 
                    Debug.Log("Cloth picked up!");
                    FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("I should be able to use this to start a fire...",
                        () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Use the cloth to start a fire and escape.")
                    );
                }

                pickedup = true;
                gameObject.SetActive(false); 
            }
        }
    }
}

