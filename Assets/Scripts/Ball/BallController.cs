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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Head"))
        {
            Vector2 dir = transform.position - collision.transform.parent.position;
            if (dir.normalized.x < 0.2 && dir.normalized.x > -0.2)
            {
                dir = dir.normalized;
                dir.x = collision.transform.right.x * 0.3f;
            }
            float downForce;
            if (dir.normalized.y < -0.3)
            {
                downForce = headButtForce / 4;
            }
            else if (dir.normalized.y < - 0.1)
            {
                downForce = headButtForce / 2;
            }
            else
            {
                downForce = headButtForce;
            }
            dir = dir.normalized * headButtForce + Vector2.down * downForce;
            rb2d.velocity = dir;
        }

        if (collision.CompareTag("Feet"))
        {
            Vector2 dir = transform.position - collision.transform.parent.position;

            if (dir.normalized.x < 0.2 && dir.normalized.x > -0.2)
            {
                dir = dir.normalized;
                dir.x = collision.transform.right.x * 0.3f;
            }

            float upForce;
            if (dir.normalized.y > 0.3)
            {
                upForce = kickForce / 4;
            }
            else if (dir.normalized.y > 0.1)
            {
                upForce = kickForce / 2;
            }
            else
            {
                upForce = kickForce;
            }
            dir = dir.normalized * kickForce + Vector2.up * upForce;
            rb2d.velocity = dir;

        }

        if (collision.CompareTag("Goal"))
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
        }

    }
}
