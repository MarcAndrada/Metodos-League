using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] cards;

    [SerializeField]
    private float xCardOffset;
    [SerializeField]
    private float zCardOffset;
    [SerializeField]
    private float cardSizeOffset;
    [SerializeField]
    private float cardDistanceReduction;

    private int currentCardIndex;

    private Vector2 starterSize;
    private Vector3 startPos;


    // Start is called before the first frame update
    void Start()
    {
        Camera cam = FindObjectOfType<Camera>();
        startPos = cam.transform.position;
        starterSize = cards[0].transform.localScale;
        startPos.z = 0;
        RecursiveMoveCards();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentCardIndex++;
            currentCardIndex %= cards.Length;
            RecursiveMoveCards();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentCardIndex--;
            currentCardIndex %= cards.Length;
            if (currentCardIndex < 0)
            {
                currentCardIndex = cards.Length - 1;
            }
            RecursiveMoveCards();

        }

    }


    private void RecursiveMoveCards()
    {
        RecursiveMoveCardsLeft(currentCardIndex, 0, starterSize, xCardOffset);
        RecursiveMoveCardsRight(currentCardIndex, 0, starterSize, xCardOffset);
    }

    private void RecursiveMoveCardsLeft(int _cardIDToCheck, int _cardsPassed, Vector2 _cardSize, float _cardsXOffset)
    {
        cards[_cardIDToCheck].transform.position = startPos + new Vector3(-_cardsXOffset * _cardsPassed, 0, zCardOffset * _cardsPassed);
        cards[_cardIDToCheck].transform.localScale = _cardSize;
        if (_cardIDToCheck != 0)
        {
            RecursiveMoveCardsLeft(--_cardIDToCheck, ++_cardsPassed, _cardSize / cardSizeOffset, _cardsXOffset / cardDistanceReduction);
        }


    }
    private void RecursiveMoveCardsRight(int _cardIDToCheck, int _cardsPassed, Vector2 _cardSize, float _cardsXOffset)
    {
        cards[_cardIDToCheck].transform.position = startPos + new Vector3(_cardsXOffset * _cardsPassed, 0, zCardOffset * _cardsPassed);
        cards[_cardIDToCheck].transform.localScale = _cardSize;
        if (_cardIDToCheck != cards.Length - 1)
        {
            RecursiveMoveCardsRight(++_cardIDToCheck, ++_cardsPassed, _cardSize / cardSizeOffset, _cardsXOffset / cardDistanceReduction);
        }


    }

}
