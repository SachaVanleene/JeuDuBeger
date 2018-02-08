using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Traps
{
    class MudTrap :Trap
    {
        private int j = 0;
        public void Start()
        {
            Level = 0;
            Damages = new List<int>() {40, 50, 60};
            Debug.Log(Damages.Count);
            
        }

        public override IEnumerator Activate(GameObject go)
        {
            if (go.tag == "Wolf")
            {
                var rb = go.GetComponent<Rigidbody>();
                if (Damages.Count > Level) rb.velocity = rb.velocity*(Damages[Level]/100f);
                yield break;
            }
            if(go.tag != "Terrain" && go.name != "Plane") j++;

        }

        public void OnTriggerExit(Collider collider)
        {
            var rb = collider.gameObject.GetComponent<Rigidbody>();
            if (Damages.Count > Level) rb.velocity = rb.velocity * (100f/ Damages[Level]);
            if (collider.gameObject.tag != "Terrain" && collider.gameObject.name != "Plane") j--;
            Debug.Log(j);
            if (IsInPreviewMode && collider.tag != "Terrain" && j == 0)
            {
                foreach (var rend in GetComponentsInChildren<Renderer>())
                {
                    rend.sharedMaterial.color = new Color(10, 205, 0, 0.02f);
                }
            }
        }

        public override void Upgrade()
        {
            throw new NotImplementedException();
        }
    }
}
