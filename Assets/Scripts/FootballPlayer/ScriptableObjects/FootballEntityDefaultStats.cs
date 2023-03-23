using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class FootballEntityDefaultStats : ScriptableObject
{
    [Header("Movement Var")]
    public float speed;
    public float maxSpeed;
    public float jumpForce;

    [Header("Raycast Var")]
    public float checkFloorRange;
    public LayerMask floorLayer;
}
