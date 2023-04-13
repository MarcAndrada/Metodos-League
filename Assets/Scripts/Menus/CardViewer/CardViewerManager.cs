using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardViewerManager : MonoBehaviour
{
    private List<List<GameObject>> cards;
    private List<Vector3> nextPos;
    private List<Vector3> currentPos;

    [Header("Prefabs"), SerializeField]
    private GameObject cardPrefab;


    [Space, Header("Variables"), SerializeField]
    private float cardSpeed;
    [SerializeField]
    private float cardXOffset;
    [SerializeField]
    private float cardYOffset;

    private Vector3 middleScreenPos;

    private int currentRow = 0;

    private int totalRows;
    private int totalColumns = 3;

    private float lerpProcess = 0;

    private bool detectInputs = true;

    private void Awake()
    {
        cards = new List<List<GameObject>>();
        currentPos = new List<Vector3>();
        nextPos = new List<Vector3>();

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
        if (detectInputs)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                GoDown();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                GoUp();
            }
        }
        else
        {
            LerpCards();
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
                    cardView.lockObj.SetActive(!allCards[createdCards].obtainedCard);
                    createdCards++;

                    int cardsLength = cards[i].Count - 1;
                    nextPos.Add(cards[i][cardsLength].transform.position);
                    currentPos.Add(cards[i][cardsLength].transform.position);
                }
            }
        }
    }


    private void LerpCards()
    {
        lerpProcess += Time.deltaTime * cardSpeed;
        int totalCardsIndex = 0;
        for (int i = 0; i < cards.Count; i++)
        {
            for (int j = 0; j < cards[i].Count; j++)
            {
                cards[i][j].transform.position = Vector3.Lerp(currentPos[totalCardsIndex], nextPos[totalCardsIndex], lerpProcess);
                totalCardsIndex++;
            }
        }

        if (lerpProcess >= 1)
        {
            detectInputs = true;
            lerpProcess = 0;
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
        detectInputs = false;
        int totalCardsIndex = 0;
        for (int i = 0; i < totalRows; i++)
        {
            for (int j = 0; j < totalColumns; j++)
            {
                if (j < cards[i].Count)
                {
                    currentPos[totalCardsIndex] = cards[i][j].transform.position;
                    nextPos[totalCardsIndex] = middleScreenPos + new Vector3(cardXOffset * j, -cardYOffset * (i - currentRow));
                    totalCardsIndex++;
                }
            }
        }



    }


    public void GoUp()
    {
        if (detectInputs)
        {
            //Go Up
            if (CheckIfCanGoUp())
            {
                currentRow--;
                LocateCards();
            }
        }
    }

    public void GoDown()
    {
        if (detectInputs)
        {
            //Go Down
            if (CheckIfCanGoDown())
            {
                currentRow++;
                LocateCards();
            }
        }
    }


}
