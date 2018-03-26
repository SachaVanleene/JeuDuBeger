using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Game/Difficulty Multiplier")]
    public class Difficulty : ScriptableObject
    {
        // Wolf life multiplier
        public float[] wolfLife;
        // Damage dealt by the wolf to an enclosure multiplier
        public float[] enclosureDamage;
        // Damage dealt by the wolf to the player multiplier
        public float[] playerDamage;
        // Gold reward from enclosures & wolves multiplier
        public float[] gold;
    }
}
