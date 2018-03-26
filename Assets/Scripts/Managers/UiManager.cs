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
        Vector3 trapPositionToScreenPoint =
            Camera.main.WorldToScreenPoint(TrapCreator.TargetedTrap.transform.parent.position);
        Vector3 textPosition = trapPositionToScreenPoint + new Vector3(3, 0.5f, 0);
        if (TrapCreator.TargetedTrap.Level < 3)
            levelIndex = TrapCreator.TargetedTrap.Level - 1;
        else
            levelIndex = 1;

        GetComponentsInChildren<Text>()[0].text = TrapCreator.TargetedTrap.UpgradeCosts[levelIndex] + " ";
        GetComponentsInChildren<Text>()[1].text = TrapCreator.TargetedTrap.Durability + "/" + TrapCreator.TargetedTrap.DurabilityMax;

        transform.localPosition = textPosition;
    }

}
