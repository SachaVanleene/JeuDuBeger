using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour {

    public GameObject LoadingScreen;
    public GameObject DifficultyPanel;
    public GameObject AchievementsPanel;
    public GameObject AchievementInfoPanel;
    public GameObject LogPanel;
    public GameObject LogButton;
    public GameObject DeleteButton;
    public GameObject ExitGameButton;
    public GameObject ExitAchievementsButton;
    public Text LoadingText;
    public Text PlayerNameText;
    public InputField NewPlayerName;


    public void Start()
    {
        // hide the panel to select profiles when we come back to this scene (from the scene of the game)
        if (SProfilePlayer.getInstance().Name.Equals("<Default>"))
            LogPanel.SetActive(true);
        else
        {
            PlayerNameText.text = "Hello " + SProfilePlayer.getInstance().Name;
            LogButton.SetActive(true);
            DeleteButton.SetActive(true);

        }
    }

    //Choose difficulty of game
    public void PlayWithDifficulty(int difficulty)
    {
        HideAllPanels();
        LogButton.SetActive(false);
        DeleteButton.SetActive(false);
        LoadingScreen.SetActive(true);
        SProfilePlayer.getInstance().Difficulty = difficulty; // to be use in game
        SceneManager.LoadScene(1);
    }

    //On play, show panel to choose difficulty
    public void ShowDifficultyPanel()
    {
        HideAllPanels();
        ExitGameButton.SetActive(false);
        DifficultyPanel.SetActive(true);
    }

    public void HideDifficultyPanel()
    {
        ExitGameButton.SetActive(true);
        DifficultyPanel.SetActive(false);
    }
    public void DeleteCurrentProfile()
    {
        ProfileManager.DeleteProfile(SProfilePlayer.getInstance().Name);
        ShowLogPanel();
    }
    public void ChangeCurrentProfile()
    {
        ProfileManager.SaveProfile();   
        ShowLogPanel();
    }
    public void CreateProfile()
    {
        if (NewPlayerName.text.Equals(""))
            GetComponent<ProfileManager>().CreateProfile("UnknownPlayer");
        else
            GetComponent<ProfileManager>().CreateProfile(NewPlayerName.text);
        NewPlayerName.text = "";
        HideLogPanel();
    }
    private void ShowLogPanel()
    {
        HideAllPanels();
        ProfileManager.RetreiveSaves();
        LogPanel.SetActive(true);
        ExitGameButton.SetActive(true);
        LogPanel.GetComponent<ListBehaviour>().CreateListPanel();
        LogButton.SetActive(false);
        DeleteButton.SetActive(false);
    }
    public void HideLogPanel()
    {
        PlayerNameText.text = "Hello " + SProfilePlayer.getInstance().Name;
        LogButton.SetActive(true);
        DeleteButton.SetActive(true);
        LogPanel.SetActive(false);
    }
    public void ShowAchievementsPanel()
    {
        HideAllPanels();
        ExitAchievementsButton.SetActive(true);
        ExitGameButton.SetActive(false);
        AchievementsPanel.GetComponent<ListBehaviourAchievements>().CreateListPanel();
        AchievementsPanel.SetActive(true);
    }
    public void HideAchievementsPanel()
    {
        AchievementsPanel.SetActive(false);
        ExitGameButton.SetActive(true);
    }
    public void ExitGame()
    {
        ProfileManager.SaveProfile();
        Application.Quit();
    }
    public void DisplayAchievementInfo(AchievementInfo achievement)
    {
        ExitAchievementsButton.SetActive(false);
        AchievementInfoPanel.SetActive(true);
        AchievementInfoPanel.GetComponent<ScriptAchievementInfo>().DisplayAchievementInfo(achievement, this.GetComponent<ProfileManager>());
    }
    public void HideAchievementInfo()
    {
        AchievementInfoPanel.SetActive(false);
        ExitAchievementsButton.SetActive(true);
    }
    public void HideAllPanels()
    {
        LoadingScreen.SetActive(false);
        DifficultyPanel.SetActive(false);
        AchievementsPanel.SetActive(false);
        LogPanel.SetActive(false);
        AchievementInfoPanel.SetActive(false);
    }
}
