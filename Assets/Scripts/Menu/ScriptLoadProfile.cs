using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptLoadProfile : MonoBehaviour {

    public string fullName;
    public ProfileManager manager;
    public void Load()
    {
        manager.LoadProfile(fullName);
    }
}
