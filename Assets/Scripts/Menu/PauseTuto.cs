using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Managers;

public class PauseTuto : MonoBehaviour {

    public GameObject PanelTuto;
    public Text NameCurrentTuto;
    public List<GameObject> ListPanelsTuto;
    public List<Texture2D> ImagesTuto;

    public void ChangeActivityTutoPanel()
    {
        PanelTuto.SetActive(!PanelTuto.activeSelf);
    }

    private void hideAllTutorialPanels()
    {
        foreach (var tutoPanel in ListPanelsTuto)
        {
            tutoPanel.SetActive(false);
        }
    }

    public void ShowTuto(string tutoName)
    {
        hideAllTutorialPanels();
        foreach(var tutoPanel in ListPanelsTuto)
        {
            if(tutoPanel.name.Equals(tutoName))
            {
                tutoPanel.SetActive(true);
                NameCurrentTuto.text = tutoName;
                return;
            }
        }
        NameCurrentTuto.text = "Panel " + tutoName + " non trouvé";
    }
    
}