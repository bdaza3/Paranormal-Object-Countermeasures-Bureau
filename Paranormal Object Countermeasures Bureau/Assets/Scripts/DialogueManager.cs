using UnityEngine;
using TMPro;
using System.Collections;

public class ThoughtDialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    public TextMeshProUGUI noteText;
    public float displayTime = 3f;

    private Coroutine currentRoutine;

    private bool note = false; //visibility of the note

    public void ShowThought(string message, System.Action onComplete = null)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(DisplayThought(message, onComplete));
    }

    public void ShowNote(string message, System.Action onComplete = null)
    {
        note = true; //set the note to be visible
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(DisplayThought(message, onComplete));
        note = false; //set the note to be invisible again so that dialogue can show
    }

    private IEnumerator DisplayThought(string message, System.Action onComplete) //shows thought 
    {
    if (!note){//if not a note and just dialogue
        dialogueText.text = "";
        dialogueText.alpha = 1;

        float typingSpeed = 0.03f; //smaller is faster
        foreach (char letter in message.ToCharArray()) //typewriter effect
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(displayTime);

        float fadeTime = 1f;
        for (float t = 0; t < fadeTime; t += Time.deltaTime) //fade out effect
        {
            dialogueText.alpha = Mathf.Lerp(1, 0, t / fadeTime);
            yield return null;
        }

        dialogueText.text = "";

        onComplete?.Invoke();
    }
    else{//if it is a note
        noteText.text = "";
        noteText.alpha = 1;
        //get the bool from the note if it is faculty or red room
        float typingSpeed = 0.02f; //smaller is faster
        NoteAppear noteAppear = FindFirstObjectByType<NoteAppear>();
        if (noteAppear.facultyNote){
            noteText.fontSize = 15;
            typingSpeed = 0.02f; //smaller is faster
        }
        else if (noteAppear.redRoomNote){
            noteText.fontSize = 22;
            typingSpeed = 0.05f; //smaller is faster
        }
        foreach (char letter in message.ToCharArray())
        {
            noteText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    }
}
