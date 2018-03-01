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

        private int _level = 1;
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

        public void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag != "Terrain" && collider.gameObject.name != "Plane") v++;
            if (IsInPreviewMode && collider.tag != "Terrain")
            {
                foreach (var rend in TrapPrefab.GetComponentsInChildren<Renderer>())
                {
                    rend.sharedMaterial.color = new Color(205, 0, 0, 0.02f);
                }
            }
            if(!IsActive && !IsInPreviewMode)
                StartCoroutine(Activate(collider.gameObject));

        }

        public void LevelUp()
        {
            Level++;
            Upgrade();
        }

        public void Update()
        {
            if (gameObject.GetComponent<Renderer>().isVisible)
            {
                bool isTheClosest;
                if (TrapFactory.ClosestTrap != null)
                {
                    isTheClosest = Vector3.Distance(TrapFactory.ClosestTrap.transform.position,
                                       TerrainTest.PlayerGameObject.transform.position) >
                                   Vector3.Distance(transform.position, TerrainTest.PlayerGameObject.transform.position);
                }
                else
                {
                    isTheClosest = true;
                }

                if (!IsInPreviewMode && isTheClosest)
                {
                    if (TrapFactory.ClosestTrap != null)
                        foreach (var rend in TrapFactory.ClosestTrap.transform.parent
                            .GetComponentsInChildren<Renderer>())
                        {
                            rend.material.color = new Color(0.3f, 0.3f, 0.3f, 1f);
                        }
                    TrapFactory.ClosestTrap = this;
                }
            }
        }

        public void OnTriggerExit(Collider collider)
        {
            if (collider.tag != "Terrain" && collider.name != "Plane") v--;
            if (IsInPreviewMode && collider.tag != "Terrain" && v == 0)
            {
                foreach (var rend in TrapPrefab.GetComponentsInChildren<Renderer>())
                {
                    rend.sharedMaterial.color = new Color(10, 205, 0, 0.02f);
                }
            }
        }
        public void OnBecameVisible()
        {
            bool isTheClosest;
            if (TrapFactory.ClosestTrap != null)
            {
                isTheClosest = Vector3.Distance(TrapFactory.ClosestTrap.transform.position,
                                   TerrainTest.PlayerGameObject.transform.position) >
                               Vector3.Distance(transform.position, TerrainTest.PlayerGameObject.transform.position);
            }
            else
            {
                isTheClosest = true;
            }
                
            if (!IsInPreviewMode && isTheClosest)
            {
                if (TrapFactory.ClosestTrap != null)
                    foreach (var rend in TrapFactory.ClosestTrap.transform.parent.GetComponentsInChildren<Renderer>())
                    {
                        rend.material.color = new Color(0.3f,0.3f,0.3f,1f);
                    }
                TrapFactory.ClosestTrap = this;
            }
        }
        public void OnBecameInvisible()
        {
            if (TrapFactory.ClosestTrap == this)
            {
                foreach (var rend in TrapFactory.ClosestTrap.transform.parent.GetComponentsInChildren<Renderer>())
                {
                    rend.material.color = new Color(0.3f, 0.3f, 0.3f, 1f);
                }
                TrapFactory.ClosestTrap = null;
            }
        }
    }


    public enum TrapTypes
    {
        NeedleTrap,
        BaitTrap,
        MudTrap,
        LandmineTrap,
        None
    }
}