﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enclosures;
//using Assets.Scripts.Enclosures;
using UnityEngine;
using UnityEngine.AI;


public class IA_Wolves_Path : MonoBehaviour {


    private Animator anim;
    private NavMeshAgent agent;


    public Transform targetTransform; //Wolf target
    public string targetTag;
    private bool targetInRange;
    float timer;

    float timeBetweenAttacks;
    int damage;

    public delegate void TriggerTag();
    public TriggerTag function;

    bool moving;

    GameObject[] enclos;


    public GameObject fakenclos;

    private void Awake()
    {
        timeBetweenAttacks = 2f;
        timer = 0f;
        damage = 10;
        targetTransform = null;
        moving = false;
        enclos = GameObject.FindGameObjectsWithTag("Enclos");
    }

    void Update()
    {
        if (enclos.Length == 0)
        {
            enclos = GameObject.FindGameObjectsWithTag("Enclos");   
            if(enclos.Length !=0)
                GetTargetEnclos();
        }
    }

    // Use this for initialization
    void Start () {
        function += GetComponent<IA_Wolves_Attack>().updateTarget;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        //updateTarget(fakenclos.transform);
        agent.Warp(this.gameObject.transform.position);
        NavMesh.pathfindingIterationsPerFrame = 500;
       // GetTargetEnclos();
	}

    public void updateTarget(Transform target)
    {
        
        if (target!=null){
           // Debug.LogError("Position target : " + target.position);
        }
        RealaseBarrer();
        RealeaseDlegate();
        targetInRange = false; // Si nouvelle target supposé qu'elle n'est pas en rnage sinon bug dans les invoke
        GetComponent<IA_Wolves_Attack>().targetInRange = false;
        if (target == null) // Remise en idle
        {
            targetTransform = target;
            targetTag = "Aucune";
            targetInRange = false;
        }else
        {
            targetTransform = target;
            targetTag = target.gameObject.tag;
            SubscribeDelegate();
        }
        function.Invoke();

    }

    void moveToTarget()
    {
        if (targetTransform != null && !targetInRange) //Si a effectivement une cible et qu'elle n'est pas en rnage
        {
            moving = true;
            anim.SetBool("Moving", moving);
            if(targetTag == "Fences")
            {
                Vector3 position = targetTransform.position - 2.3f*targetTransform.right; // Sinon le pathfinding ne larchera pas car la zone est no walkabme
                agent.SetDestination(position);
            }
            else
            {
                agent.SetDestination(targetTransform.position);
            }

        }
        if(targetInRange && moving) //Si proche de destination mais encore entrain de bouger
        {
            moving = false;
            anim.SetBool("Moving", moving);
            agent.SetDestination(transform.position);
        }
        if(targetTag == "Aucune" )// SI aucune target
        {
            moving = false;
            anim.SetBool("Moving", moving);
            agent.SetDestination(transform.position);
        }
    }

   


    void FixedUpdate()
    {
        moveToTarget();

    }


    public void updateRange()
    {
        targetInRange = GetComponent<IA_Wolves_Attack>().targetInRange;
    }


    public void GetTargetEnclos()
    {
        GameObject closest_enclos = DetectCLosestEnclos();
        if(closest_enclos != null)
        {
            GameObject barreer = GetBareerFromEnclos(closest_enclos);

            updateTarget(barreer.transform);
        }else
        {
            updateTarget(null);
        }
    }

    public GameObject DetectCLosestEnclos()
    {
        GameObject enclos_target = null;
        float dist_to_target = Mathf.Infinity;
        float current_distance = 0f;
        GameObject current_enclos = null;
        for (int i=0; i< enclos.Length; i++) // On parcoure les enclos pour trouver le plus proche
        {
            current_enclos = enclos[i];

            if (current_enclos.GetComponent<EnclosureScript>().Health > 0)
            {
                Debug.Log(current_enclos);
                current_distance = Vector3.Distance(current_enclos.transform.position, this.gameObject.transform.position);
                if (current_distance < dist_to_target)
                {
                    enclos_target = current_enclos;
                    dist_to_target = current_distance;
                }
            }
        }
        return enclos_target;
    }

    public GameObject GetBareerFromEnclos(GameObject enclos)
    {
        List<GameObject> all_bareers = new List<GameObject>();
        List<GameObject> free_bareers = new List<GameObject>();
        GameObject resu = null;
        foreach (Transform child in enclos.transform)
           {
             if (child.tag == "Fences")
                     {
                all_bareers.Add(child.gameObject);
                if(!child.gameObject.GetComponent<LoupDest>().GetStatus())
                         free_bareers.Add(child.gameObject);
                     }
           }
        Random_List.Shuffle<GameObject>(all_bareers);
        Random_List.Shuffle<GameObject>(free_bareers);
        if (free_bareers.Count > 0)
        {
            resu = free_bareers[0];
            resu.GetComponent<LoupDest>().SetStatus(true);
        } else // Atention si plus de place on essaie quand même 
        {
            resu = all_bareers[Random.Range(0, all_bareers.Count - 1)];
        }
        return resu;
    }

    public void RealaseBarrer()
    {
        if(targetTag == "Fences" && targetTransform != null)
        {
            targetTransform.gameObject.GetComponent<LoupDest>().SetStatus(false);
        }
    }

    public void RealeaseDlegate()
    {
        if (targetTransform != null)
        {
            if (targetTag == "Player")
            {
                targetTransform.gameObject.GetComponent<Player>().RemoveSubscriber(GetTargetEnclos);
            }
            else
            {
                targetTransform.parent.gameObject.GetComponent<EnclosureScript>().RemoveSubscriber(GetTargetEnclos);
            }
        }
    }

    public void SubscribeDelegate()
    {
        if (targetTransform != null)
        {
            if (targetTag == "Player")
            {
                targetTransform.gameObject.GetComponent<Player>().AddSubscriber(GetTargetEnclos);
            }
            else
            {
                targetTransform.parent.gameObject.GetComponent<EnclosureScript>().AddSubscriber(GetTargetEnclos);
            }
        }
    }



}
