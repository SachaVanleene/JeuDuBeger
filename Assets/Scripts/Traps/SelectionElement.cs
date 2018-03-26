using Assets.Script.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Traps
{
    public class SelectionElement : MonoBehaviour
    {

        // Use this for initialization
        void Start ()
        {
            transform.localScale = Vector3.one * 0.9f;
            int value = 0;
            switch (name)
            {
                case "Needle":
                    value = GameVariables.Trap.NeedleTrap.upgradePrice[0];
                    break;
                case "Bait":
                    value = GameVariables.Trap.Decoy.upgradePrice[0];
                    break;
                case "Mud":
                    value = GameVariables.Trap.Mud.upgradePrice[0];
                    break;
                case "LandMine":
                    value = GameVariables.Trap.LandMine.upgradePrice[0];
                    break;                    
            }
            GetComponentInChildren<Text>().text = value.ToString();
        }
	
        // Update is called once per frame
        void Update () {
		
        }

        public void Select()
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.localScale = Vector3.one;
        }
        public void Unselect()
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.localScale = Vector3.one * 0.9f;
        }
    }
}
