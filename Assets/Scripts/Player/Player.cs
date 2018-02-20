using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public bool alive;

    public int Gold { get; set; }

    private int health;
    

    private void Awake()
    {
        health = 100;
        alive = true;
        Gold = 100; 
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

    }
}
