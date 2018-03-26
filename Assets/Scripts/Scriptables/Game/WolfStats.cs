using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Game/Wolf stats")]
    public class WolfStats : ScriptableObject {
        public float life;
        public float enclosureDamage;
        public float playerDamage;
        public float goldReward;
        public float range;
        public float delay;

        
        public float CurrentLife { get; set; }
        public float CurrentEnclosureDamage { get; set; }
        public float CurrentPlayerDamage { get; set; }
        public  float CurrentGoldReward { get; set; }

        public void SetDifficulty(SO.Difficulty difficultySettings, int selectedDifficulty)
        {
            CurrentLife = life;
            CurrentEnclosureDamage = enclosureDamage;
            CurrentPlayerDamage = playerDamage;
            CurrentGoldReward = goldReward;

            CurrentEnclosureDamage *= difficultySettings.enclosureDamage[selectedDifficulty];
            CurrentPlayerDamage *= difficultySettings.playerDamage[selectedDifficulty];
            CurrentLife *= difficultySettings.wolfLife[selectedDifficulty];
            CurrentGoldReward *= difficultySettings.gold[selectedDifficulty];
        }

        public void IncreaseLife(float multiplier)
        {
            CurrentLife += life * multiplier;
        }
    }
}
