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
        foreach (var ev in achInfo.EventsToListen)
        {
            DetailsText.text += "\n";
            int[] values = achInfo.getCompletion(ev);
            if (values[1] <= values[0])
                DetailsText.text += "[ok] ";
            DetailsText.text += ev.ToString() + " : " + values[0] + " / " + values[1];
        }

    }
}
