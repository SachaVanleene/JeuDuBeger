using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Script.Traps
{
    class LandmineTrap : Trap

    {
        public sealed override List<int> UpgradeCosts { get; set; }
        public sealed override List<int> Pows { get; set; }

        public GameObject ExplosionEffect;

        public LandmineTrap()
        {
            DurabilityMax = 1;
            Durability = 1;
            Pows = new List<int>(GameVariables.Trap.LandMine.playerDamage);
            UpgradeCosts = new List<int>(GameVariables.Trap.LandMine.upgradePrice);
        }


        public override IEnumerator Activate(GameObject go)
        {
            if (!IsInPreviewMode && go.tag.Contains("Wolf"))
            {
                GameObject boom = CFX_SpawnSystem.GetNextObject(ExplosionEffect);
                boom.transform.position = gameObject.transform.position;

                foreach (var superTarget in Physics
                    .OverlapSphere(transform.position, GameVariables.Trap.LandMine.radius[Level - 1])
                    .Where(T => T.gameObject.tag.Contains("Wolf")))
                {
                    WolfHealth wolf = (WolfHealth) superTarget.GetComponent<WolfHealth>();
                    wolf.takeDamage(Pows[Level]);
                }
                Durability--;
                if (Durability == 0)
                {
                    Destroy(gameObject);
                }
            }
            yield break;
        }
    }
}
