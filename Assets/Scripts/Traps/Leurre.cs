using System.Collections;
using System.Collections.Generic;
using Assets.Script.Traps;
using UnityEngine;
using Assets.Scripts;

public class Leurre : MonoBehaviour {

    //IA Subscribers
    public delegate void onDead();
    public onDead onTriggerDead; //Prévenir touts les loups que je suis mort

    //Health variables
    public float actualHealth
    {
        get
        {
            return GetComponentInChildren<BaitTrap>().Durability;

        }
        set
        {
            GetComponentInChildren<BaitTrap>().Durability = (int)value; 
            
        }
    }

    public bool alive;

    //Reference to real target
    public GameObject rabbit;

    // Use this for initialization
    void Start () {
        actualHealth = GameVariables.Trap.Decoy.life;
        alive = true;
        rabbit.transform.localPosition = new Vector3(0, 0, 0.64f);
	}

    public void takeDamage(float dps)
    {
        actualHealth -= dps;

        if (actualHealth <= 0 && alive)
        {
            alive = false;
            //anim.SetTrigger("dead");
            onTriggerDead.Invoke();
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Wolf") && alive)
        {
            MakeWolfTargetMe(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Contains("Wolf") && alive)
        {
            MakeWolfTargetMe(other);
        }
    }


    void MakeWolfTargetMe(Collider other)
    {
        switch (other.tag)
        {
            case "CommonWolf":
                if(!other.gameObject.GetComponent<IA_Common_Wolves>().focusingPlayer)
                other.gameObject.GetComponent<IA_Common_Wolves>().focusLeurre(rabbit);
                break;
            case "WaterWolf":
                if (!other.transform.root.gameObject.GetComponent<IA_Water_Wolves>().focusingPlayer)
                    other.transform.parent.gameObject.GetComponent<IA_Water_Wolves>().focusLeurre(rabbit);
                break;
            case "MoutainWolf":
                if (!other.transform.root.gameObject.GetComponent<IA_Moutain_Wolves>().focusingPlayer)
                    other.transform.parent.gameObject.GetComponent<IA_Moutain_Wolves>().focusLeurre(rabbit);
                break;
            case "BossWolf":
                if (!other.gameObject.GetComponent<IA_Boss_Wolves>().focusingPlayer)
                    other.gameObject.GetComponent<IA_Boss_Wolves>().focusLeurre(rabbit);
                break;
            default:
                break;
        }
    }

    public void AddSubscriber(Leurre.onDead function)
    {
        onTriggerDead += function;
    }

    public void RemoveSubscriber(Leurre.onDead function)
    {
        onTriggerDead -= function;
    }

    // Update is called once per frame
    void Update ()
    {
    }
}
