using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA_Wolves_Path : MonoBehaviour {


    private Animator anim;
    private NavMeshAgent agent;
    public Transform targetTransform; //the zombie's target
    public string targetTag;
    private bool targetInRange;
    float timer;
    float timeBetweenAttacks;
    int damage;
    public delegate void TriggerTag();
    public TriggerTag function;
    bool moving;


    public GameObject fakenclos;

    private void Awake()
    {
        timeBetweenAttacks = 2f;
        timer = 0f;
        damage = 10;
        targetTransform = null;
        moving = false;
    }


    // Use this for initialization
    void Start () {
        function += GetComponent<IA_Wolves_Attack>().updateTarget;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        updateTarget(fakenclos.transform);
        agent.Warp(this.gameObject.transform.position);
	}

    public void updateTarget(Transform target)
    {
        if (target == null) // Remise en idle
        {
            targetTransform = target;
            targetTag = "Aucune";
            targetInRange = false;
        }else
        {
            if (targetTransform == null) // Théoriquement en idle
            {
                targetTransform = target;
                targetTag = target.gameObject.tag;
            }
            else //Attaque ou 
            {
                targetTransform = target;
                targetTag = target.gameObject.tag;
            }
        }
        function.Invoke();

    }

    void moveToTarget()
    {
        if (targetTransform != null && !targetInRange)
        {
            moving = true;
            anim.SetBool("Moving", moving);
            agent.SetDestination(targetTransform.position);
        }
        if(targetInRange && moving)
        {
            moving = false;
            anim.SetBool("Moving", moving);
            agent.SetDestination(transform.position);
        }
        if(targetTag == "Aucune")
        {
            moving = false;
            anim.SetBool("Moving", moving);
            agent.SetDestination(transform.position);
        }
    }

   


    void FixedUpdate()
    {
        //updateTarget(fakenclos.transform);
        moveToTarget();
    }


    public void updateRange()
    {
        targetInRange = GetComponent<IA_Wolves_Attack>().targetInRange;
    }

   /* public Transform detectTarget()
    {
    // fences
    //Script EnclosManager
    // Script 
    }*/ 

}
