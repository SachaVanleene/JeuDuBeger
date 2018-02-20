using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private int health;
    public bool alive;

    private void Awake()
    {
        health = 100;
        alive = true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void takeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            Destroy(gameObject);
        }

    }
}
