using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptAchievementInfo : MonoBehaviour
{
    public Text NameText;
    public Text DetailsText;
    
    public void  DisplayAchievementInfo(AchievementInfo achInfo, ProfileManager ProfileManager)
    {
        // set name text
        NameText.text = achInfo.Name;
        // set image background
        Texture2D texture = ProfileManager.DefaultSprite;
        foreach (var t in ProfileManager.SpritesAchievements)
        {
            if (t.name.Equals(achInfo.Name))
                texture = t;
        }
        this.GetComponent<RawImage>().texture = texture;
        //set details text (info + completion)
        DetailsText.text = achInfo.InfoText + "\n";
        foreach (var tupleInfo in achInfo.completion)
        {
            DetailsText.text += "\n";
            if (achInfo.Aim[tupleInfo.Key] <= tupleInfo.Value)
                DetailsText.text += "[ok] ";
            DetailsText.text += tupleInfo.Key.ToString() + " : " + tupleInfo.Value + " / " + achInfo.Aim[tupleInfo.Key];
        }

    }
}
