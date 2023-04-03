using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardViewerManager : MonoBehaviour
{
    [Header("Prefabs"), SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private GameObject lockedCardPrefab;

    [Space, Header("Variables"), SerializeField]
    private float cardXOffset;
    [SerializeField]
    private float cardYOffset;

    [SerializeField]
    private List<List<GameObject>> cards;


    private Vector3 middleScreenPos;

    private int currentRow = 0;
    private int currentColumn = 0;

    private int totalRows = 3;
    private int totalColumns;

    private void Awake()
    {
        Camera cam = FindObjectOfType<Camera>();
        middleScreenPos = cam.transform.position;
        middleScreenPos += new Vector3(-cardXOffset, cardYOffset);
        InitializeCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeCards()
    {
        cards = new List<List<GameObject>>();
        List<CardsScrObj> allCards = CurrentCardsController._instance.GetAllCards();
        float totalCards = allCards.Count;
        totalColumns = (int)System.Math.Ceiling(totalCards / 3);

        int createdCards = 0;
        for (int i = 0; i < totalColumns; i++)
        {
            cards.Add(new List<GameObject>());
            for (int j = 0; j < totalRows; j++)
            {
                if (createdCards < totalCards)
                {
                    cards[i].Add(Instantiate(cardPrefab));
                    createdCards++;
                }
            }
        }

    }

    private void LocateCards() 
    {
    
    }





}
