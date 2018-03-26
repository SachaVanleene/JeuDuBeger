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
        int levelIndex;
        if (TrapCreator.TargetedTrap.Level < 3)
            levelIndex = TrapCreator.TargetedTrap.Level;
        else
            levelIndex = 2;

        GetComponentsInChildren<Text>()[0].text = TrapCreator.TargetedTrap.UpgradeCosts[levelIndex] + " ";
        GetComponentsInChildren<Text>()[1].text = TrapCreator.TargetedTrap.Durability + "/" + TrapCreator.TargetedTrap.DurabilityMax;
    }

}
