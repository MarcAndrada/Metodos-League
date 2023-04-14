using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FootballEntityController : MonoBehaviour
{
    public enum EtityType { PLAYER, AI };
    public EtityType type;

    public FootballEntityDefaultStats footballEntityValues;

    protected bool canJump = true;
    protected bool isGrounded = false;

    private Rigidbody2D rb2d;
    private Animator animator;
    protected EntityComponentManager collisionsManager;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collisionsManager = GetComponent<EntityComponentManager>();
    }

    public void Start()
    {
        string currentPlayer;
        if (type == EtityType.PLAYER)
        {
            currentPlayer = CurrentCardsController._instance.userPlayerSelected;
        }
        else
        {
            currentPlayer = CurrentCardsController._instance.enemyPlayerSelected;
        }
        collisionsManager.headSR.sprite = CurrentCardsController._instance.GetOneCard(currentPlayer).ingameFaceSprite;

    }

    protected void MoveFootballPlayer(float _movementDir)
    {
        rb2d.AddForce(Vector2.right * footballEntityValues.speed * _movementDir * Time.deltaTime, ForceMode2D.Force);
        rb2d.velocity = new Vector2(Mathf.Clamp(rb2d.velocity.x, -footballEntityValues.maxSpeed, footballEntityValues.maxSpeed), rb2d.velocity.y);

    }

    protected void Jump()
    {
        rb2d.AddForce(Vector2.up * footballEntityValues.jumpForce, ForceMode2D.Impulse);
        canJump = false;
    }

    protected void Kick()
    {
        animator.SetTrigger("Kick");
    }

    protected void HeadButt()
    {
        animator.SetTrigger("HeadButt");
    }

    protected void CheckIfGrounded()
    {
        bool _actuallyGrounded = false;
        //Variable para poner el punto de aparicion de los raycast, asi queda mas limpio el codigo
        Vector3 spawnPosRay;
        //Creamos una variable para el offset hacia arriba debido a que sin el hay problemas con la deteccion del suelo cuando esta muy cerca
        Vector3 posOffset = Vector3.up * footballEntityValues.checkFloorRange / 2;

        //Hacemos un raycast a los pies del player desde el punto central
        spawnPosRay = transform.position - new Vector3(0, collisionsManager.physicalCollision.size.y / 2) + posOffset;
        RaycastHit2D hit = DoRaycast(spawnPosRay, Vector2.down, footballEntityValues.checkFloorRange, footballEntityValues.floorLayer);
        if (hit)
        {
            _actuallyGrounded = true;
        }
        else
        {
            //Si no choca lo lanzamos de uno de los lados (en este caso el de la izquierda)
            spawnPosRay = transform.position - new Vector3(-collisionsManager.physicalCollision.size.x / 2, collisionsManager.physicalCollision.size.y / 2) + posOffset;
            hit = DoRaycast(spawnPosRay, Vector2.down, footballEntityValues.checkFloorRange, footballEntityValues.floorLayer);
            if (hit)
            {
                _actuallyGrounded = true;
            }
            else
            {
                //Y para acabar si no ha detectado suelo en la izquierda lo lanzaremos a la derecha
                spawnPosRay = transform.position - new Vector3(collisionsManager.physicalCollision.size.x / 2, collisionsManager.physicalCollision.size.y / 2) + posOffset;
                hit = DoRaycast(spawnPosRay, Vector2.down, footballEntityValues.checkFloorRange, footballEntityValues.floorLayer);

                if (hit)
                {
                    _actuallyGrounded = true;
                }

            }
        }


        isGrounded = _actuallyGrounded;

        if (_actuallyGrounded)
        {
            canJump = true;
        }
    
}

    protected RaycastHit2D DoRaycast(Vector2 _pos, Vector2 _dir, float _distance, LayerMask _layer)
    {
        // Esta funcion es para simplificar el hacer un raycast que da un palo que flipas loquete ;)
        RaycastHit2D[] _hit = new RaycastHit2D[1];
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(_layer);

        Physics2D.Raycast(_pos, _dir, filter, _hit, _distance);

        return _hit[0];
    }


    public void EnableHeadColl()
    {
        collisionsManager.headCollision.enabled = true;
    }
    public void DisableHeadColl()
    {
        collisionsManager.headCollision.enabled = false;
    }
    public void EnableFeetColl()
    {
        collisionsManager.feetCollision.enabled = true;
    }
    public void DisableFeetColl()
    {
        collisionsManager.feetCollision.enabled = false;
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            CheckIfGrounded();
        }
    }



}
