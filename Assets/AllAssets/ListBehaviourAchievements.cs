using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListBehaviourAchievements : MonoBehaviour {

    public GameObject AchievementPrefab;
    public GameObject ObjectMainMenu;
    public GameObject ListAchievements;

    private List<GameObject> elements;

    public void Start()
    {
        elements = new List<GameObject>();
        CreateListPanel();
    }

    public void CreateListPanel() {
        foreach(var go in elements)
        {
            UnityEngine.Object.Destroy(go);
        }
        elements = new List<GameObject>();

        ListAchievements.GetComponent<RectTransform>().sizeDelta = new Vector2(220f * ProfileManager.ProfilesFound.Count, 0);
        ListAchievements.transform.localPosition += new Vector3((ProfileManager.ProfilesFound.Count * 220f) / 2, 0, 0);
        float x = (ProfileManager.ProfilesFound.Count * 220f) / 2 + 110f;

        foreach(var data in ProfileManager.ProfilesFound)
        {
            x -= 220;

            GameObject r = Instantiate(AchievementPrefab, ListAchievements.transform);
            elements.Add(r);
            r.GetComponent<ScriptLoadProfile>().fullName = data[0] + "-" + data[1] + ".save";
            r.GetComponent<Button>().onClick.AddListener(r.GetComponent<ScriptLoadProfile>().Load);
            r.GetComponent<Button>().onClick.AddListener(ObjectMainMenu.GetComponent<MainMenu>().HideLogPanel);
            r.transform.localScale = new Vector3(1f, 1f, 1f);
            r.transform.localPosition = new Vector3(x, 0, 0);
                
            //set attributes
            Text name = r.transform.Find("Name").GetComponent<Text>();
            Text lastDate = r.transform.Find("LastSave").GetComponent<Text>();
            name.text = data[0];
            lastDate.text = data[1];
        }
    }    
}
