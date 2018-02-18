using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfHealth : MonoBehaviour
{

    private Animator anim;
    private ParticleSystem cloud;
    private int health;

    private void Awake()
    {
        health = 100;
        anim = GetComponent<Animator>();
        cloud = GetComponentInChildren<ParticleSystem>();
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        anim.SetTrigger("Hit");

        if (health < 0)
        {
            anim.SetTrigger("Die");
            Destroy(gameObject, 2.5f);
        }

    }
}

