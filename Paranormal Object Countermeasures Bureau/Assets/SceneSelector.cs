using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    [SerializeField] private RawImage noteImage;
    private bool isInRange = false;
    private bool isNoteVisible = false;

    private PlayerMovement playerMovement;
    private Rigidbody playerRigidbody;

    private string sceneToLoad;

    public GameObject note;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            playerRigidbody = player.GetComponent<Rigidbody>();
        }

        noteImage.enabled = false; // Ensure the note starts hidden
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E) && !isNoteVisible)
        {
            ShowNote();
        }

        if (isNoteVisible)
        {
            if (Input.GetKeyDown(KeyCode.N)) // N to close the note
            {
                HideNote();
            }
            else if (Input.GetKeyDown(KeyCode.Y)) // Y to change scene
            {
                ChangeScene();
            }
        }
    }

    private void ShowNote()
    {
        isNoteVisible = true;
        sceneToLoad = note.tag; // Store the tag as the scene name
        noteImage.enabled = true;

        if (playerMovement != null)
            playerMovement.enabled = false; // Disable movement

        if (playerRigidbody != null)
            playerRigidbody.isKinematic = true; // Freeze physics

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HideNote()
    {
        isNoteVisible = false;
        noteImage.enabled = false;

        if (playerMovement != null)
            playerMovement.enabled = true; // Re-enable movement

        if (playerRigidbody != null)
            playerRigidbody.isKinematic = false; // Unfreeze physics

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene name is empty or not assigned!");
        }
    }
}
