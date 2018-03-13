using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ProfileManager : MonoBehaviour {
    static public List<string[]> ProfilesFound;
    public void Awake()
    {
        retreiveSaves();
    }
    private void retreiveSaves()
    {
        ProfilesFound = new List<string[]>();
        try
        {
            foreach (string file in System.IO.Directory.GetFiles("./saves/", "*.save"))
            // get list of files *.maps in folder
            {
                ProfilesFound.Add(Path.GetFileName(file).Split(new[] { '-' }));
            }
        }
        catch (Exception excp)
        {
            Debug.LogError("no save folder");
        }
    }

    public void LoadProfile(string name)
    {
        if (File.Exists("./maps/" + name))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open("./saves/" + name, FileMode.Open);
            SProfilePlayer.Instance = (SProfilePlayer)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            Debug.LogError("file not found");
        }

    }
    public void SaveProfile()
    {
        if (!File.Exists("./saves"))
            System.IO.Directory.CreateDirectory("./saves");
        FileStream file = File.Create("./saves/" + SProfilePlayer.Instance.Name + "-" + DateTime.Now.ToShortDateString() + ".save");
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, SProfilePlayer.Instance);
        file.Close();
    }

}
