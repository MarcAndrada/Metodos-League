using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void LoadPlayerSelectorScene()
    {
        SceneManager.LoadScene("PlayerCharacterSelectorScene");
    }

    public void LoadEnemySelectorScene()
    {
        SceneManager.LoadScene("EnemyCharacterSelectorScene");
    }


    public void LoadCardsScene()
    {
        SceneManager.LoadScene("CardViewerScene");
    }

    public void LoadIngameScene()
    {
        SceneManager.LoadScene("IngameScene");
    }

    public void GoMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
