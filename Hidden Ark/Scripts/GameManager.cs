using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject WinPanel;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        //player beneath the map check
        /*if (player.transform.position.y <= -6)
        {
            player.transform.position = new Vector3(0, 3, 10);
            SceneManager.LoadScene(1);
        }*/

    }

    public void ActivateWin()
    {
        print("Game ended");
        WinPanel.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
