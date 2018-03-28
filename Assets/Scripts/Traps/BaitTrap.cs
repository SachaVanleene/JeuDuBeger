using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            DurabilityMax = GameVariables.Trap.Decoy.life;// * GameVariables.Trap.Decoy.pows[levelIndex];

            base.LevelUp();
            GameOverManager.instance.goldPerTrap[1] += UpgradeCosts[Level - 1];
            
        }
        public override IEnumerator Activate(GameObject go)
        {
            yield break;
        }
    }
}
