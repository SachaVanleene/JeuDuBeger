using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeGun : MonoBehaviour {

    public SO.BoolVariable isInGunPanelRange;
    public SO.GunStats gunStats;

    [Space]
    public SO.IntVariable gunUpgradePrice;
    public SO.IntVariable gunLevel;
    public SO.GameEvent gunUpgradeEvent;
    public GameObject buttonGO;

    private int level;
    private BaseEventData pointer;

	// Use this for initialization
	void Start () {
        level = 0;
        gunLevel.Set(level + 1);
        gunUpgradePrice.Set(gunStats.upgradeCost[level]);
        pointer = new PointerEventData(EventSystem.current);
    }
	
	// Update is called once per frame
	void Update () {
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            ExecuteEvents.Execute(buttonGO, pointer, ExecuteEvents.submitHandler);
        }
            

    }

    public void TryUpgradeGun()
    {
        if (level < 2 &&
            isInGunPanelRange.value &&
            Assets.Script.Managers.GameManager.instance.SpendGold(gunStats.upgradeCost[level]))
        {
            level++;
            gunStats.SetValues(level);

            if (level < 2)
                gunUpgradePrice.Set(gunStats.upgradeCost[level]);
            else
                gunUpgradePrice.Set(0);
            gunLevel.Set(level + 1);

            gunUpgradeEvent.Raise();
        }
    }
}
