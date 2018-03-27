﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfBossHealth : MonoBehaviour
{

    private Animator anim;
    private ParticleSystem cloud;
    private int health;
    private int health_max;
    public bool alive;

    public SO.WolfStats wolfStats;

    UI_Health_Boss script_ui;

    private void Awake()
    {
        health = (int) wolfStats.CurrentLife;
        health_max = health;
        anim = GetComponent<Animator>();
        cloud = GetComponentInChildren<ParticleSystem>();
        script_ui = GetComponent<UI_Health_Boss>();
        alive = true;
    }

    public void takeDamage(int damage, bool hitByWeapon = false)
    {
        if (alive)
        {
            health -= damage;
            //anim.SetTrigger("Hit");
            script_ui.OnHit();

            if (hitByWeapon)
                GameOverManager.instance.PlayerDamageDealt.Add((health < 0) ? damage + health : damage);
            else
                GameOverManager.instance.TrapsDamageDealt.Add((health < 0) ? damage + health : damage);

            if (health <= 0)
            {
                alive = false;
                GetComponent<IA_Boss_Wolves>().DisableCollider();
                GetComponent<IA_Boss_Wolves>().updateTarget(null);
                anim.SetTrigger("dead");
                Destroy(gameObject, 3.75f);

                GameOverManager.instance.Werewolves.Add(1);

                Assets.Script.Managers.GameManager.instance.EarnGold((int)wolfStats.CurrentGoldReward);

                if (hitByWeapon)
                    GameOverManager.instance.WolvesKilledByWeapon.Add(1);
                else
                    GameOverManager.instance.WolvesKilledByTrap.Add(1);
            }
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

    public int GetHealthMax()
    {
        return health_max;
    }

    void DisableComponent()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }
}

