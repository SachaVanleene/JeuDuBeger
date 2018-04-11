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
    public List<string> TutorialsCalled = new List<string>();

    [NonSerialized]
    public List<Texture2D> SpritesAchievements;
    [NonSerialized]
    public Texture2D DefaultSprite;

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
        // created his profile
        new AchievementInfo(
            name: "Player",
            info: "You did it, u successfully created your first profile !!!",
            events: new List<AchievementEvent>() { AchievementEvent.createProfile },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.createProfile, 1 } },
            manager: AchievementsManager
        );
        // use cheats in a game
        new AchievementInfo(
            name: "Tricheur",
            info: "Oh c'est pas bien... Ca arrive plus souvent que vous le pensez",
            events: new List<AchievementEvent>() { AchievementEvent.cheat },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.cheat, 1 } },
            manager: AchievementsManager
        );
        // spend 200 golds
        new AchievementInfo(
            name: "Créateur d’emplois",
            info: "Vous injectez beaucoup d’argent dans les compagnies locales",
            events: new List<AchievementEvent>() { AchievementEvent.goldSpent },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.goldSpent, 200 } },
            manager: AchievementsManager
        );
        // earn 200 golds
        new AchievementInfo(
            name: "Golden Boy",
            info: "Un berger riche comme vous, c’est pas commun",
            events: new List<AchievementEvent>() { AchievementEvent.goldEarn },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.goldEarn, 200 } },
            manager: AchievementsManager
        );
        // quit a game
        new AchievementInfo(
            name: "Trouillard",
            info: "Vous avez quitté la partie en cours, honte à vous...",
            events: new List<AchievementEvent>() { AchievementEvent.quit },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.quit, 1 } },
            manager: AchievementsManager
        );
        // lose a game
        new AchievementInfo(
            name: "Looser",
            info: "Vous avez perdu... En même temps, c’est pas comme si vous pouviez gagner !",
            events: new List<AchievementEvent>() { AchievementEvent.lose },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.lose, 1 } },
            manager: AchievementsManager
        );
        // passed the first round
        new AchievementInfo(
            name: "Noob",
            info: "Vous avez survécu à la 1ère nuit !! Vous enflammez pas, ce n’est que le début",
            events: new List<AchievementEvent>() { AchievementEvent.cycleEnd },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.cycleEnd, 1 } },
            manager: AchievementsManager
        );
        // test many inputs
        new AchievementInfo(
            name: "Fiesta !!!",
            info: "Tuer 10 loups, mourir 5 fois et perdre 10 moutons",
            events: new List<AchievementEvent>() { AchievementEvent.sheepDeath, AchievementEvent.playerDeath, AchievementEvent.wolfDeath },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.sheepDeath, 10 },
                                                                                { AchievementEvent.playerDeath, 10 },
                                                                                { AchievementEvent.wolfDeath, 10 }},
            manager: AchievementsManager
        );
        // Sheep death
        new AchievementInfo(
            name: "Beeeeeh",
            info: "Perdre 20 moutons",
            events: new List<AchievementEvent>() { AchievementEvent.sheepDeath },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.sheepDeath, 20 } },
            manager: AchievementsManager
        );
        // Player Kills
        new AchievementInfo(
            name: "Tendances Suicidaires",
            info: "Mourir 10 fois",
            events: new List<AchievementEvent>() { AchievementEvent.playerDeath },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.playerDeath, 10 } },
            manager: AchievementsManager
        );
        // Wolf Killer
        new AchievementInfo(
            name : "Tueur",
            info : "Tuer 10 loups",
            events : new List<AchievementEvent>() { AchievementEvent.wolfDeath },
            aim : new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.wolfDeath, 10 } },
            manager : AchievementsManager
        );
    }
}