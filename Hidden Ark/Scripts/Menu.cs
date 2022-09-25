using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
   public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        print("You quit");
        Application.Quit();
    }

    public void LockCursor()
    {
        print("a");
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1;
    }
}
