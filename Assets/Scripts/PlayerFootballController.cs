using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootballController : FootballEntityController
{

    private float movementInput;

    // Update is called once per frame
    void Update()
    {
        movementInput = Input.GetAxisRaw("Horizontal");
        MoveFootballPlayer(movementInput);
        if (Input.GetButtonDown("Jump") && canJump)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Kick();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            HeadButt();
        }

    }
}
