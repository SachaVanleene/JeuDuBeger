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

    override public string ToString()
    {
        string str = "Achievement Manager :";
        foreach(var ev in listeners.Keys)
        {
            str += "\nkey " + ev + " : ";
            foreach(var achInfo in listeners[ev])
            {
                str += achInfo.Name;
            }
        }
        return str;
    }
    
    public AchievementInfo GetAchievementByName(string name)
    {
        foreach(var achInfo in achievements)
        {
            if (achInfo.Name.Equals(name))
                return achInfo;
        }
        return null;
    }

    public List<AchievementInfo> AddStepAchievement(AchievementEvent eventAchievement, int step = 1)
    {
        if (!listeners.ContainsKey(eventAchievement) || listeners[eventAchievement].Count <= 0)
        {
            //Debug.LogWarning("No achievement is listening (anymore ?) to this event : "
            //    + eventAchievement);
            return null;
        }
        Completion[eventAchievement] += step;
        List<AchievementInfo> unsubscribers = new List<AchievementInfo>();
        foreach (var l in listeners[eventAchievement])
        {
            if (l.EventNotification(eventAchievement))
                unsubscribers.Add(l);
        }
            
        List<AchievementInfo> returnValue = new List<AchievementInfo>();
        foreach (var achInfo in unsubscribers)
        {
            if(achInfo.IsComplete())
            {
                returnValue.Add(achInfo);
            }
            this.Unsubscribe(eventAchievement, achInfo);
        }
        return (returnValue.Count > 0) ? returnValue : null;
    }

    public void Subscribe(AchievementEvent listen, AchievementInfo ach)
    {
        if (!listeners.ContainsKey(listen))
        {
            listeners.Add(listen, new List<AchievementInfo>());
            Completion.Add(listen, 0);
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
