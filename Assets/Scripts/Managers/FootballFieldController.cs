using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballFieldController : MonoBehaviour
{

    public static FootballFieldController _instance;


    public FootballEntityDefaultStats playerEntityValues;
    public FootballEntityDefaultStats AIEntityValues;

    [SerializeField]
    private GameObject footballEntity1;
    [SerializeField]
    private GameObject footballEntity2;
    [SerializeField]
    private GameObject[] goals;
    private GameObject ball;

    public bool playerFirstPos;

    private PlayerFootballController playerEntity;
    private Vector2 playerStartPos;
    private FootballAIController AIEntity;
    private Vector2 AIStartPos;
    private Vector2 ballStartPos;


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
        ball = GameObject.FindGameObjectWithTag("Ball");
        ballStartPos = ball.transform.position;
        if (playerFirstPos)
        {
            //Anyadir el componente de player al primer jugador
            playerEntity = footballEntity1.AddComponent<PlayerFootballController>();
            //Anyadir el componente de IA al segundo jugador
            AIEntity = footballEntity2.AddComponent<FootballAIController>();
            AIEntity.rightSide = !playerFirstPos;
            AIEntity.goalAI = goals[1];
            
        }
        else
        {
            //Anyadir el componente de IA al primer jugador
            AIEntity = footballEntity1.AddComponent<FootballAIController>();
            AIEntity.rightSide = !playerFirstPos;
            AIEntity.goalAI = goals[0];
            //Anyadir el componente de player al segundo jugador
            playerEntity = footballEntity2.AddComponent<PlayerFootballController>();
        }

        playerEntity.footballEntityValues = playerEntityValues;
        playerStartPos = playerEntity.transform.position;

        AIEntity.footballEntityValues = AIEntityValues;
        AIEntity.ball = ball;
        AIStartPos = AIEntity.transform.position;
        

        RestartPlayers();

    }

    public void RestartPlayers()
    {
        //Desactivar scripts de movimiento
        StopAllEntities();

        //Empezar contador de 3 segundos
        IngameTimeManager._instance.StartWaitThreeSecondsToStart();

        //Activar scripts tras acabar el contador

    }

    public void StopAllEntities()
    {
        playerEntity.enabled = false;
        AIEntity.enabled = false;

        //Hacer TP a su posicion original a los players y la pelota

        playerEntity.transform.position = playerStartPos;
        Rigidbody2D playerRb = playerEntity.GetComponent<Rigidbody2D>();
        playerRb.velocity = Vector2.zero;
        playerRb.simulated = false;

        AIEntity.transform.position = AIStartPos;
        Rigidbody2D AIRb = AIEntity.GetComponent<Rigidbody2D>();
        AIRb.velocity = Vector2.zero;
        AIRb.simulated = false;

        ball.transform.position = ballStartPos;
        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
        ballRb.velocity = Vector2.zero;
        ballRb.simulated = false;
    }

    public void ContinueAllEntities()
    {
        playerEntity.GetComponent<Rigidbody2D>().simulated = true;
        AIEntity.GetComponent<Rigidbody2D>().simulated = true;
        ball.GetComponent<Rigidbody2D>().simulated = true;

        playerEntity.enabled = true;
        AIEntity.enabled = true;
    }

}
