using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FootballPlayerController : MonoBehaviour
{
    [Header("Movement Var")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;
    private float movementInput;

    private bool canJump = true;
    private bool isGrounded = false;

    [Header("Raycast Var")]
    [SerializeField]
    private float checkFloorRange;
    [SerializeField]
    private LayerMask floorLayer;

    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsuleCollider;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }



    // Update is called once per frame
    void Update()
    {
        movementInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && canJump)
        {
            Jump();
        }
    }


    private void FixedUpdate()
    {
        rb2d.AddForce(Vector2.right * speed * movementInput * Time.fixedDeltaTime, ForceMode2D.Force);
    }

    private void Jump()
    {
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        canJump = false;
    }

    private void CheckIfGrounded()
    {
        bool _actuallyGrounded = false;
        //Variable para poner el punto de aparicion de los raycast, asi queda mas limpio el codigo
        Vector3 spawnPosRay;
        //Creamos una variable para el offset hacia arriba debido a que sin el hay problemas con la deteccion del suelo cuando esta muy cerca
        Vector3 posOffset = Vector3.up * checkFloorRange / 2;

        //Hacemos un raycast a los pies del player desde el punto central
        spawnPosRay = transform.position - new Vector3(0, capsuleCollider.size.y / 2) + posOffset;
        RaycastHit2D hit = DoRaycast(spawnPosRay, Vector2.down, checkFloorRange, floorLayer);
        if (hit)
        {
            _actuallyGrounded = true;
        }
        else
        {
            //Si no choca lo lanzamos de uno de los lados (en este caso el de la izquierda)
            spawnPosRay = transform.position - new Vector3(-capsuleCollider.size.x / 2, capsuleCollider.size.y / 2) + posOffset;
            hit = DoRaycast(spawnPosRay, Vector2.down, checkFloorRange, floorLayer);
            if (hit)
            {
                _actuallyGrounded = true;
            }
            else
            {
                //Y para acabar si no ha detectado suelo en la izquierda lo lanzaremos a la derecha
                spawnPosRay = transform.position - new Vector3(capsuleCollider.size.x / 2, capsuleCollider.size.y / 2) + posOffset;
                hit = DoRaycast(spawnPosRay, Vector2.down, checkFloorRange, floorLayer);

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

    private RaycastHit2D DoRaycast(Vector2 _pos, Vector2 _dir, float _distance, LayerMask _layer)
    {
        // Esta funcion es para simplificar el hacer un raycast que da un palo que flipas loquete ;)
        RaycastHit2D[] _hit = new RaycastHit2D[1];
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(_layer);

        Physics2D.Raycast(_pos, _dir, filter, _hit, _distance);

        return _hit[0];
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            CheckIfGrounded();
        }
    }



}
