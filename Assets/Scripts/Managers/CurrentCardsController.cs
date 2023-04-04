using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentCardsController : MonoBehaviour
{
    public static CurrentCardsController _instance;

    [SerializeField]
    private string[] cardsIDs;

    private Dictionary<string, CardsScrObj> cardsValues;

    [HideInInspector]
    public string userPlayerSelected;
    [HideInInspector]
    public string enemyPlayerSelected;


    private void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(_instance.gameObject);
            }
        }
        _instance = this;

        DontDestroyOnLoad(gameObject);

        cardsValues = new Dictionary<string, CardsScrObj>();

        SetCards();

        SceneManager.LoadScene("MainMenu");

    }


    private void SetCards()
    {
        foreach (string item in cardsIDs)
        {
            cardsValues.Add(item, Resources.Load("Cards/" + item) as CardsScrObj);
        }
    }


    public List<CardsScrObj> GetOwnedCards()
    {
        List<CardsScrObj> result = new List<CardsScrObj>();

        foreach (string item in cardsIDs) 
        {
            if (cardsValues[item].obtainedCard)
            {
                result.Add(cardsValues[item]);
            }
        }

        return result;

    }
    public List<CardsScrObj> GetAllCards()
    {
        List<CardsScrObj> result = new List<CardsScrObj>();

        foreach (string item in cardsIDs)
        {
            result.Add(cardsValues[item]);

        }

        return result;
    }

    public CardsScrObj GetOneCard(string _cardID)
    {
        return cardsValues[_cardID];
    }
    public CardsScrObj GetOneCardByID(int _id)
    {
        return cardsValues[cardsIDs[_id]];

    }
    public int GetAllCardsLenght()
    {
        return cardsValues.Count;
    }
    public bool AllCardsObtained()
    {
        foreach (string item in cardsIDs)
        {
            if (cardsValues[item].obtainedCard)
            {
                return false;
            }
        }

        return true;
    }



    public void SelectedPlayerByUser(string _playerName)
    {
        userPlayerSelected = _playerName;

    }

    public void SelectPlayerEnemy(string _enemyName)
    {
        enemyPlayerSelected = _enemyName;
    }

}
