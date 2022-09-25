using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public bool IsInteracting = false;
    public bool IsPaused;
    public bool ObjectSelected;

    public GameObject RestartImage;
    [SerializeField] private TextMeshProUGUI stayInCenterText;
    [SerializeField] private TextMeshProUGUI moveToCenterText;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private Canvas interactCanvas;
    [SerializeField] private Canvas objectSelectedCanvas;

    private bool coroutineAllowed = true;

    private void Awake()
    {
        instance = this;

        //locks cursor and makes it invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        StartCoroutine(StartingCoroutine());
    }

    void Update()
    {
        //if player is ineracting turns interacting text on or off
        if (IsInteracting)
            interactCanvas.enabled = true;
        else
            interactCanvas.enabled = false;

        //if object is selected turn on text that tells player how to deselect
        if (ObjectSelected)
            objectSelectedCanvas.enabled = true;
        else
            objectSelectedCanvas.enabled = false;

        //pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = !IsPaused;

            if (IsPaused)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                LockPlayer.instance.LockAll();
                pauseMenu.SetActive(true);

            }
            if (!IsPaused)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                LockPlayer.instance.UnlockAll();
                pauseMenu.SetActive(false);
            }
        }
    }

    public IEnumerator StayInCenter()
    {
        stayInCenterText.enabled = true;

        yield return new WaitForSeconds(4.5f);

        stayInCenterText.enabled = false;
    }

    //coroutine called when player has to move to the center
    public IEnumerator MoveToCenter()
    {
        if (coroutineAllowed)
        {
            coroutineAllowed = false;

            moveToCenterText.enabled = true;

            yield return new WaitForSeconds(4.5f);

            moveToCenterText.enabled = false;
        }
    }

    IEnumerator StartingCoroutine()
    {
        yield return new WaitForSeconds(25);

        StartCoroutine(ShowRestartImage());

        yield break;
    }

    public IEnumerator ShowRestartImage()
    {
        RestartImage.SetActive(true);

        yield return new WaitForSeconds(5);

        RestartImage.SetActive(false);

        yield break;
    }
}
