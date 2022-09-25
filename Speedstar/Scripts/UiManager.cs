//https://www.youtube.com/watch?v=CC8j_fU2GTQ credit to this guy for the speedOmeter
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    private Steering steering;

    [Header("Speed O Meter")]
    public Rigidbody Target;
    public TextMeshProUGUI SpeedLabel; //the label that dispalyes the speed
    public RectTransform Arrow;        //the arrow in the speedometer
    public float MaxSpeed = 0.0f; //maximum speed of the target in KM/H
    public float MinSpeedArrowAngle; //minimum angle of the speedometer arrow
    public float MaxSpeedArrowAngle; //maximum angle of the speedometer arrow

    [Header("UI Text")] //text objects to be displayed in the scene ui
    public TextMeshProUGUI CurrentLapText;
    public TextMeshProUGUI LapTimeText1;
    public TextMeshProUGUI LapTimeText2;
    public TextMeshProUGUI LapTimeText3;
    public TextMeshProUGUI TotalRaceTimeText;
    public TextMeshProUGUI CountDownText;

    [Header("Lap related")]
    public List<float> topTimes = new List<float>(10); //list to hold your top10 race times
    private float totalRaceTime; //variable to store calculated race time when a race is finished
    private float currentLapTime = 0; //timer that starts counting up
    private float lapTime1 = 0; //variable to store a laps time
    private float lapTime2 = 0; //variable to store a laps time
    private float lapTime3 = 0; //variable to store a laps time
    private int currentLap = 0; //variable to switch laps 

    [Header("CountDown")]
    private float countDown = 3;

    [Header("Pause Menu")]
    public GameObject PauseMenu;
    private bool isPaused = false;

    [Header("Respawn Curtain")]
    public RawImage curtainObject;

    [Header("Star")]
    [SerializeField] private Star star;

    private void Awake()
    {
        //singleton
        if (instance == null)
            instance = this;

        steering = Steering.FindObjectOfType<Steering>();


        //lock playermovement
        steering.SetLockState(Steering.LockState.Locked);

        //fills the topTimes list
        for (int i = 0; i < topTimes.Count; i++)
        {
            topTimes[i] = PlayerPrefs.GetFloat("TopTimes" + i, 99999);
        }

        //resets paused bool at start of the game
        isPaused = false;

        //lock cursor and make it invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveLapTime();
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayerPrefsTimes();
        }


        //pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                PauseMenu.SetActive(true);
                Time.timeScale = 0;

                //lock cursor and make it invisible
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            if (!isPaused)
            {
                PauseMenu.SetActive(false);
                Time.timeScale = 1;

                //makes cursor visible and unlocks it
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        //Speed O Meter
        {
            //  Rigidbody speed is in m/s, here we convert it to km/h
            var speed = Target.velocity.magnitude / 1000 * 60 * 60;

            var pointerPercentage = speed / MaxSpeed;
            var pointerAngle = Mathf.Lerp(MinSpeedArrowAngle, MaxSpeedArrowAngle, pointerPercentage);

            if (SpeedLabel)
                SpeedLabel.text = $"{(int)speed} km/h";

            if (Arrow)
                Arrow.localEulerAngles = new Vector3(0, 0, pointerAngle);
        }
    }

    private void FixedUpdate()
    {
        countDown -= Time.fixedDeltaTime;

        if (countDown > 0.5)
            CountDownText.text = string.Format("{0}", Mathf.Round(countDown));

        if (countDown <= 0.5)
        {
            StartCoroutine(CountDown());

            //laptime
            currentLapTime += Time.fixedDeltaTime;
            CurrentLapText.text = string.Format("Current Lap : {0}", currentLapTime.ToString("F"));
        }
    }


    /// <summary>
    /// function that checks and updates the currentlap and text calls CalculateRaceTime when race is finished
    /// </summary>
    public void SaveLapTime()
    {
        //resets laptime for a new lap and sets old laptime in a new variable to save it
        currentLap++;

        if (currentLap == 1)
        {
            lapTime1 = currentLapTime;
            currentLapTime = 0;

            //move new lap down with animations and set the text
            LapTimeText1.gameObject.SetActive(true);
            LapTimeText1.text = string.Format("Lap 1 : {0}", lapTime1.ToString("F"));
        }

        if (currentLap == 2)
        {
            lapTime2 = currentLapTime;
            currentLapTime = 0;

            //move new lap down with animations and set the text
            LapTimeText2.gameObject.SetActive(true);
            LapTimeText2.text = string.Format("Lap 2 : {0}", lapTime2.ToString("F"));
        }

        if (currentLap == 3)
        {
            lapTime3 = currentLapTime;
            currentLapTime = 0;

            //move new lap down with animations and set the text
            LapTimeText3.gameObject.SetActive(true);
            LapTimeText3.text = string.Format("Lap 3 : {0}", lapTime3.ToString("F"));

            //turns off currentlap text
            CurrentLapText.gameObject.SetActive(false);
                        
            StartCoroutine(CalculateRaceTime());
        }
    }

    /// <summary>
    /// function to resume the game, to be called with an onclick event in a button
    /// </summary>
    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;

        //lock cursor and make it invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Sets the black RawImage object in the UI to the given opacity.
    /// </summary>
    public void SetCurtainOpacity(float opacity)
    {
        curtainObject.color = new Color(0f, 0f, 0f, opacity);

        if (opacity > 0)
        {
            curtainObject.gameObject.SetActive(true);
            return;
        }

        curtainObject.gameObject.SetActive(false);
    }

    /// <summary>
    /// couroutine to calculate TotalRaceTime and saves it in a playerpref
    /// </summary>
    /// <returns></returns>
    IEnumerator CalculateRaceTime()
    {
        yield return new WaitForSeconds(1);

        //calculates total race time from all laps
        totalRaceTime = lapTime1 + lapTime2 + lapTime3;

        //save times in playerprefs
        PlayerPrefs.SetFloat("TotalRaceTime", totalRaceTime);

        TotalRaceTimeText.gameObject.SetActive(true);
        TotalRaceTimeText.text = string.Format("Total Race Time : {0}", PlayerPrefs.GetFloat("TotalRaceTime").ToString("F"));


        //set car movement to no input to simulate a finish
        steering.SetLockState(Steering.LockState.NoInput);

        //fills the topTimes list
        for (int i = 0; i < topTimes.Count; i++)
        {
            if (totalRaceTime < topTimes[i])
            {
                Debug.Log("if working");
                topTimes.Insert(i, totalRaceTime);
                topTimes.RemoveAt(topTimes.Count - 1);
                break;
            }
        }

        //sets the TopTimes playerpref
        for (int i = 0; i < topTimes.Count; i++)
        {
            PlayerPrefs.SetFloat("TopTimes" + i, topTimes[i]);
        }

        //saves new highscore in a float, can be get from SavingRaceTimes script
        if (totalRaceTime < PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", topTimes[0]);
        }

        PlayerPrefs.Save();

        //call function to go back to menu here

        yield return new WaitForSeconds(3);
        GameManager.instance.RespawnCar(3, 3);
        yield return new WaitForSeconds(3);

        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// couroutine that changes the cooldown text to GO!
    /// </summary>
    /// <returns></returns>
    IEnumerator CountDown()
    {
        //unlock player movement
        steering.SetLockState(Steering.LockState.Free);

        //turns star on
        star.Launch();

        CountDownText.text = "GO!";

        yield return new WaitForSeconds(2);

        CountDownText.gameObject.SetActive(false);
    }

    /// <summary>
    /// developer function to reset top 10 times and highscore
    /// </summary>
    private void ResetPlayerPrefsTimes()
    {
        for (int i = 0; i < topTimes.Count; i++)
        {
            PlayerPrefs.SetFloat("TopTimes" + i, 9999);
        }
        PlayerPrefs.SetFloat("HighScore", 9999);
    }
}
