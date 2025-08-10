using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFunctions : MonoBehaviour 
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenSettings()
    {
        SceneManager.LoadScene(2);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void BackToSettings()
    {
        SceneManager.LoadScene(0);
    }
}
