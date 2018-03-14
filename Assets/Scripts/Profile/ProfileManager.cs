using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ProfileManager : MonoBehaviour {
    static public List<string[]> ProfilesFound;
    public List<Texture2D> SpritesAchievements;
    public Texture2D DefaultSprite;
    public Texture2D CompletedSprite;
    public void Awake()
    {
        RetreiveSaves();
    }
    static public void RetreiveSaves()
    {
        ProfilesFound = new List<string[]>();
        try
        {
            foreach (string file in System.IO.Directory.GetFiles("./saves/", "*.save"))
            // get list of files *.maps in folder
            {
                ProfilesFound.Add(Path.GetFileNameWithoutExtension(file).Split(new[] { '-' }));
            }
        }
        catch (Exception excp)
        {
            Debug.LogWarning("no save folder found, create a new one");
            System.IO.Directory.CreateDirectory("./saves");
        }
    }
    static public void DeleteProfile(string name)
    {
        foreach (var profile in ProfilesFound)  
        {
            if (profile[0].Equals(name))
                File.Delete("./saves/" + profile[0] + "-" + profile[1] + ".save");
        }
    }

    static public void CreateProfile(string name)
    {
        //DeleteProfile(name);
        SProfilePlayer.setInstance(new SProfilePlayer(name));
    }
    static public void LoadProfile(string fullName)
    {
        if (File.Exists("./saves/" + fullName))
        {
            //Debug.Log(SProfilePlayer.getInstance().AchievementsManager.GetHashCode());

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open("./saves/" + fullName, FileMode.Open);
            SProfilePlayer.setInstance((SProfilePlayer) bf.Deserialize(file));
            file.Close();
            SProfilePlayer.getInstance().AchievementsManager.ResetManagerOnAchievementInfos();
            /*
            Debug.Log(SProfilePlayer.getInstance().AchievementsManager.getAchivements()[0].getManager().GetHashCode());
            Debug.Log(SProfilePlayer.getInstance().AchievementsManager.GetHashCode());
            Debug.Log(" --- ");*/
        }
        else
        {
            Debug.LogWarning("file not found");
        }
    }
    static public void SaveProfile()
    {
        if (SProfilePlayer.getInstance().Name.Equals("<Default>"))
            return;
        if (!File.Exists("./saves"))
            System.IO.Directory.CreateDirectory("./saves");
        DeleteProfile(SProfilePlayer.getInstance().Name);
        FileStream file = File.Create("./saves/" + SProfilePlayer.getInstance().Name + "-" + 
            DateTime.Now.ToLongDateString().Split(',')[1] + ".save");
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, SProfilePlayer.getInstance());
        file.Close();
    }

}
