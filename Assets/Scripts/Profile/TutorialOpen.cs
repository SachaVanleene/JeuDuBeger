using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Managers;

public class TutorialOpen : MonoBehaviour {
    public static TutorialOpen instance = null;
    public GameObject ScriptTutoObject;
    private PauseTuto scriptTuto;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        scriptTuto = ScriptTutoObject.GetComponent<PauseTuto>();
    }

    public void TutorialCalled(string tutoName)
    {
        if (SProfilePlayer.getInstance().TutorialsCalled.Contains(tutoName))
            return;
        SProfilePlayer.getInstance().TutorialsCalled.Add(tutoName);
    }

    public void OpenTutorial(string tutoName)
    {
        if (SProfilePlayer.getInstance().TutorialsCalled.Contains(tutoName))
            return;
        GameManager.instance.PauseGame();
        scriptTuto.ShowTuto(tutoName);
    }
}
