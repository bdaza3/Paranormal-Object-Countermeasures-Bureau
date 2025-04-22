using UnityEngine;
using System.Collections;

public class OpenVent : MonoBehaviour
{
    public GameObject ventCover;                 
    public Transform[] boltPositions;            
    public float drillTimePerBolt = 1.0f; //the smalller the number, the faster the drill          

    public GameObject drillPrefab;               

    public Transform drillStartPoint;            
    private GameObject player;
    private bool isInRange = false;
    private bool isDrilling = false;
    private bool ventOpened = false;

    //SOUNDS
    [SerializeField] private AudioClip drillSound;
    private AudioSource audioSource;

    [SerializeField] private AudioClip ventDropSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
                FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("I need a drill to open this vent cover...");           
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
            audioSource.pitch = 0.9f;
            audioSource.Play();
            yield return new WaitForSeconds(0.6f); //play a small snippet of it
            audioSource.Stop();

            Transform bolt = boltPositions[i];
            if (bolt == null) continue;

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

        Destroy(tempDrill);
        playerHeldDrill.SetActive(true);

        OpenVentilation();
        isDrilling = false;
    }

    private void OpenVentilation() //detach the vent cover
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
            FindFirstObjectByType<ThoughtDialogueManager>().ShowThought(" ", 
                () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Find the key in classroom 1-C")
            );}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<ThoughtDialogueManager>().ShowHoverText("(E) Open vent cover");
            isInRange = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<ThoughtDialogueManager>().ShowHoverText("");
            isInRange = false;
            player = null;
        }
    }
}

