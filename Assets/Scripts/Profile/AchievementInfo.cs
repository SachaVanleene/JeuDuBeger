using Assets.Script.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AchievementInfo {
    public string Name { get; set; }
    public string InfoText { get; set; }
    public Dictionary<AchievementEvent, int> Aim { get; set; }
    public List<AchievementEvent> EventsToListen;
    [NonSerialized]
    private AchievementsManager manager; // is reset when loaded to prevent loop while serialize
    // NOTE : the image to be displayed must match the name of the AchievementInfo
    public AchievementInfo(string name, string info, List<AchievementEvent> events, 
        Dictionary<AchievementEvent, int> aim, AchievementsManager manager)
    {
        Name = name;
        InfoText = info;
        Aim = aim;
        EventsToListen = events;
        this.manager = manager;

        foreach (var ev in events)
        {
            manager.Subscribe(ev, this);
        }
        manager.AddAchievement(this);
    }
    public bool EventNotification(AchievementEvent ev)
    {   // return value indicate if the achievement must unsubscrib from the event
        if (manager.Completion[ev] >= Aim[ev])
        {
            return true;
        }
        return false;
    }
    public bool IsComplete()
    {
        foreach(var e in EventsToListen)
        {
            if(Aim[e] > manager.Completion[e])
            {
                return false;
            }
        }
        return true;
    }
    public void SetManager(AchievementsManager man)
    {
        manager = man;
    }
    public int[] getCompletion(AchievementEvent ev)
    {
        int[] values = new int[2];
        values[0] = manager.Completion[ev];
        values[1] = this.Aim[ev];
        return values;
    }
}
