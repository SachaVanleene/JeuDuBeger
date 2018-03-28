using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfHealth : MonoBehaviour
{

    private Animator anim;
    private ParticleSystem cloud;
    private int health;
    public int health_max;
    public bool alive;

    public SO.WolfStats wolfStats;
    public SO.GameEvent killWolf;

    AudioManagerWolves script_audio;

    UI_Health_Wolves ui_healt;

    public GameObject emptyGoWhoContainBoxCollider;

    private void Awake()
    {
        health = (int) wolfStats.CurrentLife;
        health_max = health;
        //health = 100;
        anim = GetComponent<Animator>();
        cloud = GetComponentInChildren<ParticleSystem>();
        alive = true;
        script_audio = GetComponent<AudioManagerWolves>();
        ui_healt = GetComponent<UI_Health_Wolves>();
    }

    public void takeDamage(int damage, bool hitByWeapon = false)
    {
        if (alive)
        {
            script_audio.PlayHitSound();

            health -= damage;
            //anim.SetTrigger("Hit");

            ui_healt.OnHit();

            if (hitByWeapon)
                GameOverManager.instance.PlayerDamageDealt.Add((health < 0) ? damage + health : damage);
            else
                GameOverManager.instance.TrapsDamageDealt.Add((health < 0) ? damage + health : damage);

            if (health <= 0)
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

                GameOverManager.instance.WolvesAliveInRound.Add(-1);

                Assets.Script.Managers.GameManager.instance.EarnGold((int) wolfStats.CurrentGoldReward);

                killWolf.Raise();

                Assets.Script.Managers.GameManager.instance.EarnGold((int)wolfStats.CurrentGoldReward);


                DisableComponent();
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
        if(transform.tag == "CommonWolf")
        {
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            if(emptyGoWhoContainBoxCollider != null)
            {
                emptyGoWhoContainBoxCollider.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}

