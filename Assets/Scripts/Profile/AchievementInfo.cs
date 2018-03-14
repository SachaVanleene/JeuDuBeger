using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AchievementInfo {
    public string Name { get; set; }
    public string InfoText { get; set; }
    public Dictionary<AchievementEvent, int> completion { get; set; }
    public Dictionary<AchievementEvent, int> Aim { get; set; }
    public List<AchievementEvent> EventsToListen;
    [NonSerialized]
    private AchievementsManager manager; // is reset when loaded to prevent loop
    // NOTE : the image to be displayed must match the name of the AchievementInfo
    public AchievementInfo(string name, string info, List<AchievementEvent> events, Dictionary<AchievementEvent, int> aim,
        Dictionary<AchievementEvent, int> startedCompletion, AchievementsManager manager)
    {
        Name = name;
        InfoText = info;
        Aim = aim;
        completion = startedCompletion;
        EventsToListen = events;
        this.manager = manager;

        foreach (var ev in events)
        {
            manager.Subscribe(ev, this);
        }
        manager.AddAchievement(this);
    }
    public void AddStep(AchievementEvent ev, int step = 1)
    {
        completion[ev] += step;
        if(completion[ev] >= Aim[ev])
        {
            manager.Unsubscribe(ev, this);
        }
    }
    public bool IsComplete()
    {
        foreach(var e in EventsToListen)
        {
            if(Aim[e] > completion[e])
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
    // TODO : to be deleted after tests 
    public AchievementsManager getManager()
    {
        return manager;
    }
}
