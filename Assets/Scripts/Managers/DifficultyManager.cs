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
        foreach (SO.WolfStats wolfStats in wolvesStats)
        {
            Debug.Log("allo");
            wolfStats.SetDifficulty(difficultySettings, selectedDifficulty);
        }
    }
}
