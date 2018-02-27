using System.Collections;
using System.Collections.Generic;
using Assets.Script.Factory;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Camera Camera;
	void Start () {
		
	}
	
	void Update () {
	   if (TrapFactory.ClosestTrap != null)
	    {
	        transform.GetChild(0).gameObject.SetActive(true);
	        Vector3 textPosition =
	        Camera.WorldToScreenPoint(TrapFactory.ClosestTrap.transform.parent.position + new Vector3(0, 2, 0));
	        transform.GetChild(0).position = textPosition;
	        transform.GetChild(0).GetComponent<Text>().text = "Level : " + TrapFactory.ClosestTrap.Level +
	                                                          "\n Durability : " + TrapFactory.ClosestTrap.Durability + "/" + TrapFactory.ClosestTrap.DurabilityMax;
	        foreach (var rend in TrapFactory.ClosestTrap.transform.parent.GetComponentsInChildren<Renderer>())
	        {
	            rend.material.color = Color.white;
	        }
        }
	    else
	    {
	        transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
