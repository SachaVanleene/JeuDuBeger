using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public SO.Difficulty difficultySettings;

    public List<SO.WolfStats> wolvesStats;
    public int selectedDifficulty;

    public void SetDiffilculty()
    {
        
        selectedDifficulty = SProfilePlayer.getInstance().Difficulty - 1;
        selectedDifficulty = Mathf.Clamp(selectedDifficulty, 0, 2);
        foreach (SO.WolfStats wolfStats in wolvesStats)
        {
            wolfStats.SetDifficulty(difficultySettings, selectedDifficulty);
        }
    }
}
