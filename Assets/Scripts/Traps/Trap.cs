using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Managers;
using Assets.Scripts.Traps;
using UnityEngine;

namespace Assets.Script.Traps
{
    public abstract class Trap : MonoBehaviour
    {
        public int DurabilityMax, Durability;
        public GameObject TrapPrefab;
        public Boolean IsActive = false;

        private int i = 0;
        private int v;

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
            get { return _level; }
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

        public abstract List<int> UpgradeCosts { get; set; }
        public abstract List<int> Pows { get; set; }
        public abstract IEnumerator Activate(GameObject go);
        
        public void OnTriggerEnter(Collider collider)
        {   
            if (collider.gameObject.tag != "Terrain" && collider.gameObject.name != "Plane" && collider.gameObject.name != "WaterShower" &&  collider.gameObject.name != "ShootableHitbox")
            {
                v++;
            }
            if (IsInPreviewMode && collider.tag != "Terrain")
            {
                foreach (var rend in TrapPrefab.GetComponentsInChildren<Renderer>())
                {
                    rend.sharedMaterial.color = new Color(205, 0, 0, 0.02f);
                }
            }
            if (!IsActive && !IsInPreviewMode)
                StartCoroutine(Activate(collider.gameObject));

        }

        public virtual void LevelUp()
        {
            if (GameManager.instance.SpendGold(UpgradeCosts[Level-1]))
            {
                Level++;
            }
        }

        public void Deselect()
        {
            foreach (var rend in TrapCreator.TargetedTrap.transform.parent
                .GetComponentsInChildren<Renderer>())
            {
                rend.material.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            }
        }

        public void Select()
        {
            foreach (var rend in TrapCreator.TargetedTrap.transform.parent
                .GetComponentsInChildren<Renderer>())
            {
                rend.material.color = Color.white;
            }
        }

        public void Destroy()
        {
            Destroy(TrapPrefab);
        }

        public void OnTriggerExit(Collider collider)
        {
            if (collider.tag != "Terrain" && collider.name != "Plane") v--;
            if (collider.tag == "Fences" &&
                Vector3.Distance(transform.position, collider.transform.parent.position) <
                collider.transform.localPosition.magnitude) return;
            if (IsInPreviewMode && collider.tag != "Terrain" && v == 0)
            {
                foreach (var rend in TrapPrefab.GetComponentsInChildren<Renderer>())
                {
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
        LandmineTrap,
        None
    }
}