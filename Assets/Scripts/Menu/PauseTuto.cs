using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Managers;

public class PauseTuto : MonoBehaviour {

    public GameObject panelTuto;
    public List<GameObject> allTutos;
    public List<Button> buttons;
    int numBouton;


    void Start () {
        panelTuto.SetActive(false);
        allTutos[0].SetActive(true);
        ColorBlock cb = buttons[0].colors;
        cb.normalColor = Color.gray;
        cb.highlightedColor = Color.gray;
        cb.pressedColor = Color.gray;
        buttons[0].colors = cb;
    }
	
	void Update () {
        handleInputs();
    }


    public void handleInputs()
    {
        //Appuye sur bouton pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Si le panneau tuto est ouvert
            if (panelTuto.activeSelf) {
                GameManager.instance.UnPauseGame();
                panelTuto.SetActive(false);

            //Si le panneau tuto est fermé : pause
            } else
            {
                GameManager.instance.PauseGame();
                panelTuto.SetActive(true);
            }
        }
    
    }
    
    public void CloseTutoPanel()
    {
        GameManager.instance.UnPauseGame();
        panelTuto.SetActive(false);
    }

    public void ShowTuto(GameObject Tuto)
    {
        //Désactivation des autres tutos + activation de celui cliqué
        foreach (GameObject tuto in allTutos)
        {
            tuto.SetActive(false);
        }
        Tuto.SetActive(true);

        //Changement de couleur des boutons
        foreach (Button b in buttons)
        {
            ColorBlock cb1 = b.colors;
            cb1.normalColor = Color.white;
            b.colors = cb1;
        }
        numBouton = int.Parse(Tuto.name[buttons.Count-1].ToString())-1;
        ColorBlock cb = buttons[numBouton].colors;
        cb.normalColor = Color.gray;
        cb.highlightedColor = Color.gray;
        cb.pressedColor = Color.gray;
        buttons[numBouton].colors = cb;
    }

  

}
