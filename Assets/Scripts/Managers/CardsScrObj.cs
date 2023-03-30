using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Card", order = 1)]
public class CardsScrObj : ScriptableObject
{
    public bool obtainedCard;
    public string cardName;
    public Sprite cardSprite;
    public Sprite ingameFaceSprite;

}
