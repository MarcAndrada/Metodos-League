using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CardManager : MonoBehaviour
{
    public enum UserType { PLAYER, ENEMY};
    public UserType userType = UserType.PLAYER;

    [SerializeField]
    private GameObject cardPrefab;
    [Space]
    private List<GameObject> cards;
    private List<Vector3> nextPos;
    private List<Vector3> currentPos;
    private List<Vector2> nextSize;
    private List<Vector2> currentSize;

    private float lerpProcess = 0;
    [Space, SerializeField]
    private float cardSpeed;
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

    

    private bool detectInputs = true;

    private void Awake()
    {
        cards = new List<GameObject>();
        currentPos = new List<Vector3>();
        currentSize = new List<Vector2>();
        nextPos = new List<Vector3>();
        nextSize = new List<Vector2>();

        foreach (var item in CurrentCardsController._instance.GetAllCards())
        {
            if (userType == UserType.PLAYER && item.obtainedCard ||
                userType == UserType.ENEMY && item.name != CurrentCardsController._instance.userPlayerSelected)
            {
                cards.Add(Instantiate(cardPrefab));
                cards[cards.Count - 1].GetComponentInChildren<SpriteRenderer>().sprite = item.cardSprite;
                cards[cards.Count - 1].name = item.cardName;
                nextPos.Add(cards[cards.Count - 1].transform.position);
                currentPos.Add(cards[cards.Count - 1].transform.position);
                nextSize.Add(cards[cards.Count - 1].transform.localScale);
                currentSize.Add(cards[cards.Count - 1].transform.localScale);
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
        if (detectInputs)
        {
            ReadInputs();
        }
        else
        {
            LerpCards();
        }
        

    }

    private void ReadInputs()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            MoveCardRight();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            MoveCardLeft();
        }
    }

    private void LerpCards()
    {
        lerpProcess += Time.deltaTime * cardSpeed;

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.position = Vector3.Lerp(currentPos[i], nextPos[i], lerpProcess);
            cards[i].transform.localScale = Vector3.Lerp(currentSize[i], nextSize[i], lerpProcess);
        }

        if (lerpProcess >= 1)
        {
            detectInputs = true;
            lerpProcess = 0;
        }

    }



    private void RecursiveMoveCards()
    {
        detectInputs = false;
        lerpProcess = 0;
        RecursiveMoveCardsLeft(currentCardIndex, 0, starterSize, xCardOffset);
        RecursiveMoveCardsRight(currentCardIndex, 0, starterSize, xCardOffset);
    }

    private void RecursiveMoveCardsLeft(int _cardIDToCheck, int _cardsPassed, Vector2 _cardSize, float _cardsXOffset)
    {
        currentPos[_cardIDToCheck] = cards[_cardIDToCheck].transform.position;
        currentSize[_cardIDToCheck] = cards[_cardIDToCheck].transform.localScale;

        nextPos[_cardIDToCheck] = startPos + new Vector3(-_cardsXOffset * _cardsPassed, 0, zCardOffset * _cardsPassed);
        nextSize[_cardIDToCheck] = _cardSize;
        if (_cardIDToCheck != 0)
        {
            RecursiveMoveCardsLeft(--_cardIDToCheck, ++_cardsPassed, _cardSize / cardSizeOffset, _cardsXOffset / cardDistanceReduction);
        }


    }
    private void RecursiveMoveCardsRight(int _cardIDToCheck, int _cardsPassed, Vector2 _cardSize, float _cardsXOffset)
    {
        currentPos[_cardIDToCheck] = cards[_cardIDToCheck].transform.position;
        currentSize[_cardIDToCheck] = cards[_cardIDToCheck].transform.localScale;

        nextPos[_cardIDToCheck] = startPos + new Vector3(_cardsXOffset * _cardsPassed, 0, zCardOffset * _cardsPassed);
        nextSize[_cardIDToCheck] = _cardSize;
        if (_cardIDToCheck != cards.Count - 1)
        {
            RecursiveMoveCardsRight(++_cardIDToCheck, ++_cardsPassed, _cardSize / cardSizeOffset, _cardsXOffset / cardDistanceReduction);
        }


    }


    public void MoveCardLeft()
    {
        if (detectInputs)
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

    public void MoveCardRight()
    {
        currentCardIndex++;
        currentCardIndex %= cards.Count;
        RecursiveMoveCards();
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
