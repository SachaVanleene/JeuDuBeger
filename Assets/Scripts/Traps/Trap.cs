using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private Boolean _isInPreviewMode = true;

        public Boolean IsInPreviewMode
        {
            get { return _isInPreviewMode; }
            set
            {
                _isInPreviewMode = value;
                if (_isInPreviewMode)
                {
                    foreach (var rend in GetComponentsInChildren<Renderer>())
                    {
                       rend.sharedMaterial.color = new Color(10, 205, 0, 0.02f);
                    }
                }
            }
        }

        private int _level;
        public int Level
        {
            get { return this._level; }
            set
            {
                if (value <= 3)
                {
                    _level = value;
                    Durability = DurabilityMax;
                }
                else
                {
                    Durability = DurabilityMax;
                }
            }
        }

        public abstract IEnumerator Activate(GameObject go);
        public abstract void Upgrade();
        private int i = 0;
        private int v = 0;
        public void OnMouseDown()
        {
            //Destroy(TrapPrefab);
        }

        public void Create(Vector3 positionVector3)
        {
            GameObject trap = Instantiate(TrapPrefab);
            trap.transform.position = positionVector3;
        }

        public void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag != "Terrain" && collider.gameObject.name != "Plane") v++;
            // Debug.Log(IsInPreviewMode);
            if (IsInPreviewMode && collider.tag != "Terrain")
            {
                foreach (var rend in TrapPrefab.GetComponentsInChildren<Renderer>())
                {
                    rend.sharedMaterial.color = new Color(205, 0, 0, 0.02f);
                }
            }
            if(!IsActive)
                StartCoroutine(Activate(collider.gameObject));

        }

        public void LevelUp()
        {
            Level++;
            Upgrade();
        }
        public void OnTriggerExit(Collider collider)
        {
            if (collider.tag != "Terrain" && collider.name != "Plane") v--;
            if (IsInPreviewMode && collider.tag != "Terrain" && v == 0)
            {
                foreach (var rend in TrapPrefab.GetComponentsInChildren<Renderer>())
                {
                    Debug.Log(rend);
                    rend.sharedMaterial.color = new Color(10, 205, 0, 0.02f);
                }
            }
        }
    }

    public enum TrapTypes
    {
        NeedleTrap,
        BaitTrap,
        MudTrap,
        LandmineTrap 
    }
}