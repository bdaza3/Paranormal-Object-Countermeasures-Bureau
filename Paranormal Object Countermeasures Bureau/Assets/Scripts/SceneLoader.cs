using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGameScene()
    {
        
        SceneManager.LoadScene("Copy3rd");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Copy3rd"));
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("credits");
    }

    public static void LoadGameStatic()
    {
        SceneManager.LoadScene("credits");
    }

    public static void LoadMenuStatic()
    {
        SceneManager.LoadScene("2ndFloor");
    }
}
