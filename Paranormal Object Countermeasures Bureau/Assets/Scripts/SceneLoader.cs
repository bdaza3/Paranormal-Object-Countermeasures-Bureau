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

    public static void LoadGameStatic()
    {
        SceneManager.LoadScene("Copy3rd");
    }

    public static void LoadMenuStatic()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
