using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfHealth : MonoBehaviour
{

    private Animator anim;
    private ParticleSystem cloud;
    private int health;
    private bool alive;

    private void Awake()
    {
        health = 100;
        anim = GetComponent<Animator>();
        cloud = GetComponentInChildren<ParticleSystem>();
        alive = true;
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        //anim.SetTrigger("Hit");

        if (health < 0 && alive)
        {
            alive = false;
            GetComponent<IA_Wolves_Path>().updateTarget(null);
            anim.SetTrigger("dead");
            Destroy(gameObject, 2.5f);
        }

    }

    public void setHealth(int newHealth)
    {
        health = newHealth;
    }

    public int getHealth()
    {
        return health;
    }
}

