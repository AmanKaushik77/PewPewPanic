using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void PlayGame()
    {
        SceneManager.LoadSceneAsync("MainGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
