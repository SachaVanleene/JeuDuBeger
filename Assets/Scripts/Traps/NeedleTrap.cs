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
            DurabilityMax = GameVariables.Trap.NeedleTrap.durability;
            Durability = DurabilityMax;
            UpgradeCosts = new List<int>(GameVariables.Trap.NeedleTrap.upgradePrice);
        }

        public override void LevelUp()
        {
            base.LevelUp();
            GameOverManager.instance.goldPerTrap[0] += UpgradeCosts[Level - 1];
        }

        public override IEnumerator Activate(GameObject go)
        {
            TrapPrefab.GetComponent<Animation>().Play();
            if (go.tag.Contains("Wolf"))
            {
                TrapPrefab.GetComponent<Animation>().Play();
                IsActive = true;
                foreach (var superTarget in Physics
                    .OverlapBox(GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size / 2)
                    .Where(T => T.gameObject.tag == "Wolf"))
                {
                    WolfHealth wolf = (WolfHealth)go.GetComponent<WolfHealth>();
                    Debug.Log(wolf.getHealth());
                    wolf.takeDamage(Pows[Level-1]);
                    Debug.Log(wolf.getHealth());
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
