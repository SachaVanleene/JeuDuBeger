using System;
using System.Collections.Generic;

[Serializable]
public class SProfilePlayer
{
    static private SProfilePlayer instance = null;
    public string Name { get; set; }
    public int Difficulty { get; set; }
    public AchievementsManager AchievementsManager { get; set; }

    public static SProfilePlayer getInstance()
    {
        if (instance == null)
        {
            instance = new SProfilePlayer();
        }
        return instance;
    }
    public static void setInstance(SProfilePlayer spp)
    {
        instance = spp;
    }

    public void init()
    {  // create void Achievements
       // Wolf Killer
        AchievementsManager.AddAchievement(new AchievementInfo(
            "Tueur",
            "Tuer 10 loups",
            new List<AchievementEvent>() { AchievementEvent.wolfDeath },
            new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.wolfDeath, 10 } },
            new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.wolfDeath, 0 } }
        ));
        // Player Kills
        AchievementsManager.AddAchievement(new AchievementInfo(
            "Tendances Suicidaires",
            "Le joueur meurt 10 fois",
            new List<AchievementEvent>() { AchievementEvent.playerDeath },
            new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.playerDeath, 10 } },
            new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.playerDeath, 0 } }
        ));
        // Sheep death
        AchievementsManager.AddAchievement(new AchievementInfo(
            "Beeeh",
            "Vous perdez 20 moutons",
            new List<AchievementEvent>() { AchievementEvent.sheepDeath },
            new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.sheepDeath, 20 } },
            new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.sheepDeath, 0 } }
        ));
        // etc
    }
}
