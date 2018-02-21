using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {

    public bool aiming;
    public bool canRun;
    public bool walk;
    public bool shoot;
    public bool reloading;
    public bool onGround;

    public float horizontal;
    public float vertical;
    public Vector3 lookPosition;
    public Vector3 lookHitPosition;
    public LayerMask layerMask;

    //public CharacterAudioManager audioManager;

    //[HideInInspector]
    //public HandleShooting handleShooting;

    //[HideInInspector]
    //public HandleAnimations handleAnim;


    // Use this for initialization
    void Start () {
		
	}
	
	void FixedUpdate () {
        onGround = IsOnGround();	
	}

    bool IsOnGround()
    {
        bool retVal = false;

        Vector3 origin = transform.position + new Vector3(0, 0.05f, 0);
        RaycastHit hit;

        if (Physics.Raycast(origin, -Vector3.up, out hit, 0.5f, layerMask))
        {
            retVal = true;
        }

        return retVal;
    }
}
