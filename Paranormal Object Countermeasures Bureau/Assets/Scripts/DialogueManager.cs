using UnityEngine;
using TMPro;
using System.Collections;
using JetBrains.Annotations;

public class ThoughtDialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    public TextMeshProUGUI noteText;

    public TextMeshProUGUI hoverText;
    public float displayTime = 3f;

    private Coroutine currentRoutine;

    private bool note = false; //visibility of the note

    private bool interactionhover = false; //if the player is hovering over an object to interact with

    public bool redRoomNote = false; //if the note is a red room note
    public bool facultyNote = false; //if the note is a faculty note

    public bool artNote = false; //if the note is an art note
    
    public void ShowThought(string message, System.Action onComplete = null)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(DisplayThought(message, onComplete));
    }

    public void ShowNote(string message, System.Action onComplete = null)
    {
        note = true; 
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(DisplayThought(message, onComplete));
        note = false; 
    }

    public void ShowHoverText(string message)//shows the hover text instantly
    {
        DisplayHoverText(message);
    }

    private IEnumerator DisplayThought(string message, System.Action onComplete) //shows thought 
    {
    if (!note){//if not a note and just dialogue
        dialogueText.text = "";
        dialogueText.alpha = 1;
        dialogueText.fontSize = 25; //default font size

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


        if (redRoomNote){//RED ROOM
            noteText.fontSize = 30;
            typingSpeed = 0.05f; //smaller is faster
        }
        else if (facultyNote){//FACULTY
            noteText.fontSize = 22;
            typingSpeed = 0.03f;
        }
        else if (artNote){//ART
            noteText.fontSize = 50;
            typingSpeed = 0.05f;
        }
        foreach (char letter in message.ToCharArray())
        {
            noteText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    }

    private void DisplayHoverText(string message) //shows hover text instantly
    {
        hoverText.text = message;
        hoverText.alpha = 1;
    }
}
