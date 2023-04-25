using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PowerUpController : MonoBehaviour
{
    public static PowerUpController _instance;


    public enum PowerUps { NONE = 0, BULLET_TIME, POWER_SHOT, INVERTED_INPUT, SPRING_FEET, ZERO_GRAVITY };
    public PowerUps currentPowerUp = PowerUps.NONE;
    public PowerUps lastPowerUp = PowerUps.NONE;

    [SerializeField]
    private float timeToSpawnPowerUp;
    private float startTime = 0;

    [Header("SlowDown Power Up"), SerializeField]
    private float slowdownMaxValue;
    [SerializeField]
    private float slowdownMinValue;
    [SerializeField]
    private float changeRythmSpeed;

    private float currentSlowdown;

    [Header("Zero Gravity Power Up"), SerializeField]
    private float zeroGravityValue;
    private float starterGravity;

    

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
        startTime = IngameTimeManager._instance.time;
        currentSlowdown = slowdownMaxValue;
        starterGravity = Physics2D.gravity.y;
        FootballFieldController._instance.SetFireParticles(false);

    }

    private void Update()
    {
        CheckIfCanApplyPowerUp();

        PowerUpActions();

        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangePowerUp(PowerUps.BULLET_TIME);
        }else if (Input.GetKeyDown(KeyCode.O))
        {
            ChangePowerUp(PowerUps.SPRING_FEET);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            ChangePowerUp(PowerUps.POWER_SHOT);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            ChangePowerUp(PowerUps.ZERO_GRAVITY);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            ChangePowerUp(PowerUps.INVERTED_INPUT);
        }
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
        currentPowerUp = _nextPowerUp;
        lastPowerUp = _nextPowerUp;
        switch (_nextPowerUp)
        {
            case PowerUps.BULLET_TIME:
                //relentizar el tiempo
                Time.timeScale = currentSlowdown;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                break;
            case PowerUps.POWER_SHOT:
                //Cambiar algo en la bola para que al tocar con un collider salga disparada y poner particulas en los pies de los Jugadores 
                FootballFieldController._instance.SetFireParticles(true);
                break;
            case PowerUps.INVERTED_INPUT:
                //Cambiar el input de los Jugadores
                FootballFieldController._instance.SetMoveDir(-1);
                break;
            case PowerUps.SPRING_FEET:
                //Actvar algo en los jugadores para que cada vez que toquen el suelo salten

                break;
            case PowerUps.ZERO_GRAVITY:
                //Cambiar la gravedad para que las entidades tengan muy poca gravedad
                Physics2D.gravity = new Vector2(0, -zeroGravityValue);
                break;
            default:
                break;
        }
    }

    private void PowerUpActions()
    {
        switch (currentPowerUp)
        {
            case PowerUps.BULLET_TIME:
                currentSlowdown += changeRythmSpeed * Time.deltaTime;

                if (currentSlowdown >= slowdownMaxValue || currentSlowdown <= slowdownMinValue)
                {
                    changeRythmSpeed *= -1;

                    currentSlowdown = Mathf.Clamp(currentSlowdown, slowdownMinValue, slowdownMaxValue);
                }

                Time.timeScale = currentSlowdown;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;

                break;
            case PowerUps.POWER_SHOT:
                break;
            case PowerUps.INVERTED_INPUT:
                break;
            case PowerUps.SPRING_FEET:
                break;
            case PowerUps.ZERO_GRAVITY:
                break;
            default:
                break;
        }
    }

    public void ResetPowerUps()
    {
        currentPowerUp = PowerUps.NONE;
        startTime = IngameTimeManager._instance.time;
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        FootballFieldController._instance.SetMoveDir(1);
        FootballFieldController._instance.SetFireParticles(false);
        Physics2D.gravity = new Vector2(0, starterGravity);
        FootballFieldController._instance.SetFireParticles(false);

    }
}
