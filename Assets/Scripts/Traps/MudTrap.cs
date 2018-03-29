using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Script.Traps
{
    class MudTrap :Trap
    {
        public sealed override List<int> UpgradeCosts { get; set; }
        public sealed override List<int> Pows { get; set; }

        private int j = 0;
        public void Start()
        {
            Pows = new List<int>(GameVariables.Trap.Mud.wolfSlow);
            UpgradeCosts = new List<int>(GameVariables.Trap.Mud.upgradePrice);
            SellingPrice = (int)(UpgradeCosts[0] * 0.75f);
            DurabilityMax = 100;
            Durability = DurabilityMax;            
        }

        public override void LevelUp()
        {
            base.LevelUp();
            GameOverManager.instance.goldPerTrap[2] += UpgradeCosts[Level - 1];
        }
        public override IEnumerator Activate(GameObject go)
        {
            if (go.tag != "Terrain" && go.name != "Plane") j++;
            if (!go.tag.Contains("Wolf")) yield break;
            var rb = go.GetComponent<NavMeshAgent>();
            rb.speed = rb.speed * ( 1 - Pows[Level - 1] / 100f);
            yield break;
        }

       public new void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag == "Wolf")
            {   
                var rb = collider.gameObject.GetComponent<NavMeshAgent>();
                if (Pows.Count > Level)
                    rb.speed = rb.speed / ((1 - Pows[Level - 1] / 100f));
            }
            if (collider.gameObject.tag != "Terrain" && collider.gameObject.name != "Plane") j--;
            if (!IsInPreviewMode || collider.tag == "Terrain" || j > 0) return;
            foreach (var rend in GetComponentsInChildren<Renderer>())
            {
                rend.sharedMaterial.color = new Color(10, 205, 0, 0.02f);
            }
        }
    }
}
