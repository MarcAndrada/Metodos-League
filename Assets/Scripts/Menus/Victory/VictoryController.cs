using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class VictoryController : MonoBehaviour
{
    [SerializeField]
    private Image cardImage;
    [SerializeField]
    private TextMeshProUGUI obtainedCardText;
    [SerializeField]
    private Button button;

    private Animator animator;

    private CardsScrObj generatedCard;

    private void Awake()
    {
        GenerateRandomCard();
        if (generatedCard)
        {
            cardImage.sprite = generatedCard.cardSprite;
            obtainedCardText.text = "Pulsa cualquier tecla";
        }
        cardImage.enabled = false;
        button.gameObject.SetActive(false);
        animator = FindObjectOfType<Animator>();    


    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            ShowCard();
        }
    }

    private void GenerateRandomCard()
    {
        int randomID = Random.Range(0, CurrentCardsController._instance.GetAllCardsLenght());
        generatedCard = CurrentCardsController._instance.GetOneCardByID(randomID);
        if (generatedCard.obtainedCard)
        {
            if (!CurrentCardsController._instance.AllCardsObtained())
            {
                GenerateRandomCard();
            }
            else
            {
                //Hacer algo mas
                generatedCard = null;
            }
        }
    }

    private void ShowCard()
    {
        cardImage.enabled = true;
        obtainedCardText.text = "¡¡¡Has desbloqueado a " + generatedCard.cardName + "!!!";
        button.gameObject.SetActive(true);

        animator.SetTrigger("Open");
        generatedCard.obtainedCard = true;

    }

}
