using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IngameTimeManager : MonoBehaviour
{
    public static IngameTimeManager _instance;

    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI startTimeText;
    [SerializeField]
    private TextMeshProUGUI victoryText;

    public float time { get; private set; } = 45;

    private float timeWaited;

    private bool canPassTime = false;

    private bool waitingBeforeScore = false;
    private bool waitingBeforeStart = false;
    [HideInInspector]
    public bool endGame = false;

    private string lastSecond;
    private AudioClip timerSound;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(_instance.gameObject);
            }
        }

        _instance = this;

        startTimeText.text = "";

        timerSound = Resources.Load("Sounds/CountDownLoop") as AudioClip;

    }

    // Update is called once per frame
    void Update()
    {
        Timer();

        WaitBeforeScore();
        WaitThreeSecondsToStart();
    }

    private void Timer()
    {
        if (canPassTime)
        {
            time -= Time.deltaTime;
            string prefix = "";
            if (time < 9)
            {
                prefix = "0";
            }
            timeText.text = prefix + time.ToString("f0");
            if (time <= 0)
            {
                EndGame();
            }
        }
        
    }

    private void EndGame()
    {
        timeText.text = "00";
        waitingBeforeScore = false;
        waitingBeforeStart = false;
        endGame = true;
        if (IngameScoreManger._instance.player1Score > IngameScoreManger._instance.player2Score)
        {
            victoryText.text = "¡" + CurrentCardsController._instance.userPlayerSelected + " ha ganado!";
            if (time <= -3)
            {
                PowerUpController._instance.ResetPowerUps();
                SceneManager.LoadScene("VictoryScene");
            }
        }
        else
        {
            if (IngameScoreManger._instance.player1Score < IngameScoreManger._instance.player2Score)
            {
                victoryText.text = "¡" + CurrentCardsController._instance.enemyPlayerSelected + " ha ganado!";
            }
            else
            {
                victoryText.text = "¡ EMPATE!";
            }

            if (time <= -3)
            {
                PowerUpController._instance.ResetPowerUps();
                SceneManager.LoadScene("MainMenu");
            }
        }


    }

    public void StartWaitBeforeScore()
    {
        waitingBeforeScore = true;
        timeWaited = 1;
        canPassTime = false;
    }

    private void WaitBeforeScore()
    {
        if (waitingBeforeScore)
        {
            timeWaited -= Time.deltaTime;
            
            if (timeWaited <= 0)
            {
                waitingBeforeScore = false;
                FootballFieldController._instance.RestartPlayers();
            }
        }
    }

    public void StartWaitThreeSecondsToStart()
    {
        waitingBeforeStart = true;
        timeWaited = 4f;

        startTimeText.text = "0" + timeWaited.ToString("f0");
        lastSecond = timeWaited.ToString("f0");
        AudioManager._instance.Play2dOneShotSound(timerSound, 0.5f, 0.5f, 1);
    }

    private void WaitThreeSecondsToStart()
    {
        if (waitingBeforeStart)
        {
            timeWaited -= Time.deltaTime;
            startTimeText.text = "0" + timeWaited.ToString("f0");
            if (lastSecond != timeWaited.ToString("f0"))
            {
                Debug.Log((int)timeWaited);
                switch (timeWaited.ToString("f0"))
                {
                    case "0":
                        AudioManager._instance.Play2dOneShotSound(timerSound, 1.5f, 1.5f, 1);
                        break;
                    case "1":
                        AudioManager._instance.Play2dOneShotSound(timerSound, 1f, 1, 1);
                        break;
                    case "2":
                        AudioManager._instance.Play2dOneShotSound(timerSound, 0.5f, 0.5f, 1);
                        break;
                    default:
                        break;
                }
                lastSecond = timeWaited.ToString("f0");
            }
           
            if (timeWaited <= 0)
            {
                startTimeText.text = "";
                waitingBeforeStart = false;
                canPassTime = true;
                FootballFieldController._instance.ContinueAllEntities();
            }
        }

    }
}
