using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    [Header("References")]
    [SerializeField] private PlayerSettings pSettings;
    private Pickup pickup;

    [Header("Objects")]
    [SerializeField] private GameObject doubleJumpText;
    [SerializeField] private GameObject dashText;
    [SerializeField] private TextMeshProUGUI arkText;
    [SerializeField] private TextMeshProUGUI dashCooldownText;
    [SerializeField] private GameObject pauseMenu;

    private bool isPaused;
    private void Start()
    {
        pickup = FindObjectOfType<Pickup>();

        //set starting alpha of dashcooldown to 0
        dashCooldownText.alpha = 0;
    }

    private void Update()
    {
        arkText.text = string.Format("{0} / ???", pickup.amountPickedUp);

        dashCooldownText.text = string.Format("{0}", pSettings.dashCooldown.ToString("F"));

        //sets the alpha of the dash cooldown to zero if cooldown is over
        if (pSettings.dashCooldown <= 0)
            dashCooldownText.alpha = 0;
        else
            dashCooldownText.alpha = 255;

        if (pickup.doubleJumpAcquired)
            doubleJumpText.SetActive(true);

        if (pickup.dashAcquired)
            dashText.SetActive(true);

        //pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;

                Cursor.lockState = CursorLockMode.None;
            }
            if (!isPaused)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;

                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
