using UnityEngine;

public class LockerInteraction : MonoBehaviour
{
    public GameObject otherLocker;
    public Transform lockerInsidePosition;
    private BoxCollider lockerCollider;

    private bool playerInRange = false;
    private bool isInLocker = false;
    private GameObject player;
    private Rigidbody playerRigidbody;
    private PlayerMovement playerMovement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerMovement = player.GetComponent<PlayerMovement>();
        lockerCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isInLocker)
            {
                ExitLocker();
            }
            else
            {
                EnterLocker();
            }
        }
    }

    void EnterLocker()
    {
        isInLocker = true;
        playerInRange = false; // Reset detection
        gameObject.SetActive(false); // Hide this locker
        otherLocker.SetActive(true); // Show the "open" version

        // Move player inside the locker
        player.transform.position = lockerInsidePosition.position;
        player.transform.rotation = lockerInsidePosition.rotation;

        // Disable movement
        playerRigidbody.isKinematic = true;
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (lockerCollider != null)
            lockerCollider.enabled = false;

        Debug.Log("Entered Locker");
    }

    void ExitLocker()
    {
        if (!isInLocker) return;

        isInLocker = false;
        playerInRange = false; // Prevent immediate re-entry
        gameObject.SetActive(true); // Show the locker again
        otherLocker.SetActive(false); // Hide the "open" version

        // Move player slightly outside of locker
        player.transform.position = transform.position + transform.right * 1.5f; // Adjust as needed
        player.transform.rotation = Quaternion.identity;

        // Restore movement
        playerRigidbody.isKinematic = false;
        if (playerMovement != null)
            playerMovement.enabled = true;

        if (lockerCollider != null)
            lockerCollider.enabled = true;

        Debug.Log("Exited Locker");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player Entered Locker Range");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player Exited Locker Range");
        }
    }
}
