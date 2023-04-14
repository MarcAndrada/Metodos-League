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

    private Vector2 startPos;

    private Rigidbody2D rb2d;

    private void Awake()
    {

        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        startPos = transform.position;
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
        }

        if (collision.CompareTag("Feet"))
        {
            Vector2 dir = collision.transform.parent.right;
            float upForce;

            if (PowerUpController._instance.currentPowerUp != PowerUpController.PowerUps.POWER_SHOT)
            {
                upForce = kickForce / 2;
            }
            else
            {
                dir *= superKickForce;
                upForce = kickForce / 5;
                //activar las particulas
            }

            dir = dir.normalized * kickForce + Vector2.up * upForce;

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
    }
}
