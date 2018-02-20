using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Wolves_Attack : MonoBehaviour {

    private Animator anim;
    // charactéristique du loup - attaque
    int damage;
    float timeBetweenAttacks;
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

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        script_path = GetComponent<IA_Wolves_Path>();

        timeBetweenAttacks = 0.833f; // timing animation
        timer = 0f;
        damage = 10;
        anim_time = 0.5f;

        isAttacking = false;

        onTriggerRange += script_path.updateRange;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == targetTag)
        {
            targetInRange = true;
            onTriggerRange.Invoke();
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == targetTag)
        {
            targetInRange = false;
            onTriggerRange.Invoke();
        }
    }

    public void updateTarget()
    {
        targetTag = GetComponent<IA_Wolves_Path>().targetTag; 
        targetTransform = GetComponent<IA_Wolves_Path>().targetTransform;
        Debug.LogError("Invoke fonctionne tag :" + targetTag);
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (isAttacking && anim.GetCurrentAnimatorStateInfo(0).IsName("Wolf_Layer.Attack Jump") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > anim_time) // attaquer au bon moment de l'naimation
        {
            Attack();
            isAttacking = false;
        }
        // si tmeps danimations poche de 99 % is attacking devient false
        if ((timer >= timeBetweenAttacks) && targetInRange && !isAttacking && targetTag !="Aucune")
        {
            anim.SetTrigger("attack");
            isAttacking = true;
            timer = 0f;
        }
    }

    void Attack()
    {
        if (targetTag == "Player")
        {
            targetTransform.gameObject.GetComponent<Player>().takeDamage(damage);
            Debug.LogError("Attaque joueur");
            if (!targetTransform.gameObject.GetComponent<Player>().alive)
            {
                script_path.updateTarget(null); // TO DO 
            }
        }
        if (targetTag == "Fences")
        {
            targetTransform.parent.gameObject.GetComponent<EnclosManager>().DamageEnclos(damage);
            Debug.LogError("Attaque enclos");
            if (targetTransform.parent.gameObject.GetComponent<EnclosManager>().getHealth() <= 0)
            {
                script_path.updateTarget(null); // TO DO 
            }
        }
    }

}
