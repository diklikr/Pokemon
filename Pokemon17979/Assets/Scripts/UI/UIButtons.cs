using UnityEngine;
using UnityEngine.SceneManagement;
public class UIButtons: MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Game");
    }
    public void RestartButton()
    {
        SceneManager.LoadScene("Menu");
    }
    public void ExitButton()
    {
            Application.Quit();
    }
}
