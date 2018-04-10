using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Managers;

public class PauseTuto : MonoBehaviour
{

    public GameObject PanelTuto;
    public GameObject ButtonPanelTuto;
    public Text NameCurrentTuto;
    public List<GameObject> ListPanelsTuto;
    public List<Texture2D> ImagesTuto;

    public void GameOver()
    {
        if (!PanelTuto.activeSelf)
            return;
        ButtonPanelTuto.GetComponent<Image>().transform.Rotate(0, 0, 180);
        PanelTuto.SetActive(false);
    }
    public void ChangeActivityTutoPanel()
    {
        PanelTuto.SetActive(!PanelTuto.activeSelf);
        ButtonPanelTuto.GetComponent<Image>().transform.Rotate(0, 0, 180);
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
        if(!PanelTuto.activeSelf)
        {
            PanelTuto.SetActive(true);
            ButtonPanelTuto.GetComponent<Image>().transform.Rotate(0, 0, 180);
        }
        foreach (var tutoPanel in ListPanelsTuto)
        {
            if (tutoPanel.name.Equals(tutoName))
            {
                tutoPanel.SetActive(true);
                NameCurrentTuto.text = tutoName;
                TutorialOpen.instance.TutorialCalled(tutoName);
                return;
            }
        }
        NameCurrentTuto.text = "Panel " + tutoName + " non trouvé";
    }
}