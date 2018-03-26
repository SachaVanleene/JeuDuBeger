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
        if(elements == null)
            elements = new List<GameObject>();
        CreateListPanel();
    }
    public void ResetList()
    {
        Debug.Log("reset");
        if (elements != null)
            foreach (var go in elements)
            {
                UnityEngine.Object.DestroyImmediate(go);
            }
        else
            Debug.Log("nothing to delete");
        elements = new List<GameObject>();
    }
    public void CreateListPanel() {
        ResetList();

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
        Debug.Log(ProfileManager.ProfilesFound.Count);
        foreach(var data in ProfileManager.ProfilesFound)
        {
            x -= 220;
            GameObject profileDetail = Instantiate(DetailPrefab, ListProfiles.transform);
            
            elements.Add(profileDetail);
            profileDetail.GetComponent<ScriptLoadProfile>().manager = ObjectMainMenu.GetComponent<ProfileManager>();
            profileDetail.GetComponent<ScriptLoadProfile>().list = this;
            profileDetail.GetComponent<ScriptLoadProfile>().fullName = data[0] + "-" + data[1] + ".save";
            profileDetail.GetComponent<Button>().onClick.AddListener(profileDetail.GetComponent<ScriptLoadProfile>().Load);
            profileDetail.GetComponent<Button>().onClick.AddListener(ObjectMainMenu.GetComponent<MainMenu>().HideLogPanel);
            profileDetail.transform.localScale = new Vector3(1f, 1f, 1f);
            profileDetail.transform.localPosition = new Vector3(x, 0, 0);
                
            //set attributes
            profileDetail.transform.Find("Name").GetComponent<Text>().text = data[0];
            profileDetail.transform.Find("LastSave").GetComponent<Text>().text = data[1];
        }
    }    
}
