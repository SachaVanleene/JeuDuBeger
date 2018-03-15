using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enclosures;
//using Assets.Scripts.Enclosures;
using UnityEngine;

public class IA_Wolves_Attack : MonoBehaviour {

    private Animator anim;
    // charactéristique du loup - attaque
    int damage;
    float timeBetweenAttacks;
    float rotationSpeed;
    float anim_time; // time of anim where it attack
    // variable commune  à tous lesl oups 
    public bool targetInRange;
    string targetTag;
    private Transform targetTransform; //the zombie's target
    IA_Wolves_Path script_path;
    //Variable pour animation
    float timer;
    bool isAttacking;
    //Delegate pour prévenir le script path
    public delegate void onRange();
    public onRange onTriggerRange;
    //Variable pour regarder l'objet
    private Quaternion lookRotation;
    private Vector3 direction;

    //
    bool targetAlive;


    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        script_path = GetComponent<IA_Wolves_Path>();

        timeBetweenAttacks = 0.833f; // timing animation
        timer = 0f;
        damage = 10;
        anim_time = 0.5f;//
        rotationSpeed = 2f;

        isAttacking = false;

        onTriggerRange += script_path.updateRange;
    }

    void OnTriggerEnter(Collider other)
    {
        if (targetTransform != null)
        {
            float dist = Vector3.Distance(targetTransform.position, transform.position); // Afin dêtre sur que ce soit le bon enclos
            if (other.gameObject.tag == targetTag && dist < 5f)
            {
                targetInRange = true;
                onTriggerRange.Invoke();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (targetTransform != null)
        {
            float dist = Vector3.Distance(targetTransform.position, transform.position); // Afin dêtre sur que ce soit le bon enclos
            if (other.gameObject.tag == targetTag && dist > 5f)
            {
                targetInRange = false;
                onTriggerRange.Invoke();
            }
        }
    }

    // oncollider stay
    void OnTriggerExit(Collider other)
    {
        if (targetTransform != null)
        {
            float dist = Vector3.Distance(targetTransform.position, transform.position);
            if (other.gameObject.tag == targetTag)
            {
                targetInRange = false;
                onTriggerRange.Invoke();
            }
        }
    }

    public void updateTarget()
    {
        targetTag = GetComponent<IA_Wolves_Path>().targetTag; 
        targetTransform = GetComponent<IA_Wolves_Path>().targetTransform;
        //Debug.LogError("Invoke fonctionne tag :" + targetTag);
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (true & targetTransform!=null)
        {
            //look at target
            //find the vector pointing from our position to the target
            direction = (targetTransform.position - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            lookRotation = Quaternion.LookRotation(direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);


        }

        if (targetTransform != null)
        {
            if (isAttacking && anim.GetCurrentAnimatorStateInfo(0).IsName("Wolf_Layer.Attack Jump") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > anim_time) // attaquer au bon moment de l'naimation
            {
                Attack();
                isAttacking = false;
            }
            // si tmeps danimations poche de 99 % is attacking devient false
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
        if (targetTag == "Player")
        {
            targetTransform.gameObject.GetComponent<Player>().takeDamage(damage);
        }
        if (targetTag == "Fences")
        {
            targetTransform.parent.gameObject.GetComponent<EnclosureScript>().DamageEnclos(damage);
        }
    }

}
