using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [HideInInspector]
    public int maxHealth;
    public int actualHealth;

    bool alive;
    public bool Alive
    {
        get
        {
            return alive;
        }
        set
        {
            alive = value;
            states.alive = alive;
            anim.SetBool("alive", alive);

            if (!alive)
            {
                hMove.AddVelocity(Vector3.zero, respawnDelay, 0, true);
                anim.SetTrigger("deathMoment");  
            }

        }
    }
    
    [Space]
    public Transform spawnPoint;
    public float respawnDelay;
    WaitForSeconds respawnWait;

    Animator anim;
    TPC.StateManager states;
    TPC.HandleMovement_Player hMove;

    //IA Subscribers
    public delegate void onDead();
    public onDead onTriggerDead; //Prévenir touts les loups que je suis mort

    public delegate void onRespawn();
    public onRespawn onTriggerRespawn; //Prévenir touts les loups que je suis en vie

    public void Init()
    {
        maxHealth = 100;
        actualHealth = maxHealth;
        respawnWait = new WaitForSeconds(respawnDelay);
        spawnPoint.position.Set(spawnPoint.position.x, 1.72f, spawnPoint.position.z);

        anim = GetComponent<Animator>();
        states = GetComponent<TPC.StateManager>();

        Alive = true;
        hMove = GetComponent<TPC.HandleMovement_Player>();
    }

    public void takeDamage(int dps)
    {
        actualHealth -= dps;

        if (actualHealth <= 0)
        {
            Alive = false;
            anim.SetTrigger("dead");
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return respawnWait;
        Alive = true;
        actualHealth = maxHealth;

        transform.position = spawnPoint.position;
        onTriggerRespawn.Invoke();
        anim.SetTrigger("respawn");
    }

    #region IA Subscribers
    public void AddSubscriber(Player.onDead function)
    {
        onTriggerDead += function;
    }

    public void RemoveSubscriber(Player.onDead function)
    {
        onTriggerDead -= function;
    }

    public void AddSubscriberRespawn(Player.onRespawn function)
    {
        onTriggerRespawn += function;
    }

    public void RemoveSubscriberRespawn(Player.onRespawn function)
    {
        onTriggerRespawn -= function;
    }
    #endregion
}
