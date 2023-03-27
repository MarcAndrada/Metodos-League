using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("CharacterSelectorScene");
    }

    public void LoadCardsScene()
    {
    }
    
    public void LoadIngameScene()
    {
        SceneManager.LoadScene("IngameScene");
    }
}
