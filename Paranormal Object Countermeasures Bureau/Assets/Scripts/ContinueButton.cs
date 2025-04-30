using UnityEngine;

public class ContinueButton : MonoBehaviour
{
public GameObject continueButton;

void Start()
{
    if (PlayerPrefs.GetInt("ReachedLevel2", 0) == 1)
    {
        continueButton.SetActive(true);
    }
    else
    {
        continueButton.SetActive(false);
    }
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
