using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Script.Traps
{
    public class NeedleTrap : Trap {
        public sealed override List<int> UpgradeCosts { get; set; }
        public sealed override List<int> Pows { get; set; }


        public NeedleTrap()
        {
            Pows = GameVariables.Trap.NeedleTrap.playerDamage;
            Durability = DurabilityMax;
            UpgradeCosts = new List<int>(GameVariables.Trap.NeedleTrap.upgradePrice);
            SellingPrice = (int) (UpgradeCosts[0] * 0.75f);
        }

        public override void LevelUp()
        {
            base.LevelUp();
            GameOverManager.instance.goldPerTrap[0] += UpgradeCosts[Level - 1];
        }

        public override IEnumerator Activate(GameObject go)
        {
            if (go.tag.Contains("Wolf"))
            {
               
                TrapPrefab.GetComponent<Animation>().Play();
                IsActive = true;
                foreach (var superTarget in Physics
                    .OverlapBox(TrapPrefab.transform.position, GetComponent<BoxCollider>().bounds.size)
                    .Where(T => T.gameObject.tag.Contains("Wolf")))
                {
                    WolfHealth wolf = (WolfHealth)go.GetComponent<WolfHealth>();
                    wolf.takeDamage(Pows[Level-1]);
                }
                yield return new WaitForSeconds(2f);
                Durability--;
                if (Durability == 0)
                {
                    Destroy(TrapPrefab);
                }
                IsActive = false;
            }
            yield break;
        }
    }
}
