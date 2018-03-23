using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [HideInInspector]
    public float maxHealth;
    [Header("Info")]
    public float actualHealth;

    [SerializeField]
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



    public int gold;
    public int storedSheeps;

    [Space]
    [Header("Respawn")]
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

    //Freezing System
    bool isFreezing;
    bool froze;
    bool canBeFrozen;

    float timerForLastCallingFreeze;

    float timeNeededToDodgeFreeze;
    float timeNeededForBeingFrozen;
    float freezingDUration;
    float freezingCoolDown;

    float timer;

    Color32 frozenColor = new Color32(54, 167, 204, 255);
    Color32 normalColor = new Color32(255, 255, 255, 255);

    //Reference To Change Color Of The Player
    public GameObject goAttachedToModel;

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

    //FreezingHandler

    private void Awake()
    {
        isFreezing = false;
        froze = false;
        canBeFrozen = true;

        //Timers
        timer = 0f;

        //Limits
        freezingDUration = 5f;
        freezingCoolDown = 7f;
        timeNeededForBeingFrozen = 2f;
        timeNeededToDodgeFreeze = 1f;

    }

    public void Freezing()
    {
        timerForLastCallingFreeze = 0f;
        if (!isFreezing && canBeFrozen)
        {
            isFreezing = true;
            timer = 0f;
        }
    }

    void Frozen()
    {
        Debug.LogError("Frozen");
        if(alive)
        {
            anim.speed = 0;
            hMove.AddVelocity(Vector3.zero, freezingDUration, 0, true);
            goAttachedToModel.GetComponent<Renderer>().material.SetColor("_Color", frozenColor);
            StartCoroutine(WaitForEndOfFreeze());
        }
    }

    IEnumerator WaitForBeingFrozeOnceAgain()
    {
        yield return new WaitForSeconds(freezingCoolDown);
        canBeFrozen = true;
        Debug.LogError("Can be Froze again");
    }

    IEnumerator WaitForEndOfFreeze()
    {
        yield return new WaitForSeconds(freezingDUration);
        UnFrozen();
    }

    void UnFrozen()
    {
        anim.speed = 1f;
        Debug.LogError("UnFrozen");
        goAttachedToModel.GetComponent<Renderer>().material.SetColor("_Color", normalColor);
        froze = false;
        StartCoroutine(WaitForBeingFrozeOnceAgain());
    }

    public void Update()
    {

        timer += Time.deltaTime;
        timerForLastCallingFreeze += Time.deltaTime;
        if(timerForLastCallingFreeze >= timeNeededToDodgeFreeze && isFreezing)
        {
            isFreezing = false;
            Debug.LogError("Remise à 0 du freeze");
        }

            if(timer >= timeNeededForBeingFrozen && canBeFrozen && isFreezing) // Si on a dépassé la liite sous le gel on se freeze
            {
                canBeFrozen = false;
                froze = true;
                isFreezing = false;
                Frozen();
            }
    }



    public void takeDamage(float dps)
    {
        actualHealth -= dps;

        if (actualHealth <= 0 && Alive)
        {
            Alive = false;
            if(froze)
            {
                anim.speed = 1f;
                goAttachedToModel.GetComponent<Renderer>().material.SetColor("_Color", normalColor);
            }
            anim.SetTrigger("dead");
            onTriggerDead.Invoke();
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return respawnWait;
        Alive = true;
        actualHealth = maxHealth;

        transform.position = spawnPoint.position;
        if(onTriggerRespawn != null)
        {
            onTriggerRespawn.Invoke();
        }
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
