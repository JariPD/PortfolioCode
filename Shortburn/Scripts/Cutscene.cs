using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CutSceneEnd()
    {
        SceneManager.LoadScene(3);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
