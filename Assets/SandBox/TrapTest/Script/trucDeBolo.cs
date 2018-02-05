using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trucDeBolo : MonoBehaviour
{
    private float i = 0;
	// Use this for initialization
	void Start ()
	{
	    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 1);

	}
	
	// Update is called once per frame
	void Update ()
	{
	    i += Time.deltaTime;
        if(Math.Abs(i%5) <= 0.04) 
	    	transform.position = new Vector3(0,0.15f,0);
	}
}
