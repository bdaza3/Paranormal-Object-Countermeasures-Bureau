using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class creditsRolling : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] textObjects; // Array to hold the text objects for the credits
    public float typingSpeed = 0.03f; // Speed of the typewriter effect
    public float displayTime = 3f; // Time to display each sentence before moving to the next
    public AudioSource backgroundMusic; // Audio source for the background music

    private Coroutine currentRoutine;

    void Start()
    {
        // Deactivate all text objects at the start
        foreach (TextMeshProUGUI textObject in textObjects)
        {
            textObject.gameObject.SetActive(false);
        }

        // Start the credits rolling
        if (textObjects.Length > 0)
        {
            StartCredits();
        }
    }

    // Function to start the credits rolling
    public void StartCredits()
    {
        if (currentRoutine == null)
        {
            if (backgroundMusic != null)
            {
                backgroundMusic.Play(); // Start the background music
            }
            currentRoutine = StartCoroutine(DisplayCredits());
        }
    }

    private IEnumerator DisplayCredits()
    {
        foreach (TextMeshProUGUI textObject in textObjects)
        {
            // Activate the current text object
            textObject.gameObject.SetActive(true);

            string message = textObject.text; // Get the text written in the TextMeshProUGUI component
            textObject.text = ""; // Clear the text
            textObject.alpha = 1; // Ensure the text is visible

            // Typewriter effect
            foreach (char letter in message.ToCharArray())
            {
                textObject.text += letter; // Add one letter at a time
                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitForSeconds(displayTime); // Wait before moving to the next text object

            // Deactivate the current text object
            textObject.gameObject.SetActive(false);
        }

        if (backgroundMusic != null)
        {
            backgroundMusic.Stop(); // Stop the background music when credits finish
        }

        currentRoutine = null; // Reset the coroutine reference when done
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true;
        SceneManager.LoadScene("MenuScene");
    }
}
