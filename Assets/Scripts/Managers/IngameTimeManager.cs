using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IngameTimeManager : MonoBehaviour
{
    public static IngameTimeManager _instance;

    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI startTimeText;

    private float time = 45;

    private float timeWaited;

    private bool canPassTime = false;

    private bool waitingBeforeScore = false;
    private bool waitingBeforeStart = false;


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
        }
        
    }

    private void EndGame()
    {

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
        timeWaited = 3;
        startTimeText.text = "0" + timeWaited.ToString("f0");

    }

    private void WaitThreeSecondsToStart()
    {
        if (waitingBeforeStart)
        {
            timeWaited -= Time.deltaTime;
            startTimeText.text = "0" + timeWaited.ToString("f0");
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
