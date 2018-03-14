using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDisplayAchievement : MonoBehaviour {

    public GameObject MenuGO;
    public AchievementInfo Name;
    public void Load()
    {
        MenuGO.GetComponent<MainMenu>().DisplayAchievementInfo(Name);
    }
}
