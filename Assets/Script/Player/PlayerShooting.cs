using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    public Animator anim;
    float timer;
    private int shoting_layer;
    bool isShooting;
    GameObject gun;
    Shooting gun_script;
    private float nextFire;
    float anim_time;

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    // Use this for initialization
    private void Awake()
    {
        anim = GetComponent<Animator>();
        shoting_layer = 1;
        gun = getChildGameObject(this.gameObject, "gun_end");
        gun_script = gun.GetComponent<Shooting>();
        Debug.Log(gun.transform.position);
        nextFire = 1.2f;
        anim_time = .30f;

    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        bool attack = Input.GetButtonDown("Fire1");
        /*if (!attack)
            anim.SetBool("Fire", false);*/


        if ( attack && timer > nextFire && !isShooting)
        {
            //Debug.LogError("Shooting");
            isShooting = true;
            if (anim.GetLayerWeight(shoting_layer) != 1.0f)
            { // if not active yet

                anim.SetLayerWeight(shoting_layer, 1.0f);

            }
            anim.SetTrigger("Fire");
        }
        if(isShooting && anim.GetCurrentAnimatorStateInfo(shoting_layer).IsName("Shooting_Layer.Shot") && anim.GetCurrentAnimatorStateInfo(shoting_layer).normalizedTime > anim_time) {
            timer = 0f;
            gun_script.Shoot();
            isShooting = false;
            //anim.SetLayerWeight(shoting_layer, 0.0f);
        }

        if (timer >= nextFire * gun_script.effectsDisplayTime)
        {
            // ... disable the effects.
            gun_script.DisableEffects(true);
        }
    }
}
