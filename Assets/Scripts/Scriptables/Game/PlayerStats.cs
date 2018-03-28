using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Game/Player")]
    public class PlayerStats : ScriptableObject
    {
        public float healthMax;
        public float recoverDelay;
        public float healTickDelay;
        public float healPerTick;

        public float walkSpeed;
        public float runSpeed;
        
        public float CurrentHealthMax { get; set; }
        public float CurrentRecoverDelay { get; set; }
        public float CurrentHealTickDelay { get; set; }
        public float CurrentHealPerTick { get; set; }

        public float CurrentWalkSpeed { get; set; }
        public float CurrentRunSpeed { get; set; }

        public void Init()
        {
            CurrentHealthMax = healthMax;
            CurrentRecoverDelay = recoverDelay;
            CurrentHealTickDelay = healTickDelay;
            CurrentHealPerTick = healPerTick;

            CurrentWalkSpeed = walkSpeed;
            CurrentRunSpeed = runSpeed;
        }
    }
}
