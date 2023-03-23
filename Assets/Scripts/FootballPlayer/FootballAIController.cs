using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballAIController : FootballEntityController
{

    [HideInInspector]
    public GameObject goalAI;
    public bool rightSide;
    [HideInInspector]
    public GameObject ball;
    [SerializeField]
    private float minDistanceFromBallY = 0.8f;
    [SerializeField]
    private float minDistanceToAttack = 0.7f;
    [SerializeField]
    private float maxDistanceFromAIGoal = 9;
    private float minDistanceFromAIGoal = 3;
    [SerializeField]
    private bool returningToAIGoal = false;

    private bool isJumping = false;
    private float timeToWaitJump = 1.5f;
    private float timeWaitedJump = 0;

    private bool canDoAction = true;
    private float timeToWaitAction = 1;
    private float timeWaitedAction = 0;
    


    // Update is called once per frame
    void Update()
    {
        AIBehaviour();
    }

    private void AIBehaviour()
    {
        float distanceFromBallX = CheckDistanceBallX();
        float distanceFromAIGoal = CheckDistanceFromAIGoal();

        Debug.Log("Distancia en X " + distanceFromBallX);
        Debug.Log("Distancia de la Porteria " + distanceFromAIGoal);

        CheckAIMovement(distanceFromAIGoal);

        JumpTimer();
        float distanceFromBallY = CheckJump();

        WaitActionCD();
        CheckIfDoAction(distanceFromBallX, distanceFromBallY);
       

    }

    private float CheckDistanceBallX() 
    {
        float distanceFromBallX;
        if (rightSide)
        {
            distanceFromBallX = ball.transform.position.x - transform.position.x;
        }
        else
        {
            distanceFromBallX = transform.position.x - ball.transform.position.x;

        }

        return distanceFromBallX;
    }

    private float CheckDistanceFromAIGoal()
    {
        float distanceFromAIGoal;
        if (rightSide)
        {
            distanceFromAIGoal = transform.position.x - goalAI.transform.position.x;
        }
        else
        {
            distanceFromAIGoal = goalAI.transform.position.x - transform.position.x;

        }

        return distanceFromAIGoal;
    } 

    private void CheckAIMovement(float _distanceFromAIGoal) 
    {
        //Comprobar la distancia en x de la pelota por si tiene que moverse y no esta muy lejos de la porteria ir a por la pelota
        //Si la pelota esta atras ir hacia la porteria
        if (_distanceFromAIGoal <= maxDistanceFromAIGoal && !returningToAIGoal)
        {
            float dir;
            //Ir hacia la pelota
            if (transform.position.x - ball.transform.position.x > 0)
            {
                dir = -1;
            }
            else
            {
                dir = 1;
            }

            Debug.Log("Va hacia la pelota la direccion es " + dir);

            MoveFootballPlayer(dir);

        }
        else if (_distanceFromAIGoal > maxDistanceFromAIGoal || returningToAIGoal)
        {
            returningToAIGoal = true;
            float dir;
            //Ir hacia la porteria
            if (transform.position.x - goalAI.transform.position.x > 0)
            {
                dir = -1;
            }
            else
            {
                dir = 1;
            }
            
            Debug.Log("Va hacia la porteria la direccion es " + dir);

            MoveFootballPlayer(dir);

            if (_distanceFromAIGoal <= minDistanceFromAIGoal)
            {
                returningToAIGoal = false;
            }
        }
    }

    private void JumpTimer()
    {
        if (isJumping)
        {
            timeWaitedJump += Time.deltaTime;
            if (timeWaitedJump >= timeToWaitJump)
            {
                timeWaitedJump = 0;
                isJumping = false;
            }

        }
    }

    private float CheckJump()
    {
        float distanceFromBallY = ball.transform.position.y - transform.position.y;
        //Comprobar la distancia en Y de la pelota para saber si tiene que saltar
        if (distanceFromBallY > minDistanceFromBallY && !isJumping)
        {
            Jump();
            isJumping = true;
        }

        return distanceFromBallY;
    }
    
    private void WaitActionCD()
    {
        if (!canDoAction)
        {
            timeWaitedAction += Time.deltaTime;
            if (timeWaitedAction >= timeToWaitAction)
            {
                timeWaitedAction = 0;
                canDoAction = true;
            }
        }
    }
    private void CheckIfDoAction(float _distanceFromBallX, float _distanceFromBallY)
    {
        //Si la pelota esta suficientemente cerca de la AI "atacar"
        if (_distanceFromBallX <= minDistanceToAttack && canDoAction)
        {
            //Comprobar la altura de la pelota para saber que accion hacer
            if (_distanceFromBallY > 0.3f)
            {
                //Si la pelota esta mas alta de la mitad dar un cabezazo
                HeadButt();

            }
            else
            {
                //Si no da una patada
                Kick();
            }
            canDoAction = false;
        }
    }


}
