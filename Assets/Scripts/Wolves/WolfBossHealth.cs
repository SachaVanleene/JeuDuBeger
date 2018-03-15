using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBossHealth : MonoBehaviour
{

    private Animator anim;
    private ParticleSystem cloud;
    private int health;
    public bool alive;

    private void Awake()
    {
        health = 300;
        anim = GetComponent<Animator>();
        cloud = GetComponentInChildren<ParticleSystem>();
        alive = true;
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        //anim.SetTrigger("Hit");

        if (health < 0)
        {
            alive = false;
            GetComponent<IA_Boss_Wolves>().DisableCollider();
            GetComponent<IA_Boss_Wolves>().updateTarget(null);
            anim.SetTrigger("dead");
            Destroy(gameObject, 3.75f);
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

