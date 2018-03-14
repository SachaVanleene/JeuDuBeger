using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SProfilePlayer
{
    static private SProfilePlayer instance = null;
    public string Name { get; set; }
    public int Difficulty { get; set; }
    public AchievementsManager AchievementsManager { get; set; }
    
    public SProfilePlayer(string name = "<Default>")
    {
        this.Name = name;
        this.AchievementsManager = new AchievementsManager();
        this.init();
    }
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
        new AchievementInfo(
            name : "Tueur",
            info : "Tuer 10 loups",
            events : new List<AchievementEvent>() { AchievementEvent.wolfDeath },
            aim : new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.wolfDeath, 10 } },
            startedCompletion : new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.wolfDeath, 0 } },
            manager : AchievementsManager
        );
        // Player Kills
        new AchievementInfo(
            "Tendances Suicidaires",
            "Le joueur meurt 10 fois",
            new List<AchievementEvent>() { AchievementEvent.playerDeath },
            new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.playerDeath, 10 } },
            new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.playerDeath, 0 } },
            AchievementsManager
        );
        // Sheep death
        new AchievementInfo(
            "Beeeh",
            "Vous perdez 20 moutons",
            new List<AchievementEvent>() { AchievementEvent.sheepDeath },
            new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.sheepDeath, 20 } },
            new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.sheepDeath, 0 } },
            AchievementsManager
        );
        new AchievementInfo(
            "Fiesta !!!",
            "Tuez 10 loups, mourez 5 fois et perdez 10 moutons",
            new List<AchievementEvent>() { AchievementEvent.sheepDeath },
            new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.sheepDeath, 10 },
                                                                                { AchievementEvent.playerDeath, 10 },
                                                                                { AchievementEvent.wolfDeath, 10 }},
            new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.sheepDeath, 0 },
                                                                                { AchievementEvent.playerDeath, 0 },
                                                                                { AchievementEvent.wolfDeath, 0 }},
            AchievementsManager
        );
        // etc
    }
}
