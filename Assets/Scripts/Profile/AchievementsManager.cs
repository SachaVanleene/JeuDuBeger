using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AchievementsManager {
    // list of achievements
    private List<AchievementInfo> achievements;
    // which achievement listen to which event
    private Dictionary<AchievementEvent, List<AchievementInfo>> listeners;
    public Dictionary<AchievementEvent, int> Completion;

    public AchievementsManager()
    {
        this.achievements = new List<AchievementInfo>();
        this.listeners = new Dictionary<AchievementEvent, List<AchievementInfo>>();
        this.Completion = new Dictionary<AchievementEvent, int>();
    }

    public void AddStepAchievement(AchievementEvent eventAchievement, int step = 1)
    {
        if (!listeners.ContainsKey(eventAchievement))
        {
            Debug.LogError("Trying to call a not existing achievement : "
                + eventAchievement);
            return;
        }
        Completion[eventAchievement] += step;
        foreach (var l in listeners[eventAchievement])
            l.EventNotification(eventAchievement);
    }

    public void Subscribe(AchievementEvent listen, AchievementInfo ach)
    {
        if (!listeners.ContainsKey(listen))
        {
            listeners.Add(listen, new List<AchievementInfo>());
            Completion.Add(listen, 0);
            return;
        }
        listeners[listen].Add(ach);
    }
    public void Unsubscribe(AchievementEvent listen, AchievementInfo ach)
    {
        var list = listeners[listen];
        list.Remove(ach);
        if (list.Count <= 0)
            listeners.Remove(listen);
        else
            listeners[listen] = list;
    }
    public List<AchievementInfo> getAchivements()
    {
        return achievements;
    }
    public void AddAchievement(AchievementInfo ach)
    {
        achievements.Add(ach);
    }
    public void RemoveAchievement(AchievementInfo ach)
    {
        achievements.Remove(ach);
    }
    public void ResetManagerOnAchievementInfos()
    {
        foreach(var achInf in achievements)
        {
            achInf.SetManager(this);
        }
    }
}
