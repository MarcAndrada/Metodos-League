using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class BallController : MonoBehaviour
{

    [SerializeField]
    private float headButtForce;
    [SerializeField]
    private float kickForce;
    [SerializeField]
    private float superKickForce;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private ParticleSystem[] fireParticles;

    private Vector2 startPos;

    private Rigidbody2D rb2d;
    private AudioClip floorHitSound;
    private AudioSource floorAS;
    private AudioClip playerHitSound;
    private AudioSource playerAS;
    private AudioClip kickHitSound;
    private AudioSource kickAS;
    private AudioClip headHitSound;
    private AudioSource headAS;

    private void Awake()
    {
        floorHitSound = Resources.Load("Sounds/BallFloorHit") as AudioClip;
        playerHitSound = Resources.Load("Sounds/BallPlayerHit") as AudioClip;
        kickHitSound = Resources.Load("Sounds/BallKickHit") as AudioClip;
        headHitSound = Resources.Load("Sounds/BallHeadHit") as AudioClip;
        
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        startPos = transform.position;
        ActivateParticles(false);

    }

    private void FixedUpdate()
    {
        rb2d.angularVelocity = -rb2d.velocity.x * rotationSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Head"))
        {
            Vector2 dir = collision.transform.right;



            float downForce = headButtForce / 3;
            dir = dir.normalized * headButtForce + Vector2.down * downForce;
            rb2d.velocity = dir;
            if (CheckSound(headAS, headHitSound))
            {
                headAS = AudioManager._instance.Play2dOneShotSound(headHitSound, 0.5f, 1.5f, 0.6f);
            }

        }

        if (collision.CompareTag("Feet"))
        {
            Vector2 dir = collision.transform.parent.right;
            float upForce;

            if (PowerUpController._instance.currentPowerUp != PowerUpController.PowerUps.POWER_SHOT)
            {
                upForce = kickForce / 2;
                dir = dir.normalized * kickForce + Vector2.up * upForce;
                if (CheckSound(kickAS, kickHitSound))
                {
                    kickAS = AudioManager._instance.Play2dOneShotSound(kickHitSound, 0.5f, 1.5f, 0.6f);
                }
            }
            else
            {
                upForce = superKickForce / 5;
                //activar las particulas
                ActivateParticles(true);
                dir = dir.normalized * superKickForce + Vector2.up * upForce;
                if(CheckSound(kickAS, kickHitSound))
                {
                    kickAS = AudioManager._instance.Play2dOneShotSound(kickHitSound, 0.5f, 1.5f, 2);
                }


            }

            dir += Vector2.up * upForce;

            rb2d.velocity = dir;

        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Goal") && !IngameTimeManager._instance.endGame)
        {
            IngameTimeManager._instance.StartWaitBeforeScore();

            bool rightPlayer;
            if (collision.transform.position.x > transform.position.x)
            {
                rightPlayer = true;
            }
            else
            {
                rightPlayer = false;
            }
            IngameScoreManger._instance.ScoreGoal(rightPlayer);

            transform.position = new Vector3(startPos.x, -200, transform.position.z);
            rb2d.velocity = Vector2.zero;

            PowerUpController._instance.ResetPowerUps();
            //Desactivar las particulas
           

        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            if (CheckSound(floorAS, floorHitSound))
            {
                floorAS = AudioManager._instance.Play2dOneShotSound(floorHitSound, 0.2f, 1.8f, rb2d.velocity.magnitude / 25);

            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if (CheckSound(playerAS, playerHitSound))
            {
                playerAS = AudioManager._instance.Play2dOneShotSound(playerHitSound, 0.5f, 1.5f, rb2d.velocity.magnitude / 25);
            }
        }

        ActivateParticles(false);
    }


    private void ActivateParticles(bool _activate) 
    {
        foreach (ParticleSystem item in fireParticles)
        {
            if (_activate)
            {
                item.Play();
            }
            else
            {
                item.Stop();
            }
        }
    }
    private bool CheckSound(AudioSource _as, AudioClip _clip)
    {
        if (_as == null || _as != null && !_as.isPlaying || _as != null && _as.clip != _clip)
        {
            return true;
        }

        return false;
    }
}
