using UnityEngine;
using TMPro;
using System.Collections;

public class ObjectiveManager : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;

    private string currentObjective;

    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    private IEnumerator FadeObjective()
    {
        canvasGroup.alpha = 0;
        UpdateObjectiveUI();
        yield return new WaitForSeconds(0.1f);

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
    }

    public void SetObjective(string newObjective)
    {
        currentObjective = newObjective;
        StartCoroutine(FadeObjective());
        UpdateObjectiveUI();
    }


    private void UpdateObjectiveUI()
    {
        objectiveText.text = currentObjective;
    }

}

