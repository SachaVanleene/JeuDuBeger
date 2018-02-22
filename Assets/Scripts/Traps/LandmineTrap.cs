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
        public GameObject ExplosionEffect;
        public LandmineTrap()
        {
            DurabilityMax = 1;
            Durability = 1;
            Damages = new List<int>(){20,27,35};
            UpgradeCosts = new List<int>() { 100, 200, 300 };
            Height = 10;
            Length = 10;
            Width = 10;


        }
        public override IEnumerator Activate(GameObject go)
        {
            if (!IsInPreviewMode && go.tag == "Wolf")
            {
                GameObject boom = CFX_SpawnSystem.GetNextObject(ExplosionEffect);
                boom.transform.position = gameObject.transform.position;
                foreach (var superTarget in Physics
                    .OverlapSphere(GetComponent<BoxCollider>().center, 2)
                    .Where(T => T.gameObject.tag == "Wolf"))
                {
                    WolfHealth wolf = (WolfHealth)superTarget.GetComponent<WolfHealth>();
                    wolf.takeDamage(Damages[Level]);
                }
                Durability--;
                if(Durability == 0)
                {
                    Destroy(gameObject);
                }
            }
            yield break;
        }



        public override void Upgrade()
        {
            throw new NotImplementedException();
        }
    }
}
