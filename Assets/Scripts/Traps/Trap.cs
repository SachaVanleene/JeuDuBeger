using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using Assets.Script.Factory;
using UnityEngine;

namespace Assets.Script.Traps
{
    public abstract class Trap : MonoBehaviour
    {
        public int Height, Width, Length;
        public int DurabilityMax, Durability, BuyingCost;
        public GameObject TrapPrefab;
        public List<int> UpgradeCosts, Damages;
        public Boolean IsActive = false;
        public int Level
        {
            get { return Level; }
            set
            {
                if (value <= 3)
                {
                    Level = value;
                    Durability = DurabilityMax;
                }
                else
                {
                    Durability = DurabilityMax;
                }
            }
        }

        public abstract IEnumerator Activate();
        public abstract void Upgrade();
        private int i = 0;

        public void OnMouseDown()
        {
            Destroy(TrapPrefab);
        }

        public void Create(Vector3 positionVector3)
        {
            GameObject trap = Instantiate(TrapPrefab);
            trap.transform.position = positionVector3;
        }

        public void OnTriggerEnter(Collider collider)
        {
            Debug.Log("bim");
            if(!IsActive)
                StartCoroutine(Activate());
        }

        public void LevelUp()
        {
            Level++;
            Upgrade();
        }
    }

    public enum TrapTypes
    {
        NeedleTrap,
        BaitTrap,
        MudTrap,
        LandmineTrap,
    
        
    }
}