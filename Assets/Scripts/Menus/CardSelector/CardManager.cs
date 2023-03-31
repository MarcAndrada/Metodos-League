using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public enum UserType { PLAYER, ENEMY};
    public UserType userType = UserType.PLAYER;

    [SerializeField]
    private GameObject cardPrefab;
    [Space]
    [SerializeField]
    private List<GameObject> cards;
    
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


    private void Awake()
    {

        foreach (var item in CurrentCardsController._instance.GetAllCards())
        {
            if (userType == UserType.PLAYER && item.obtainedCard ||
                userType == UserType.ENEMY && item.name != CurrentCardsController._instance.userPlayerSelected)
            {
                cards.Add(Instantiate(cardPrefab));
                cards[cards.Count - 1].GetComponentInChildren<SpriteRenderer>().sprite = item.cardSprite;
                cards[cards.Count - 1].name = item.cardName;
            }
        }

    }

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
            currentCardIndex %= cards.Count;
            RecursiveMoveCards();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentCardIndex--;
            currentCardIndex %= cards.Count;
            if (currentCardIndex < 0)
            {
                currentCardIndex = cards.Count - 1;
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
        if (_cardIDToCheck != cards.Count - 1)
        {
            RecursiveMoveCardsRight(++_cardIDToCheck, ++_cardsPassed, _cardSize / cardSizeOffset, _cardsXOffset / cardDistanceReduction);
        }


    }


    public void CurrentCardSelected()
    {
        if (userType == UserType.PLAYER)
        {
            CurrentCardsController._instance.userPlayerSelected = cards[currentCardIndex].name;
        }
        else
        {
            CurrentCardsController._instance.enemyPlayerSelected = cards[currentCardIndex].name;
        }
    }


}
