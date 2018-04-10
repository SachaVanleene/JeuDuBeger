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
       // Wolf Killer
        new AchievementInfo(
            name : "Tueur",
            info : "Tuer 10 loups",
            events : new List<AchievementEvent>() { AchievementEvent.wolfDeath },
            aim : new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.wolfDeath, 10 } },
            manager : AchievementsManager
        );
        // Player Kills
        new AchievementInfo(
            name: "Tendances Suicidaires",
            info: "Le joueur meurt 10 fois",
            events: new List<AchievementEvent>() { AchievementEvent.playerDeath },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.playerDeath, 10 } },
            manager: AchievementsManager
        );
        // Sheep death
        new AchievementInfo(
            name: "Beeeh",
            info: "Vous perdez 20 moutons",
            events: new List<AchievementEvent>() { AchievementEvent.sheepDeath },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.sheepDeath, 20 } },
            manager: AchievementsManager
        );
        // test many inputs
        new AchievementInfo(
            name: "Fiesta !!!",
            info: "Tuez 10 loups, mourez 5 fois et perdez 10 moutons",
            events: new List<AchievementEvent>() { AchievementEvent.sheepDeath, AchievementEvent.playerDeath, AchievementEvent.wolfDeath },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.sheepDeath, 10 },
                                                                                { AchievementEvent.playerDeath, 10 },
                                                                                { AchievementEvent.wolfDeath, 10 }},
            manager: AchievementsManager
        );
        // quit a game
        new AchievementInfo(
            name: "Quitter",
            info: "You quit befor losing all your sheep, you coward !",
            events: new List<AchievementEvent>() { AchievementEvent.quit },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.quit, 1 } },
            manager: AchievementsManager
        );
        // lose a game
        new AchievementInfo(
            name: "Looser",
            info: "You lose a game ... but it's not like you could win one.",
            events: new List<AchievementEvent>() { AchievementEvent.lose },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.lose, 1 } },
            manager: AchievementsManager
        );
        // use cheats in a game
        new AchievementInfo(
            name: "Cheater",
            info: "You cheat ! Yeah that happens more that you could imagine",
            events: new List<AchievementEvent>() { AchievementEvent.cheat },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.cheat, 1 } },
            manager: AchievementsManager
        );
        // passed the first round
        new AchievementInfo(
            name: "Newbie",
            info: "You survived your first night out, congrats, more are comming",
            events: new List<AchievementEvent>() { AchievementEvent.cycleEnd },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.cycleEnd, 1 } },
            manager: AchievementsManager
        );
        // created his profile
        new AchievementInfo(
            name: "Player",
            info: "You did it, u successfully created your first profile !!!",
            events: new List<AchievementEvent>() { AchievementEvent.createProfile },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.createProfile, 1 } },
            manager: AchievementsManager
        );
        // spend 200 golds
        new AchievementInfo(
            name: "Jobs maker",
            info: "You injected a lot of money on local companies",
            events: new List<AchievementEvent>() { AchievementEvent.goldSpent },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.goldSpent, 200 } },
            manager: AchievementsManager
        );
        // earn 200 golds
        new AchievementInfo(
            name: "Riche",
            info: "You earned a lot of money boy",
            events: new List<AchievementEvent>() { AchievementEvent.goldEarn },
            aim: new System.Collections.Generic.Dictionary<AchievementEvent, int>() { { AchievementEvent.goldEarn, 200 } },
            manager: AchievementsManager
        );
    }
}
