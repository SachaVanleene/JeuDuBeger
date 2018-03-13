using System;
using System.Collections.Generic;

[Serializable]
public class AchievementInfo {
    public string Name { get; set; }
    public string InfoText { get; set; }
    public Dictionary<AchievementEvent, int> completion { get; set; }
    public Dictionary<AchievementEvent, int> Aim { get; set; }
    public List<AchievementEvent> EventsToListen;
    // NOTE : the image to be displayed must match the name of the AchievementInfo
    public AchievementInfo(string name, string info, List<AchievementEvent> events, Dictionary<AchievementEvent, int> aim,
        Dictionary<AchievementEvent, int> startedCompletion)
    {
        Name = name;
        InfoText = info;
        Aim = aim;
        completion = startedCompletion;
        EventsToListen = events;
        foreach(var ev in events)
        {
            SProfilePlayer.getInstance().AchievementsManager.Subscribe(ev, this);
        }
        SProfilePlayer.getInstance().AchievementsManager.AddAchievement(this);
    }
    public void AddStep(AchievementEvent ev, int step = 1)
    {
        completion[ev] += step;
        if(completion[ev] >= Aim[ev])
        {
            SProfilePlayer.getInstance().AchievementsManager.Unsubscribe(ev, this);
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
}
