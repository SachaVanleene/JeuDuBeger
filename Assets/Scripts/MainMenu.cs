using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour {

    public GameObject loadingScreen;
    public Text loadingText;
    public GameObject difficultyPanel;

    private void Start()
    {
    }


    //Choose difficulty of game
    public void PlayWithDifficulty(int difficulty)
    {
        loadingScreen.SetActive(true);
        switch (difficulty)
        {
            case 1:
                //Load scene with easy enemies
                break;
            case 2:
                //Load scene with medium enemies
                break;
            case 3:
                //Load scene with hard enemies
                break;
        }


        SceneManager.LoadScene(1);
    }

    //On play, show panel to choose difficulty
    public void ShowDifficultyPanel()
    {
        difficultyPanel.SetActive(true);
    }

    public void HideDifficultyPanel()
    {
        difficultyPanel.SetActive(false);
    }

    //Exit game
    public void ExitGame()
    {
        Application.Quit();
    }
}
