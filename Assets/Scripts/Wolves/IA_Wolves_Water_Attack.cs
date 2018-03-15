using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enclosures;
//using Assets.Scripts.Enclosures;
using UnityEngine;

public class IA_Wolves_Water_Attack : MonoBehaviour {

    private Animator anim;
    // charactéristique du loup - attaque
    int damage;
    float timeBetweenAttacks;
    float range;
    float rotationSpeed;
    float anim_time; // time of anim where it attack
    // variable commune  à tous lesl oups 
    public bool targetInRange;
    string targetTag;
    private Transform targetTransform; //the zombie's target
    IA_Wolves_Water_Path script_path;
    //Variable pour animation
    float timer;
    bool isAttacking;
    //Delegate pour prévenir le script path
    public delegate void onRange();
    public onRange onTriggerRange;

    //ParticleSystem
    public ParticleSystem waterJet;

    //Variable pour regarder l'objet
    //values for internal use
    private Quaternion lookRotation;
    private Vector3 direction;


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        script_path = GetComponent<IA_Wolves_Water_Path>();

        timeBetweenAttacks = 0.833f; // timing animation
        timer = 0f;
        rotationSpeed = 2f;
        damage = 10;
        range = 10;
        anim_time = 0.5f;

        isAttacking = false;

        onTriggerRange += script_path.updateRange;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (targetTag == "Player")
        {
            targetTransform.gameObject.GetComponent<Player>().takeDamage(damage);
            //Debug.LogError("Attaque joueur");
            if (!targetTransform.gameObject.GetComponent<Player>().Alive)
            {
                targetInRange = false;
                onTriggerRange.Invoke();
                script_path.GetTargetEnclos(); // TO DO 
            }
        }
        if (targetTag == "Fences")
        {
            targetTransform.parent.gameObject.GetComponent<EnclosureScript>().DamageEnclos(damage);
            //Debug.LogError("Attaque enclos");
            if (targetTransform.parent.gameObject.GetComponent<EnclosureScript>().Health <= 0)
            {
                targetInRange = false;
                onTriggerRange.Invoke();
                script_path.GetTargetEnclos(); // TO DO 
            }
        }
    }

    public void updateTarget()
    {
        targetTag = GetComponent<IA_Wolves_Water_Path>().targetTag;
        targetTransform = GetComponent<IA_Wolves_Water_Path>().targetTransform;
        //Debug.LogError("Invoke fonctionne tag :" + targetTag);
    }
    // Update is called once per frame
    void Update()
    {

        if (targetTransform != null && targetTransform.parent.gameObject.GetComponent<EnclosureScript>().Health > 0)
        {
            float dist = Vector3.Distance(targetTransform.position, transform.position);
            if (dist < range && !targetInRange)
            {
                targetInRange = true;
                onTriggerRange.Invoke();
            }
            if (targetInRange && dist > range)
            {

                targetInRange = false;
                onTriggerRange.Invoke();
            }
            if (targetInRange)
            {
                //look at target
                //find the vector pointing from our position to the target
                direction = (targetTransform.position - transform.position).normalized;

                //create the rotation we need to be in to look at the target
                lookRotation = Quaternion.LookRotation(direction);

                //rotate us over time according to speed until we are in the required rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

                anim.SetTrigger("attack");
                waterJet.Play();

            }
            else
            {
                waterJet.Stop();
            }
        }
    }

    
}
