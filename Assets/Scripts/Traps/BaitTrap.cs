using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts;
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
        }
        public override IEnumerator Activate(GameObject go)
        {
            yield break;
        }
    }
}
