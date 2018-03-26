using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptLoadProfile : MonoBehaviour {

    public string fullName;
    public ProfileManager manager;
    public ListBehaviour list;
    public void Load()
    {
        list.ResetList();
        manager.LoadProfile(fullName);
    }
}
