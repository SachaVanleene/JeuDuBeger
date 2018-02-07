using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Traps
{
    class MudTrap :Trap
    {
        public void Start()
        {
            Level = 0;
            Damages = new List<int>() {40, 50, 60};
            Debug.Log(Damages.Count);

        }

        public override IEnumerator Activate(GameObject go)
        {
            var rb = go.GetComponent<Rigidbody>();
            if (Damages.Count > Level) rb.velocity = rb.velocity*(Damages[Level]/100f);
            yield break;
        }

        public void OnTriggerExit(Collider collider)
        {
            var rb = collider.gameObject.GetComponent<Rigidbody>();
            if (Damages.Count > Level) rb.velocity = rb.velocity * (100f/ Damages[Level]);
        }

        public override void Upgrade()
        {
            throw new NotImplementedException();
        }
    }
}
