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

        private float currentLife;
        private float currentEnclosureDamage;
        private float currentPlayerDamage;
        private float currentGoldReward;
        private float currentRange;

        public void SetDifficulty(SO.Difficulty difficultySettings, int selectedDifficulty)
        {
            currentLife = life;
            currentEnclosureDamage = enclosureDamage;
            currentPlayerDamage = playerDamage;
            currentGoldReward = goldReward;

            currentEnclosureDamage *= difficultySettings.enclosureDamage[selectedDifficulty];
            currentPlayerDamage *= difficultySettings.playerDamage[selectedDifficulty];
            currentLife *= difficultySettings.wolfLife[selectedDifficulty];
            currentGoldReward *= difficultySettings.gold[selectedDifficulty];
        }

        public void IncreaseLife(float multiplier)
        {
            currentLife += life * multiplier;
        }
    }
}
