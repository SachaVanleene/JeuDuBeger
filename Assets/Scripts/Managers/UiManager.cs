using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using Assets.Scripts.Traps;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Camera Camera;
	void Start () {
		
	}
	
	void Update ()
	{
	    if (TrapCreator.TargetedTrap == null) return;
	    AdjustPosition();
    }

    public void AdjustPosition()
    {
        var levelIndex = TrapCreator.TargetedTrap.Level < 3 ? TrapCreator.TargetedTrap.Level : 2;

        if (TrapCreator.TargetedTrap.Level >= 3)
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            Color green = new Color(6 / 255f, 201 / 255f, 24 / 255f);
            foreach (var text in GetComponentsInChildren<Text>())
            {
                text.color = green;
            }
        }
        else
        {
            transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            foreach (var text in GetComponentsInChildren<Text>())
            {
                text.color = Color.red;
            }
        }

        GetComponentsInChildren<Text>()[0].text = TrapCreator.TargetedTrap.UpgradeCosts[levelIndex] + " ";
        GetComponentsInChildren<Text>()[1].text = TrapCreator.TargetedTrap.Durability + "/" + TrapCreator.TargetedTrap.DurabilityMax;
        GetComponentsInChildren<Text>()[2].text = "Level " + TrapCreator.TargetedTrap.Level;

    }

}
