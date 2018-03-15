using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptBackToMenuPanel : MonoBehaviour {

    public GameObject ButtonCancel;
    public GameObject ButtonAccept;
    public GameObject ButtonLose;
    public Text TextDisplayed;

    public void Pause(bool gameOver = false)
    {
        ButtonAccept.SetActive(!gameOver);
        ButtonCancel.SetActive(!gameOver);
        ButtonLose.SetActive(gameOver);
        TextDisplayed.text = (gameOver)? 
            "Vous avez perdu tout vos moutons !"
            : "Pause. \n Abandonner cette partie ?";
    }
}
