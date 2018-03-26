using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfHealth : MonoBehaviour
{

    private Animator anim;
    private ParticleSystem cloud;
    private int health;
    public bool alive;

    public SO.WolfStats wolfStats;

    AudioManagerWolves script_audio;

    private void Awake()
    {
        health = (int) wolfStats.CurrentLife;
        anim = GetComponent<Animator>();
        cloud = GetComponentInChildren<ParticleSystem>();
        alive = true;
        script_audio = GetComponent<AudioManagerWolves>();
    }

    public void takeDamage(int damage, bool hitByWeapon = false)
    {
        script_audio.PlayHitSound();

        health -= damage;
        //anim.SetTrigger("Hit");

        if (hitByWeapon)
            GameOverManager.instance.PlayerDamageDealt.Add((health < 0) ? damage + health : damage);
        else
            GameOverManager.instance.TrapsDamageDealt.Add((health < 0) ? damage + health : damage);

        if (health <= 0 && alive)
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


            GameOverManager.instance.Wolves.Add(1);

            if (hitByWeapon)
                GameOverManager.instance.WolvesKilledByWeapon.Add(1);
            else
                GameOverManager.instance.WolvesKilledByTrap.Add(1);
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

