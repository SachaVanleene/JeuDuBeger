using System.Collections;
using UnityEngine;

namespace Assets.Script.Traps
{
    public class NeedleTrap : Trap {
        public NeedleTrap()
        {
        }

        public override IEnumerator Activate()
        {
            TrapPrefab.GetComponent<Animation>().Play();
            IsActive = true;
            yield return new WaitForSeconds(2f);
            IsActive = false;
            Debug.Log(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name
            );
        }
        
        public override void Upgrade()
        {
            throw new System.NotImplementedException();
        }
    }
}
