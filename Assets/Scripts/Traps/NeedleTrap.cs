using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Script.Traps
{
    public class NeedleTrap : Trap {
        public NeedleTrap()
        {
        }

        public override IEnumerator Activate(GameObject go)
        {
            TrapPrefab.GetComponent<Animation>().Play();
            IsActive = true;
            foreach (var superTarget in Physics.OverlapBox(GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size/2).
                Where(T => T.gameObject.tag == "wolf") )
            {
                //implement dealt damage
                Debug.Log("lol");
            }
            yield return new WaitForSeconds(2f);
            IsActive = false;
        }
        
        public override void Upgrade()
        {
            throw new System.NotImplementedException();
        }
    }
}
