using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Managers;



public class TutorialOpen : MonoBehaviour {

    public static TutorialOpen instance = null;

    public GameObject panel;
    PauseTuto scrip_tuto;

    bool howToPlayTutorial;
    bool wolfTutorial;
    bool goldTutorial;
    bool trapsTutorial;
    bool sheepTutorial;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        scrip_tuto = panel.GetComponent<PauseTuto>();

        howToPlayTutorial = false;
        wolfTutorial = false;
        goldTutorial = false;
        trapsTutorial = false;
        sheepTutorial = false;
    }

    // 1 how to play; 2 wolf; 3 golds 4 traps; 5 sheeps
    public void OpenTutorial(int tuto)
    {
        switch (tuto)
        {
            case 1:
                if (!howToPlayTutorial)
                {
                    GameManager.instance.PauseGame();
                    scrip_tuto.ShowTuto("deroulement");
                    howToPlayTutorial = true;
                }
                break;
            case 2:
                if (!wolfTutorial)
                {
                    GameManager.instance.PauseGame();
                    scrip_tuto.ShowTuto("loups");
                    wolfTutorial = true;
                }
                break;
            case 3:
                if (!goldTutorial)
                {
                    GameManager.instance.PauseGame();
                    scrip_tuto.ShowTuto("monnaie");
                    goldTutorial = true;
                }
                break;
            case 4:
                if (!trapsTutorial)
                {
                    GameManager.instance.PauseGame();
                    scrip_tuto.ShowTuto("pieges");
                    trapsTutorial = true;
                }
                break;
            case 5:
                if (!sheepTutorial)
                {
                    GameManager.instance.PauseGame();
                    scrip_tuto.ShowTuto("moutons");
                    sheepTutorial = true;
                }
                break;
            default:
                break;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
