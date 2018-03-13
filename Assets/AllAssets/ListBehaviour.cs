using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListBehaviour : MonoBehaviour {

    public GameObject DetailPrefab;
    public GameObject Parent;
    public GameObject ObjectMainMenu;
    private GameObject[] elements;

    public void Start() {
        List<String> names = new List<String>();
        names.Add("toto");
        names.Add("titi");
        names.Add("tata");

        names.Add("toto2");
        names.Add("titi2");
        names.Add("tata2");

        //float x = 0;// -330;
        Parent.GetComponent<RectTransform>().sizeDelta = new Vector2(220f * names.Count, 0);
        Parent.transform.localPosition += new Vector3((names.Count * 220f) / 2, 0, 0);
        float x = (names.Count * 220f) / 2 + 110f;

        foreach (String str in names)
        {
            x -= 220;

            GameObject r = Instantiate(DetailPrefab, Parent.transform);
            r.GetComponent<Button>().onClick.AddListener(ObjectMainMenu.GetComponent<MainMenu>().HideLogPanel);
            r.transform.localScale = new Vector3(1f, 1f, 1f);
            r.transform.localPosition = new Vector3(x, 0, 0);
                
            //set attributes
            Text name = r.transform.Find("Name").GetComponent<Text>();
            Text lastDate = r.transform.Find("LastSave").GetComponent<Text>();
            name.text = str;
            lastDate.text = "9 Mars 2018";
        }
    }    
}
