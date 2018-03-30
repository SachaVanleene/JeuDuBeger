using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListBehaviourAchievements : MonoBehaviour {

    public GameObject AchievementPrefab;
    public GameObject ObjectMainMenu;
    public GameObject ListAchievements;

    private List<GameObject> elements = null;

    public void CreateListPanel() {
        if(elements == null)
            elements = new List<GameObject>();
        else
            foreach (var go in elements)
            {
                UnityEngine.Object.Destroy(go);
            }
        elements = new List<GameObject>();
        float padding = 150f;
        int sizeList =  SProfilePlayer.getInstance().AchievementsManager.getAchivements().Count;
        if (sizeList % 2 != 0)
            sizeList++;

        ListAchievements.GetComponent<RectTransform>().sizeDelta = new Vector2(padding * sizeList + 20f, 0); 
        ListAchievements.transform.localPosition += new Vector3((sizeList * padding) / 2, 0, 0);

        float x = ((sizeList * padding) + padding) / 2f + 50f;
        bool createBottom = false;

        foreach(var achInfo in SProfilePlayer.getInstance().AchievementsManager.getAchivements())
        {
            x -= (createBottom)? 0 : padding * 2f;
            int y = (int) ((createBottom) ? padding : -padding);
            createBottom = !createBottom;

            GameObject r = Instantiate(AchievementPrefab, ListAchievements.transform);
            elements.Add(r);
            Texture2D texture = ObjectMainMenu.GetComponent<ProfileManager>().DefaultSprite;
            if (achInfo.IsComplete())
                texture = ObjectMainMenu.GetComponent<ProfileManager>().CompletedSprite;
            else
                foreach (var t in ObjectMainMenu.GetComponent<ProfileManager>().SpritesAchievements)
                {
                    if (t.name.Equals(achInfo.Name))
                        texture = t;
                }
            r.GetComponent<RawImage>().texture = texture;
            r.GetComponent<ScriptDisplayAchievement>().Name = achInfo;
            r.GetComponent<ScriptDisplayAchievement>().MenuGO = ObjectMainMenu;
            r.GetComponent<Button>().onClick.AddListener(r.GetComponent<ScriptDisplayAchievement>().Load);
            r.transform.localScale = new Vector3(1f, 1f, 1f);
            r.transform.localPosition = new Vector3(x, y, 0);
                
            //set attributes
            Text name = r.transform.Find("Name").GetComponent<Text>();
            name.text = achInfo.Name;
        }
    }    
}
