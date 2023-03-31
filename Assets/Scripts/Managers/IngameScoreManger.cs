using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngameScoreManger : MonoBehaviour
{
    public static IngameScoreManger _instance;

    [SerializeField]
    private TextMeshProUGUI player1ScoreText;
    [SerializeField]
    private TextMeshProUGUI player2ScoreText;

    [HideInInspector]
    public int player1Score;
    [HideInInspector]
    public int player2Score;


    private void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(_instance.gameObject);
            }
        }

        _instance = this;
    }

    private void Start()
    {
        player1Score = 0;
        player2Score = 0;
    }

    private void UpdateScores()
    {
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }

    
    public void ScoreGoal(bool _rightPlayer)
    {
        if (_rightPlayer)
        {
            player1Score++;
        }
        else
        {
            player2Score++;
        }

        UpdateScores();

    }

}
