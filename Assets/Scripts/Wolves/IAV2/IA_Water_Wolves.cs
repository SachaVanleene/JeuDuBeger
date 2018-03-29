using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enclosures;
using UnityEngine;

public class IA_Water_Wolves : MonoBehaviour {

    //Variable for anim management and attack system
    private Animator anim;
    float anim_time; // time of anim where it attack
    float timer;
    bool moving;
    bool isAttacking;
    bool targetAlive;
    bool alreadyFocusingAFence;
    SphereCollider firstSPhere;
    public bool focusingPlayer;
    bool focusingLeurre;
    GameObject player;

    //IA variables
    private UnityEngine.AI.NavMeshAgent agent;

    //Variable for target management
    GameObject[] enclos;
    bool enclosFound;
    public GameObject fakenclos;
    public Transform targetTransform; //Wolf target
    public string targetTag;
    private bool targetInRange;

    //Variable for wolf stats
    public SO.WolfStats stats;
    float timeBetweenAttacks;
    float Playerdamage;
    float enclosureDamage;

    //Variable for looking to target
    private Quaternion lookRotation;
    private Vector3 direction;
    float rotationSpeed;

    //Variables for particle system
    private ParticleSystem waterJet;
    public GameObject jets;

    private void Awake()
    {
        timer = 0f;
        //Characteristics of wolf
        timeBetweenAttacks = 0.833f; // time between attack 
        Playerdamage = stats.CurrentPlayerDamage;
        enclosureDamage = stats.CurrentEnclosureDamage;
        anim_time = 0.5f;
        rotationSpeed = 4f;

        //Initial set up
        targetTransform = null;
        moving = false;
        isAttacking = false;
        targetTag = "Aucune";

        alreadyFocusingAFence = false;
        firstSPhere = GetComponent<SphereCollider>();
        focusingPlayer = false;
        focusingLeurre = false;

        player = GameObject.FindGameObjectWithTag("Player");
        enclosFound = false;
    }

    // Use this for initialization
    void Start()
    {
        waterJet = jets.GetComponentInChildren<ParticleSystem>();
        anim = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.Warp(this.gameObject.transform.position);
        UnityEngine.AI.NavMesh.pathfindingIterationsPerFrame = 500;
        enclos = GameObject.FindGameObjectsWithTag("Enclos");
        GetTargetEnclos();
    }

    public void updateTarget(Transform target)
    {
        agent.updateRotation = true;
        waterJet.Stop();
        RealaseBarrer();
        RealeaseDlegate();
        targetInRange = false; // Si nouvelle target supposé qu'elle n'est pas en rnage sinon bug dans les invoke
        alreadyFocusingAFence = false;
        if (target == null) // Remise en idle
        {
            targetTransform = target;
            targetTag = "Aucune";
            targetInRange = false;
            anim.SetBool("attaque", false);
        }
        else
        {
            targetTransform = target;
            targetTag = target.gameObject.tag;
            SubscribeDelegate();
        }
        jets.GetComponent<Water_Wolves_ColliderSystem>().updateParameter();
        EnableFirstSPhere();
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
                Vector3 position = targetTransform.position - 2.3f * targetTransform.right; // Sinon le pathfinding ne larchera pas car la zone est no walkabme
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
        if (targetTag == "Aucune" && GetComponent<WolfHealth>().alive)// SI aucune target
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
        if (focusingPlayer)
        {
            focusingPlayer = false;
        }
        if (focusingLeurre)
        {
            focusingLeurre = false;
        }
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
            else if (targetTag == "Fences")
            {
                targetTransform.parent.gameObject.GetComponent<EnclosureScript>().RemoveSubscriber(GetTargetEnclos);
            }
            else if (targetTag == "Leurre")
            {
                targetTransform.parent.gameObject.GetComponent<Leurre>().RemoveSubscriber(GetTargetEnclos);
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
            else if (targetTag == "Fences")
            {
                targetTransform.parent.gameObject.GetComponent<EnclosureScript>().AddSubscriber(GetTargetEnclos);
            }
            else if (targetTag == "Leurre")
            {
                targetTransform.parent.gameObject.GetComponent<Leurre>().AddSubscriber(GetTargetEnclos);
            }
        }
    }

    //Check if we are close to our target using colider
    void OnTriggerEnter(Collider other)
    {
        if (targetTransform != null)
        {
            if (other.gameObject.tag == targetTag )
            {
                if(targetTag == "Player")
                {
                    targetInRange = true;
                    HandleMove();
                    firstSPhere.enabled = false;
                }
                else // fences case
                {
                    if (other.transform.IsChildOf(targetTransform.parent) && targetTag == "Fences")
                    {
                        targetTransform = other.transform;
                        targetInRange = true;
                        HandleMove();
                        firstSPhere.enabled = false;
                    }
                    if (targetTag == "Leurre")
                    {
                        targetInRange = true;
                        HandleMove();
                        firstSPhere.enabled = false;
                    }
                }

            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (targetTransform != null)
        {
            if (other.gameObject.tag == targetTag)
            {
                if (targetTag == "Player")
                {
                    targetInRange = true;
                    HandleMove();
                    firstSPhere.enabled = false;
                }
                else // fences case
                {
                    if (other.transform.IsChildOf(targetTransform.parent) && targetTag == "Fences")
                    {
                        targetTransform = other.transform;
                        targetInRange = true;
                        HandleMove();
                        firstSPhere.enabled = false;
                    }
                    if (targetTag == "Leurre")
                    {
                        targetInRange = true;
                        HandleMove();
                        firstSPhere.enabled = false;
                    }
                }

            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!enclosFound)
        {
            enclos = GameObject.FindGameObjectsWithTag("Enclos");
            if(enclos != null && !focusingLeurre && !focusingPlayer)
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

            if (targetTag == "Fences")
            {
                targetAlive = (targetTransform.parent.gameObject.GetComponent<EnclosureScript>().Health > 0);
            }
            else if (targetTag == "Player")
            {
                targetAlive = targetTransform.gameObject.GetComponent<Player>().Alive;
            }
            else if (targetTag == "Leurre")
            {
                targetAlive = targetTransform.parent.gameObject.GetComponent<Leurre>().alive;
            }
            if (targetInRange && targetTag != "Aucune" && targetAlive)
            {
                // Debug.LogError("Attaque");
                anim.SetBool("attaque", true);
                waterJet.Play();
                agent.updateRotation = false;
            }
            if (!targetInRange)
            {
                anim.SetBool("attaque", false);
                waterJet.Stop();
            }
        }
    }

    public void focusPlayer()
    {
        if (!focusingPlayer)
        {
            focusingLeurre = false;
            focusingPlayer = true;
            Debug.LogError("focus player");
            updateTarget(player.transform);
        }
    }

    public void focusLeurre(GameObject leurre)
    {
        if (!focusingLeurre && !focusingPlayer)
        {
            Debug.LogError("Focus Leurre");
            focusingLeurre = true;
            updateTarget(leurre.transform);
        }
    }


    public void updateRange(bool r)
    {
        targetInRange = r;
        EnableFirstSPhere();
    }

    public Transform getTargetTransform()
    {
        return targetTransform;
    }
    public string getTargetTag()
    {
        return targetTag;
    }

    public float getEnclosureDamage()
    {
        return enclosureDamage;
    }

    public float getPlayerDamage()
    {
        return Playerdamage;
    }

    public void EnableFirstSPhere()
    {
        firstSPhere.enabled = true;
    }
}
