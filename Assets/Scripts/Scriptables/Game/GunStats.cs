using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{

    [CreateAssetMenu(menuName = "Game/Gun")]
    public class GunStats : ScriptableObject {
        public float[] fireRate = new float[3];
        public float[] damage = new float[3];
        public int[] upgradeCost = new int[2];

        public float CurrentFireRate { get; set; }
        public float CurrentDamage { get; set; }

        public float FireRateMultiplier { get; set; }
        public float DamageMultiplier { get; set; }

        public void Init()
        {
            CurrentFireRate = fireRate[0] * FireRateMultiplier;
            CurrentDamage = damage[0] * DamageMultiplier;
        }

        public void SetValues(int level)
        {
            CurrentFireRate = fireRate[level] * FireRateMultiplier;
            CurrentDamage = damage[level] * DamageMultiplier;
        }

    }
}
