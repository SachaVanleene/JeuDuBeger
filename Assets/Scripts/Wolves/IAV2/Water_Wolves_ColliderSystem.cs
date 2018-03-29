using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enclosures;
using UnityEngine;

public class Water_Wolves_ColliderSystem : MonoBehaviour {

    IA_Water_Wolves script_ia;

    //Variable (some are shared with script_ia) of target system
    string targetTag;
    Transform targetTransform;
    bool targetInRange;

    //Stat of wolf
    float playerDamage;
    float enclosureDamage;
	// Use this for initialization
	void Start () {
        playerDamage = script_ia.getPlayerDamage();
        enclosureDamage = script_ia.getEnclosureDamage();
	}

    private void Awake()
    {
        script_ia = transform.parent.gameObject.GetComponent<IA_Water_Wolves>();
        targetTag = "Aucune";
        targetTransform = null;
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnTriggerExit(Collider other)
    {
        if (targetTransform != null)
        {
            if (other.gameObject.tag == targetTag)
            {
                if (targetTag == "Player") //Seul le joueur est une cible mouvante
                {
                    targetInRange = false;
                    script_ia.updateRange(targetInRange);
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
        if (targetTag == "Player")
        {
            targetTransform.gameObject.GetComponent<Player>().takeDamage(playerDamage);
        }
        if (targetTag == "Leurre")
        {
            targetTransform.parent.gameObject.GetComponent<Leurre>().takeDamage(enclosureDamage);
        }
        if (targetTag == "Fences")
        {
            if (other.transform.IsChildOf(targetTransform.parent))
            {
                targetTransform.parent.gameObject.GetComponent<EnclosureScript>().DamageEnclos(enclosureDamage);
            }
        }
    }
}
