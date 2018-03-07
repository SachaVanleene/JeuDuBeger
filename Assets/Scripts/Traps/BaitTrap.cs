using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Traps
{
    public class BaitTrap : Trap
    {
        public override IEnumerator Activate(GameObject go)
        {
            yield break;
        }
    }
}
