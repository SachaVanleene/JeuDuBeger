using System.Collections;
using System.Collections.Generic;
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
	    if (TrapFactory.ClosestTrap == null) return;
	    AdjustPosition();
    }

    public void AdjustPosition()
    {
        int levelIndex;
        Vector3 textPosition =
            Camera.main.WorldToScreenPoint(TrapFactory.ClosestTrap.transform.parent.position + new Vector3(3, 0.5f, 0));
        if (TrapFactory.ClosestTrap.Level < 3)
            levelIndex = TrapFactory.ClosestTrap.Level - 1;
        else
            levelIndex = 1;

        GetComponentsInChildren<Text>()[0].text = TrapFactory.ClosestTrap.UpgradeCosts[levelIndex] + " ";
        GetComponentsInChildren<Text>()[1].text = TrapFactory.ClosestTrap.Durability + "/" + TrapFactory.ClosestTrap.DurabilityMax;

        transform.position = textPosition;
    }

}
