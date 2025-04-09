using UnityEngine;
using System.Collections;

public class OpenVent : MonoBehaviour
{
    public GameObject ventCover;                 
    public Transform[] boltPositions;            
    public float drillTimePerBolt = 0.5f;          

    public GameObject drillPrefab;               

    public Transform drillStartPoint;            
    private GameObject player;
    private bool isInRange = false;
    private bool isDrilling = false;
    private bool ventOpened = false;

    private void Update()
    {
        if (isInRange && !isDrilling && Input.GetKeyDown(KeyCode.E))
        {
            PlayerInventory inventory = player?.GetComponent<PlayerInventory>();
            if (inventory != null && inventory.drillObtained && inventory.isDrillEquipped)
            {
                Debug.Log("Starting drill process...");
                StartCoroutine(DrillBolts(inventory.drill));
            }
            else
            {
                Debug.Log("E pressed, but drill not obtained or equipped");
            }
        }
    }

    private IEnumerator DrillBolts(GameObject playerHeldDrill)
    {
        isDrilling = true;

        playerHeldDrill.SetActive(false);

        GameObject tempDrill = Instantiate(drillPrefab, drillStartPoint.position, drillStartPoint.rotation);
        
        for (int i = 0; i < boltPositions.Length; i++) //for each bolt
        {
            Transform bolt = boltPositions[i];
            if (bolt == null) continue;

            Debug.Log($"Drilling bolt {i + 1}...");

            Vector3 directionToBolt = -bolt.forward;
            float forwardOffset = 0.35f; //dist from front bolt
            Vector3 boltOffset = bolt.position + (bolt.up * 0.35f) - (directionToBolt.normalized * forwardOffset); 
            Quaternion endRot = Quaternion.LookRotation(directionToBolt);

            //animate drill to each bolt's position
            Vector3 startPos = tempDrill.transform.position;
            Quaternion startRot = tempDrill.transform.rotation;

            float t = 0f;
            while (t < drillTimePerBolt)
            {
                t += Time.deltaTime;
                float normalized = t / drillTimePerBolt;

                tempDrill.transform.position = Vector3.Lerp(startPos, boltOffset, normalized);
                tempDrill.transform.rotation = Quaternion.Slerp(startRot, endRot, normalized);
                yield return null;
            }
        }

        Debug.Log("All bolts removed!");

        Destroy(tempDrill);
        playerHeldDrill.SetActive(true);

        OpenVentilation();
        isDrilling = false;
    }

    private void OpenVentilation()
    {
        if (ventCover != null && !ventOpened)
        {
            Rigidbody rb = ventCover.GetComponent<Rigidbody>();
            if (rb == null)
                rb = ventCover.AddComponent<Rigidbody>();

            rb.isKinematic = false;
            rb.useGravity = true;

            ventOpened = true;

            Debug.Log("Vent cover detached!");
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

