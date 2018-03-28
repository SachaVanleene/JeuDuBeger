using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("Difficulty")]
    public SO.Difficulty difficultySettings;
    public int selectedDifficulty;

    public List<SO.WolfStats> wolvesStats;

    [Space]
    [Header("Player & Gun stats")]
    public SO.PlayerStats playerStats;
    public SO.GunStats gunStats;

    private void Start()
    {
        SetDiffilculty();
    }

    public void SetDiffilculty()
    {
        selectedDifficulty = SProfilePlayer.getInstance().Difficulty;
        foreach (SO.WolfStats wolfStats in wolvesStats)
        {
            wolfStats.SetDifficulty(difficultySettings, selectedDifficulty);
        }
    }

    public void SetCurrentGunStats()
    {
        gunStats.DamageMultiplier = 1f;
        gunStats.FireRateMultiplier = 1f;

        if (SProfilePlayer.getInstance().AchievementsManager.GetAchievementByName("Shoot").IsComplete())
        {
            gunStats.DamageMultiplier = 1.1f;
        }
        
        // ...

        gunStats.Init();
    }

    public void SetCurrentPlayerStats()
    {
        playerStats.Init();
    }
}
