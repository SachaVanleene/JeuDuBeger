using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Traps
{
    class LandmineTrap : Trap

    {
        public LandmineTrap()
        {
            DurabilityMax = 1;
            Durability = 1;
            Damages = new List<int>(){100,200,300};
            UpgradeCosts = new List<int>() { 100, 200, 300 };
            Height = 10;
            Length = 10;
            Width = 10;


        }
        public override IEnumerator Activate(GameObject go)
        {
            GameObject boom = CFX_SpawnSystem.GetNextObject(TrapPrefab);
            boom.transform.position = gameObject.transform.position;
            Destroy(gameObject);
            yield break;
        }



        public override void Upgrade()
        {
            throw new NotImplementedException();
        }
    }
}
