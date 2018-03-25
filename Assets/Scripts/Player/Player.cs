using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                GameOverManager.instance.DeathCount.Add(1);
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

    [Space]
    [Header("Heal")]
    [Tooltip("Delay during which the player must not receive damage to start healing.")]
    public float recoverDelay;
    [Tooltip("Delay between each tick of heal.")]
    public float healTick;
    [Tooltip("Percent health regen between 0 and 1.")]
    public float healPerTick;
    WaitForSeconds healTickWait;
    [SerializeField]
    bool isHealing;
    float recoverTimer;

    public GameObject damageGO;
    Image damageImage;
    Color temp;
    float imageTimer;

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
    [Space]
    public GameObject goAttachedToModel;

    public void Init()
    {
        maxHealth = 100;
        actualHealth = maxHealth;
        respawnWait = new WaitForSeconds(respawnDelay);
        healTickWait = new WaitForSeconds(healTick);
        spawnPoint.position.Set(spawnPoint.position.x, 1.72f, spawnPoint.position.z);

        anim = GetComponent<Animator>();
        states = GetComponent<TPC.StateManager>();

        Alive = true;
        hMove = GetComponent<TPC.HandleMovement_Player>();

        damageImage = damageGO.GetComponent<Image>();
        recoverTimer = 0.0f;
        isHealing = false;

        temp = damageImage.color;
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

        recoverTimer -= Time.deltaTime;
        
        if (recoverTimer  + recoverDelay < 0 && actualHealth < maxHealth && !isHealing)
        {
            StartCoroutine(Recover());
            imageTimer = 0;
        }

        if (isHealing)
        {
            imageTimer += Time.deltaTime / healTick;
            damageImage.color = Color.Lerp(damageImage.color, temp, imageTimer);
        }

        if (!Alive)
        {
            imageTimer += Time.deltaTime / respawnDelay;
            damageImage.color = Color.Lerp(damageImage.color, temp, imageTimer);
        }
    }



    public void takeDamage(float dps)
    {
        actualHealth -= dps;

        temp = damageImage.color;
        temp.a = 1 - (actualHealth / maxHealth);
        damageImage.color = temp;

        recoverTimer = recoverDelay;

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
        temp.a = 0;
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

    IEnumerator Recover()
    {
        isHealing = true;
        while (actualHealth < maxHealth)
        {
            yield return healTickWait;
            if (recoverTimer - Time.deltaTime < 0)
            {
                actualHealth += maxHealth * healPerTick;
                actualHealth = Mathf.Clamp(actualHealth, 0, maxHealth);
                temp.a = Mathf.Clamp(1 - (actualHealth / maxHealth), 0, 1);
                imageTimer = 0;
            }
            else
            {
                break;
            }
        }
        isHealing = false;
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
