using System;
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

    private int totalRows;
    private int totalColumns = 3;

    private void Awake()
    {
        Camera cam = FindObjectOfType<Camera>();
        middleScreenPos = cam.transform.position;
        middleScreenPos += new Vector3(-cardXOffset, cardYOffset);
        middleScreenPos.z = 0;
        InitializeCards();
        LocateCards();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxisRaw("Mouse ScrollWheel") == -1)
        {
            //Go Down
            if (CheckIfCanGoDown())
            {
                currentRow++;
                LocateCards();
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxisRaw("Mouse ScrollWheel") == 1)
        {
            //Go Up
            if (CheckIfCanGoUp())
            {
                currentRow--;
                LocateCards();
            }
        }
    }

    private void InitializeCards()
    {
        cards = new List<List<GameObject>>();
        List<CardsScrObj> allCards = CurrentCardsController._instance.GetAllCards();
        float totalCards = allCards.Count;
        totalRows = (int)System.Math.Ceiling(totalCards / 3);

        int createdCards = 0;
        for (int i = 0; i < totalRows; i++)
        {
            cards.Add(new List<GameObject>());
            for (int j = 0; j < totalColumns; j++)
            {
                if (createdCards < totalCards)
                {
                    cards[i].Add(Instantiate(cardPrefab));
                    cards[i][j].name = allCards[createdCards].cardName;
                    CardViewController cardView = cards[i][j].GetComponent<CardViewController>();
                    cardView.cardSR.sprite = allCards[createdCards].cardSprite;
                    cardView.lockSR.enabled = !allCards[createdCards].obtainedCard;
                    createdCards++;
                }
            }
        }
    }

    private bool CheckIfCanGoUp()
    {
        if (currentRow - 1 >= 0)
        {
            return true;
        }

        return false;
    }
    private bool CheckIfCanGoDown()
    {
        if (currentRow + 4 <= totalRows)
        {
            return true;
        }

        return false;
    }

    private void LocateCards() 
    {
        for (int i = 0; i < totalRows; i++)
        {
            for (int j = 0; j < totalColumns; j++)
            {
                if (j < cards[i].Count)
                {
                    if (i >= currentRow && i <= currentRow + 2)
                    {
                        cards[i][j].transform.position = middleScreenPos + new Vector3(cardXOffset * j, -cardYOffset * (i - currentRow));

                    }
                    else
                    {
                        cards[i][j].transform.position = new Vector3(100, 1000);
                    }
                }
            }
        }



    }





}
