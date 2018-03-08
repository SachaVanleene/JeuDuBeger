using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script à placer sur le Particle system enfant du loup aquatique

public class WolfWaterAttack : MonoBehaviour {

    private ParticleSystem waterJet;
    private int damage = 1;
    private Animator anim;


    // Use this for initialization
    /*void Start () {
        waterJet = gameObject.GetComponentInChildren<ParticleSystem>();
        anim = GetComponentInParent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        
        //temp pour l'attaque
        waterJet.Stop();
        if (Input.GetKey(KeyCode.M))
        {
            anim.SetTrigger("Attack");
            waterJet.Play();
        } 

    }*/

    /*private void OnParticleCollision(GameObject other)
    {
        if (targetTag == "Player")
        {
            targetTransform.gameObject.GetComponent<Player>().takeDamage(damage);
            //Debug.LogError("Attaque joueur");
            if (!targetTransform.gameObject.GetComponent<Player>().Alive)
            {
                targetInRange = false;
            }
        }
        if (targetTag == "Fences")
        {
            targetTransform.parent.gameObject.GetComponent<EnclosureScript>().DamageEnclos(damage);
            //Debug.LogError("Attaque enclos");
            if (targetTransform.parent.gameObject.GetComponent<EnclosureScript>().Health <= 0)
            {
                targetInRange = false;
            }
        }
    }*/

    private void OnParticleCollision(GameObject other)
    {
        Debug.LogError("Collision particle");
    }
}
