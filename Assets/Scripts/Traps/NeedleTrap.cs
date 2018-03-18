using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script.Traps
{
    public class NeedleTrap : Trap {
        public NeedleTrap()
        {
            DurabilityMax = 100;
            Durability = DurabilityMax;
            Damages = new List<int>() { 5, 10, 20 };

        }

        public override IEnumerator Activate(GameObject go)
        {
            //TrapPrefab.GetComponent<Animation>().Play();
            if (go.tag.Contains("Wolf"))
            {
                TrapPrefab.GetComponent<Animation>().Play();
                IsActive = true;
              /**  foreach (var superTarget in Physics
                    .OverlapBox(GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size / 2)
                    .Where(T => T.gameObject.tag == "Wolf"))
                {**/
                    WolfHealth wolf = (WolfHealth)go.GetComponent<WolfHealth>();
                    Debug.Log(wolf.getHealth());
                    wolf.takeDamage(Damages[Level-1]);
                    Debug.Log(wolf.getHealth());
                // }
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
