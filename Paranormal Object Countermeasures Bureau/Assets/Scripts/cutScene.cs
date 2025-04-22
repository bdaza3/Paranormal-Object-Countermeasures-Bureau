using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class cutScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] textObjects; // Array to hold the 3 text objects
    [SerializeField] private GameObject[] backgrounds; // Backgrounds for the cutscene
    public float typingSpeed = 0.03f; // Speed of the typewriter effect
    public float displayTime = 3f; // Time to display each sentence before moving to the next
    public AudioSource typingSound; // Audio source for the typing sound

    private Coroutine currentRoutine;

    void Start()
    {
        // Deactivate all text objects and backgrounds at the start
        foreach (TextMeshProUGUI textObject in textObjects)
        {
            textObject.gameObject.SetActive(false);
        }


    }

    // Function to start the cutscene
    public void StartCutscene()
    {
        if (currentRoutine == null && textObjects.Length > 0 && backgrounds.Length > 1)
        {
            currentRoutine = StartCoroutine(DisplaySentences());
        }
    }

    private IEnumerator DisplaySentences()
    {
        foreach (TextMeshProUGUI textObject in textObjects)
        {
            // Activate the second background and deactivate the first one
            backgrounds[0].SetActive(false);
            backgrounds[1].SetActive(true);

            // Activate the current text object
            textObject.gameObject.SetActive(true);

            string message = textObject.text; // Get the text written in the TextMeshProUGUI component
            textObject.text = ""; // Clear the text
            textObject.alpha = 1; // Ensure the text is visible

            foreach (char letter in message.ToCharArray())
            {
                textObject.text += letter; // Add one letter at a time
                if (typingSound != null && !typingSound.isPlaying)
                {
                    typingSound.Play(); // Play the typing sound
                }
                yield return new WaitForSeconds(typingSpeed);
            }

            if (typingSound != null)
            {
                typingSound.Stop(); // Stop the typing sound after the sentence is complete
            }

            yield return new WaitForSeconds(displayTime); // Wait before moving to the next sentence

            // Optional: Fade out the text
            float fadeTime = 1f;
            for (float t = 0; t < fadeTime; t += Time.deltaTime)
            {
                textObject.alpha = Mathf.Lerp(1, 0, t / fadeTime);
                yield return null;
            }

            textObject.text = ""; // Clear the text after fading out

            // Deactivate the current text object
            textObject.gameObject.SetActive(false);
        }

        currentRoutine = null; // Reset the coroutine reference when done
        SceneManager.LoadScene("Copy3rd");
    }
}
