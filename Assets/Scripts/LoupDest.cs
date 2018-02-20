using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoupDest : MonoBehaviour {

    private bool occuped;
    private GameObject enclosParent;

    // Use this for initialization
    void Start () {
        occuped = false;
        enclosParent = this.transform.parent.gameObject;
        //print(enclosParent);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // GETTER
    public bool GetStatus() {
        return occuped;
    }
    public bool GetEnclosParent() {
        return enclosParent;
    }

    // SETTER
    public void SetStatus(bool status) {
        occuped = status;
    }
}