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
            FlashlightInventory inventory = player.GetComponent<FlashlightInventory>();

            if (inventory != null)
            {
                if (CompareTag("AxeItem")) 
                {
                    inventory.axeObtained = true; 
                    Debug.Log("Axe picked up!");
                }

                if (CompareTag("DrillItem")) 
                {
                    inventory.drillObtained = true; 
                    Debug.Log("Drill picked up!");
                }

                gameObject.SetActive(false); 
            }
        }
    }
}

