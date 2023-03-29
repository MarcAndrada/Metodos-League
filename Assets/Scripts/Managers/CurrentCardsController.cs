using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.Progress;

public class CurrentCardsController : MonoBehaviour
{
    public static CurrentCardsController _instance;

    [SerializeField]
    private string[] cardsIDs;

    private Dictionary<string, CardsScrObj> cardsValues;


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

        SetCards();
        //CheckCurentCards();


    }


    private void SetCards()
    {
        foreach (string item in cardsIDs)
        {
            cardsValues.Add(item, Resources.Load("Cards/" + item) as CardsScrObj);
            if (true)
            {

            }

        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
