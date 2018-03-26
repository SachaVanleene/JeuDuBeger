using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enclosures;
using UnityEngine;
using UnityEngine.AI;

public class IA_Common_Wolves : MonoBehaviour {

    //Variable using for anim management and attack system
    private Animator anim;
    bool moving;
    bool isAttacking;
    bool targetAlive;
    float anim_time; // time of anim where it attack
    float timer;
    bool focusingPlayer;
    GameObject player;

    //IA variable
    private NavMeshAgent agent;

    //Variables for target detection system, and target management
    GameObject[] enclos;
    bool enclosFound;
    public Transform targetTransform; //Wolf target
    public string targetTag;
    private bool targetInRange;

    //Variable describing stats of the wolf
    public SO.WolfStats stats;
    float timeBetweenAttacks;
    float playerDamage;
    float enclosureDamage;

    //Variable for looking to target
    private Quaternion lookRotation;
    private Vector3 direction;
    float rotationSpeed;

    //audio
    AudioManagerWolves script_audio;

    private void Awake()
    {
        timer = 0f;
        //Characteristics of Common WOlves
        timeBetweenAttacks = 0.834f; // time between attack 
        playerDamage = stats.CurrentPlayerDamage;
        enclosureDamage = stats.CurrentEnclosureDamage;
        anim_time = 0.5f;
        rotationSpeed = 2f;

        //Initial set up
        targetTransform = null;
        moving = false;
        isAttacking = false;;

        focusingPlayer = false;
        player = GameObject.FindGameObjectWithTag("Player");
        enclosFound = false;
    }


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.Warp(this.gameObject.transform.position);
        NavMesh.pathfindingIterationsPerFrame = 500;
        enclos = GameObject.FindGameObjectsWithTag("Enclos");
        GetTargetEnclos();
        script_audio = GetComponent<AudioManagerWolves>();
    }

    public void updateTarget(Transform target)
    {
        RealaseBarrer();
        RealeaseDlegate();
        targetInRange = false; // Si nouvelle target supposé qu'elle n'est pas en rnage sinon bug dans les invoke
        if (target == null) // Remise en idle
        {
            targetTransform = target;
            targetTag = "Aucune";
            targetInRange = false;
        }
        else { 
            targetTransform = target;
            targetTag = target.gameObject.tag;
            SubscribeDelegate();
        }

    }

    void moveToTarget()
    {
        HandleMove();
    }

    void HandleMove()
    {
        if (targetTransform != null && !targetInRange) //Si a effectivement une cible et qu'elle n'est pas en rnage
        {
            moving = true;
            anim.SetBool("Moving", moving);
            if (targetTag == "Fences")
            {
                Vector3 position = targetTransform.position - 2.3f * targetTransform.right; // Sinon le pathfinding ne marchera pas car la zone est no walkabme
                agent.SetDestination(position);
            }
            else
            {
                agent.SetDestination(targetTransform.position);
            }

        }
        if (targetInRange && moving) //Si proche de destination mais encore entrain de bouger
        {
            moving = false;
            anim.SetBool("Moving", moving);
            agent.SetDestination(transform.position);
        }
        if (targetTag == "Aucune")// SI aucune target
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



    // Get a tagret from an enclos which is alive
    public void GetTargetEnclos()
    {
        focusingPlayer = false;
        GameObject closest_enclos = DetectCLosestEnclos();
        if (closest_enclos != null)
        {
            GameObject barreer = GetBareerFromEnclos(closest_enclos);

            updateTarget(barreer.transform);
        }
        else
        {
            updateTarget(null);
        }
    }

    //Detect closest enclos which is alive
    public GameObject DetectCLosestEnclos()
    {
        GameObject enclos_target = null;
        float dist_to_target = Mathf.Infinity;
        float current_distance = 0f;
        GameObject current_enclos = null;
        for (int i = 0; i < enclos.Length; i++) // On parcoure les enclos pour trouver le plus proche
        {
            current_enclos = enclos[i];
            if (current_enclos.GetComponent<EnclosureScript>().Health > 0)
            {
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


    // Get a barreer from the preivous enclos found and lock this bareer if it is free. If every barreer are not availbale get one using random way
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
                if (!child.gameObject.GetComponent<LoupDest>().GetStatus())
                    free_bareers.Add(child.gameObject);
            }
        }
        Random_List.Shuffle<GameObject>(all_bareers);
        Random_List.Shuffle<GameObject>(free_bareers);
        if (free_bareers.Count > 0)
        {
            resu = free_bareers[0];
            resu.GetComponent<LoupDest>().SetStatus(true);
        }
        else // Atention si plus de place on essaie quand même 
        {
            resu = all_bareers[Random.Range(0, all_bareers.Count - 1)];
        }
        return resu;
    }

    //ReleaseBareer
    public void RealaseBarrer()
    {
        if (targetTag == "Fences" && targetTransform != null)
        {
            targetTransform.gameObject.GetComponent<LoupDest>().SetStatus(false);
        }
    }

    //Realease Deleagate when switching target
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

    //Subscribe to target to be alert when our target died to switch
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

    //Check if we are close to our target using colider
    void OnTriggerEnter(Collider other)
    {
        if (targetTransform != null)
        {
            if (GameObject.ReferenceEquals(targetTransform.gameObject,other.gameObject))
            {
                targetInRange = true;
                HandleMove();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (targetTransform != null)
        {
            if (GameObject.ReferenceEquals(targetTransform.gameObject, other.gameObject))
            {
                if (!targetInRange)
                {
                    targetInRange = true;
                    HandleMove();
                }
            }
        }
    }

    //If we exit collider of  our target then it is not in range anymore
    void OnTriggerExit(Collider other)
    {
        if (targetTransform != null)
        {
            if (GameObject.ReferenceEquals(targetTransform.gameObject, other.gameObject))
            {
                targetInRange = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!enclosFound)
        {
            enclos = GameObject.FindGameObjectsWithTag("Enclos");
            if (enclos != null)
            {
                enclosFound = true;
                GetTargetEnclos();
            }
        }
        timer += Time.deltaTime;
        if (true & targetTransform != null)
        {
            //look at target
            //find the vector pointing from our position to the target
            direction = (targetTransform.position - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            lookRotation = Quaternion.LookRotation(direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            if (isAttacking && anim.GetCurrentAnimatorStateInfo(0).IsName("Wolf_Layer.Attack Jump") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > anim_time) // attaquer au bon moment de l'naimation
            {
                Attack();
                isAttacking = false;
            }
            if (targetTag == "Fences")
            {
                targetAlive = (targetTransform.parent.gameObject.GetComponent<EnclosureScript>().Health > 0);
            }
            else
            {
                targetAlive = targetTransform.gameObject.GetComponent<Player>().Alive;
            }
            if ((timer >= timeBetweenAttacks) && targetInRange && !isAttacking && targetTag != "Aucune" && targetAlive)
            {
                anim.SetTrigger("attack");
                isAttacking = true;
                timer = 0f;
            }
        }
    }

    void Attack()
    {
        script_audio.PlayAttackCommonWolvesSound();
        if (targetTag == "Player")
        {
            targetTransform.gameObject.GetComponent<Player>().takeDamage(playerDamage);
        }
        if (targetTag == "Fences")
        {
            targetTransform.parent.gameObject.GetComponent<EnclosureScript>().DamageEnclos(enclosureDamage);
        }
    }

    public void focusPlayer()
    {
        if (!focusingPlayer)
        {
            focusingPlayer = true;
            updateTarget(player.transform);
        }
    }
}
