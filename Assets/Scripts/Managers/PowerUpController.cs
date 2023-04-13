using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PowerUpController : MonoBehaviour
{
    public enum PowerUps { NONE = 0, BULLET_TIME, POWER_SHOT, INVERTED_INPUT, SPRING_FEET, ZERO_GRAVITY };
    public PowerUps currentPowerUp = PowerUps.NONE;
    public PowerUps lastPowerUp = PowerUps.NONE;

    [SerializeField]
    private float timeToSpawnPowerUp;
    private float startTime = 0;

    [Header("SlowDown Power Up"), SerializeField]
    private float slowdownValue;

    [Header("Zero Gravity Power Up"), SerializeField]
    private float zeroGravityValue;
    private float starterGravity;

    private void Start()
    {
        startTime = IngameTimeManager._instance.time;
        starterGravity = Physics2D.gravity.y;
    }

    private void Update()
    {
        CheckIfCanApplyPowerUp();
    }


    private void CheckIfCanApplyPowerUp()
    {
        if (currentPowerUp == PowerUps.NONE)
        {
            if (startTime - IngameTimeManager._instance.time >= timeToSpawnPowerUp)
            {
                GenerateRandomPowerUp();
            }
        }

    }

    private void GenerateRandomPowerUp()
    {
        int randNum = Random.Range(1, (int)PowerUps.ZERO_GRAVITY + 1);

        if ((PowerUps)randNum == lastPowerUp)
        {
            GenerateRandomPowerUp();
        }
        else
        {
            ChangePowerUp((PowerUps)randNum);
        }

    }

    private void ChangePowerUp(PowerUps _nextPowerUp)
    {
        lastPowerUp = currentPowerUp;
        currentPowerUp = _nextPowerUp;
        switch (_nextPowerUp)
        {
            case PowerUps.BULLET_TIME:
                //relentizar el tiempo
                break;
            case PowerUps.POWER_SHOT:
                //Cambiar algo en la bola para que al tocar con un collider salga disparada y poner particulas en los pies de los Jugadores 
                break;
            case PowerUps.INVERTED_INPUT:
                //Cambiar el input de los Jugadores
                break;
            case PowerUps.SPRING_FEET:
                //Actvar algo en los jugadores para que cada vez que toquen el suelo salten

                break;
            case PowerUps.ZERO_GRAVITY:
                //Cambiar la gravedad para que las entidades tengan muy poca gravedad
                Physics2D.gravity = new Vector2(0, zeroGravityValue);
                break;
            default:
                break;
        }
    }


    public void ResetPowerUps()
    {
        lastPowerUp = currentPowerUp;
        currentPowerUp = PowerUps.NONE;
        startTime = IngameTimeManager._instance.time;
        Physics2D.gravity = new Vector2(0, starterGravity);
    }
}
