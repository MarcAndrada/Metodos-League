using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public enum PowerUps { NONE, BULLET_TIME, POWER_SHOT, INVERTED_INPUT, SPRING_FEET, ZERO_GRAVITY };
    public PowerUps currentPowerUp = PowerUps.NONE;
    public PowerUps lastPowerUp = PowerUps.NONE;

    [SerializeField]
    private float timeToSpawnPowerUp;
    private float startTime = 0;

    private void Start()
    {
        startTime = IngameTimeManager._instance.time;
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

            }
        }

    }

    private void GenerateRandomPowerUp()
    {

    }

    public void ResetPowerUps()
    {
        lastPowerUp = currentPowerUp;
        currentPowerUp = PowerUps.NONE;
        startTime = IngameTimeManager._instance.time;
    }
}
