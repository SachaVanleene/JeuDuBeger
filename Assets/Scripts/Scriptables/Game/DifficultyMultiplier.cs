using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Game/Difficulty Multiplier")]
    public class Difficulty : ScriptableObject
    {
        // Wolf life multiplier
        public float[] wolfLife = new float[3];
        // Damage dealt by the wolf to an enclosure multiplier
        public float[] enclosureDamage = new float[3];
        // Damage dealt by the wolf to the player multiplier
        public float[] playerDamage = new float[3];
        // Gold reward from enclosures & wolves multiplier
        public float[] gold = new float[3];
    }
}
