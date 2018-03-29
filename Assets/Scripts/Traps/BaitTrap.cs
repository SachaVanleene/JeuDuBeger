using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Managers;
using Assets.Scripts;
using Assets.Scripts.Traps;
using UnityEngine;

namespace Assets.Script.Traps
{
    public class BaitTrap : Trap
    {
        public sealed override List<int> UpgradeCosts { get; set; }
        public sealed override List<int> Pows { get; set; }

        public BaitTrap()
        {
            UpgradeCosts = new List<int>(GameVariables.Trap.Decoy.upgradePrice);
            SellingPrice = (int)(UpgradeCosts[0] * 0.75f);
            DurabilityMax = GameVariables.Trap.Decoy.life;
        }
        public override void LevelUp()
        {
            var levelIndex = Level < 3 ? TrapCreator.TargetedTrap.Level : 2;

            if (TrapCreator.TargetedTrap.Level == 3)
                if (Durability == DurabilityMax)
                    return;
            if (!GameManager.instance.SpendGold(UpgradeCosts[levelIndex])) return;
            DurabilityMax = GameVariables.Trap.Decoy.life * GameVariables.Trap.Decoy.pows[levelIndex];
            SellingPrice += (int)(UpgradeCosts[levelIndex] * 0.75f);
            Level++;
            GameOverManager.instance.goldPerTrap[1] += UpgradeCosts[Level - 1];
        }
        public override IEnumerator Activate(GameObject go)
        {
            yield break;
        }
    }
}
