using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Script.Traps
{
    class MudTrap :Trap
    {
        private int j = 0;
        public void Start()
        {
            Damages = new List<int>() {40, 50, 60};
            DurabilityMax = 100;
            Durability = DurabilityMax;            
        }

        public override IEnumerator Activate(GameObject go)
        {
            if (go.tag != "Terrain" && go.name != "Plane") j++;
            if (go.tag.Contains("Wolf"))
            {
                var rb = go.GetComponent<NavMeshAgent>();
                if (Damages.Count > Level)
                {
                    Debug.Log(rb.velocity);
                    rb.speed = rb.speed*(Damages[Level]/100f);
                }
                yield break;
            }    
        }

        public void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag == "Wolf")
            {   
                var rb = collider.gameObject.GetComponent<NavMeshAgent>();
                if (Damages.Count > Level) rb.speed = 10f;
            }
            if (collider.gameObject.tag != "Terrain" && collider.gameObject.name != "Plane") j--;
            Debug.Log(j);
            if (IsInPreviewMode && collider.tag != "Terrain" && j <= 0)
            {
                foreach (var rend in GetComponentsInChildren<Renderer>())
                {
                    rend.sharedMaterial.color = new Color(10, 205, 0, 0.02f);
                }
            }
        }
    }
}
