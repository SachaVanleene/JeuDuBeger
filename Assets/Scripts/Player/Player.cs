using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public bool alive;
    public GameObject spawn;
    private float respawningTime;
    Animator anim;
    int initHealth;

    public int Gold { get; set; }

    private int health;
    

    private void Awake()
    {
        initHealth = 100;
        health = initHealth;
        alive = true;
        Gold = 100; 
        respawningTime = 7f;
        anim = GetComponent<Animator>();
    }

    public void takeDamage(int dps)
    {
        health -= dps;
        if (health< 0)
        {
            alive = false;
            anim.SetTrigger("dead");
            Debug.LogError("Mort");
            this.GetComponent<PlayerMovement>().function.Invoke();
            StartCoroutine(WaitForSpawning());
        }
    }


    IEnumerator WaitForSpawning()
    {
        yield return new WaitForSeconds(respawningTime);
        alive = true;
        health = initHealth;
        this.GetComponent<PlayerMovement>().function.Invoke();
        anim.SetTrigger("respawn");
        Respawn();
    }
    void Respawn()
    {
        this.gameObject.transform.position = spawn.transform.position;
    }
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
<<<<<<< HEAD
	void Update ()
    {

    }
=======
	void Update () {
		
	}
>>>>>>> master
}
