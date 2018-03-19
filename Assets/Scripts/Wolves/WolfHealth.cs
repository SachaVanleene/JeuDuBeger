using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfHealth : MonoBehaviour
{

    private Animator anim;
    private ParticleSystem cloud;
    private int health;
    public bool alive;

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
            if (gameObject.tag == "CommonWolf")
            {
                GetComponent<IA_Common_Wolves>().updateTarget(null);
            }
            if (gameObject.tag == "WaterWolf")
            {
                GetComponent<IA_Water_Wolves>().updateTarget(null);
            }
            if (gameObject.tag == "MoutainWolf")
            {
                GetComponent<IA_Moutain_Wolves>().updateTarget(null);
            }
            anim.SetTrigger("dead");
            Destroy(gameObject, 2.5f);
            Assets.Script.Managers.GameManager.instance.DeathWolf(this.gameObject);
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

