using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public SO.Difficulty difficultySettings;

    public List<SO.WolfStats> wolvesStats;
    public int selectedDifficulty;

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
}
