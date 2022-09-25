using UnityEngine;
using TMPro;

public class SavingRaceTimes : MonoBehaviour
{
    public TextMeshProUGUI HighScoreText;
    private float highScore;

    [SerializeField] private TextMeshProUGUI[] allRaceTimes;

    private void Start()
    {
        GetPlayerPrefs();

        highScore = PlayerPrefs.GetFloat("HighScore", 0);

        HighScoreText.text = string.Format(PlayerPrefs.GetFloat("HighScore").ToString("F"));

        RaceTimes();
    }

    public void GetPlayerPrefs()
    {
        //gets the list of topTimes
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.GetFloat("TopTimes" + i, 0);
        }
    }

    public void RaceTimes()
    {
        //sets the text arary with the topTimes list
        for (int i = 0; i < allRaceTimes.Length; i++)
        {
            allRaceTimes[i].text = string.Format("Race {0} : {1} ", i + 1, PlayerPrefs.GetFloat("TopTimes" + i).ToString("F"));
        }
    }
}
