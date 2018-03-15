using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enclosures;
using UnityEngine;

public class Mountain_Wolves_ColliderSystem : MonoBehaviour {

    string targetTag;
    Transform targetTransform;
    IA_Moutain_Wolves script_ia;
    bool targetInRange;
    float damage;
    // Use this for initialization
    void Start()
    {
        if (script_ia != null)
        {
            // Debug.LogError("linked");
        }
        damage = script_ia.getDamage();
    }

    private void Awake()
    {
        script_ia = transform.parent.gameObject.GetComponent<IA_Moutain_Wolves>();
        targetTag = "Aucune";
        targetTransform = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerExit(Collider other)
    {
        //Debug.LogError("Not in range");
        if (targetTransform != null)
        {
            if (other.gameObject.tag == targetTag)
            {
                if (targetTag == "Player")
                {
                    targetInRange = false;
                    script_ia.updateRange(targetInRange);
                }
                else // fences case
                {
                    /*if (GameObject.ReferenceEquals(targetTransform.gameObject, other.gameObject))
                    {
                       Debug.LogError("Not in range");
                        targetInRange = false;
                        script_ia.updateRange(targetInRange);
                        //alreadyFocusingAFence = false;
                    }*/
                }

            }
        }
    }

    public void updateParameter()
    {
        targetTag = script_ia.getTargetTag();
        targetTransform = script_ia.getTargetTransform();
    }

    void OnParticleCollision(GameObject other)
    {
        //Debug.LogError("Coollide");
        if (targetTag == "Player")
        {
            targetTransform.gameObject.GetComponent<Player>().takeDamage(damage);
            targetTransform.gameObject.GetComponent<Player>().Freezing();
            //Debug.LogError("Attaque joueur");
            /* if (!targetTransform.gameObject.GetComponent<Player>().Alive)
             {
                 targetInRange = false;
             }*/
        }
        if (targetTag == "Fences")
        {
            if (other.transform.IsChildOf(targetTransform.parent))
            {
                //Debug.LogError("Collision particle");
                targetTransform.parent.gameObject.GetComponent<EnclosureScript>().DamageEnclos(damage);
                //Debug.LogError("Attaque enclos");
            }
            /*if (targetTransform.parent.gameObject.GetComponent<EnclosureScript>().Health <= 0)
            {
                targetInRange = false;
            }*/
        }
    }
}
