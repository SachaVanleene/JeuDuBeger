using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enclosures;
using UnityEngine;

public class Mountain_Wolves_ColliderSystem : MonoBehaviour {

    IA_Moutain_Wolves script_ia;

    //Variable (some are shared with script_ia) of target system
    string targetTag;
    Transform targetTransform;
    bool targetInRange;

    //Stats of wolf
    float playerDamage;
    float enclosureDamage;

    // Use this for initialization
    void Start()
    {
        if (script_ia != null)
        {
            // Debug.LogError("linked");
        }
        playerDamage = script_ia.getPlayerDamage();
        enclosureDamage = script_ia.getEnclosureDamage();
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
        if (targetTransform != null)
        {
            if (other.gameObject.tag == targetTag)
            {
                if (targetTag == "Player") //Seul le joueur ets une cible mouvante
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
            targetTransform.gameObject.GetComponent<Player>().Freezing();
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
