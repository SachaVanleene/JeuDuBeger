using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptLoadProfile : MonoBehaviour {

    public string fullName;
    public void Load()
    {
        ProfileManager.LoadProfile(fullName);
    }
}
