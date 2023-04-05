using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Card", order = 1)]
public class CardsScrObj : ScriptableObject
{
    public bool obtainedCard;
    public string cardName;
    public Sprite cardSprite;
    public Sprite ingameFaceSprite;

    public void Initialize()
    {
        if (PlayerPrefs.HasKey(cardName))
        {
            if (PlayerPrefs.GetString(cardName) == "True")
            {
                obtainedCard = true;
            }
            else
            {
                obtainedCard = false;
            }
        }
        else
        {
            ChangeObtainedValue(obtainedCard);
        }
    }


    public void ChangeObtainedValue(bool _obtained)
    {
        obtainedCard = _obtained;
        PlayerPrefs.SetString(cardName, obtainedCard.ToString());
        PlayerPrefs.Save();
    }

}
