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

    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();    
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Head"))
        {
            Vector2 dir = transform.position - collision.transform.parent.position;
            dir = dir.normalized * headButtForce + Vector2.down * headButtForce;
            rb2d.velocity = dir;
            rb2d.AddForce(dir.normalized, ForceMode2D.Impulse);
            Debug.Log("Toca la cabeza");
        }

        if (collision.CompareTag("Feet"))
        {
            Vector2 dir = transform.position - collision.transform.parent.position;
            dir = dir.normalized * kickForce + Vector2.up * kickForce;
            rb2d.velocity = dir;
            rb2d.AddForce(dir.normalized, ForceMode2D.Impulse);
            Debug.Log("Toca el pie");

        }
    }
}
