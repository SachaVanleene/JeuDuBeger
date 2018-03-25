using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListBehaviour : MonoBehaviour {

    public GameObject DetailPrefab;
    public GameObject ObjectMainMenu;
    public GameObject ListProfiles;
    public GameObject ScrollList;
    public GameObject TextLogPanel;
    public GameObject TextNoProfileFound;
    private List<GameObject> elements;

    public void Start()
    {
        elements = new List<GameObject>();
        CreateListPanel();
    }

    public void CreateListPanel() {
        if(elements != null)
            foreach(var go in elements)
            {
                UnityEngine.Object.Destroy(go);
            }
        elements = new List<GameObject>();

        if(ProfileManager.ProfilesFound.Count <= 0)
        {
            ScrollList.SetActive(false);
            TextLogPanel.SetActive(false);
            TextNoProfileFound.SetActive(true);
            return;
        }
        else
        {
            ScrollList.SetActive(true);
            TextLogPanel.SetActive(true);
            TextNoProfileFound.SetActive(false);
        }

        ListProfiles.GetComponent<RectTransform>().sizeDelta = new Vector2(220f * ProfileManager.ProfilesFound.Count, 0);
        ListProfiles.transform.localPosition += new Vector3((ProfileManager.ProfilesFound.Count * 220f) / 2, 0, 0);
        float x = (ProfileManager.ProfilesFound.Count * 220f) / 2 + 110f;

        foreach(var data in ProfileManager.ProfilesFound)
        {
            x -= 220;

            GameObject r = Instantiate(DetailPrefab, ListProfiles.transform);
            elements.Add(r);
            r.GetComponent<ScriptLoadProfile>().manager = ObjectMainMenu.GetComponent<ProfileManager>();
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
