using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPC
{
    public class HandleShooting : MonoBehaviour
    {

        StateManager states;
        public Animator weaponAnim;
        float timer;
        public Transform bulletSpawnPoint;
        public GameObject smokeParticle;
        public ParticleSystem[] muzzle;
        public GameObject casingPrefab;
        public Transform caseSpawn;

        public int curBullets = 3;
        public bool infiniteBullets = true;

        public SO.GunStats gunStats;

        [SerializeField]
        private int level;
        public int Level
        {
            get
            {
                return level;
            }

            set
            {
                level = value;
                gunStats.SetValues(level);
            }
        }

        bool shoot;
        bool dontShoot;
        bool emptyGun;

        public GameObject killGo;

        // Use this for initialization
        void Start()
        {
            states = GetComponent<StateManager>();
            timer = 1;
            killGo.SetActive(false);
            gunStats.Init();
        }

        // Update is called once per frame
        void Update()
        {
            if (states.shoot && states.alive)
            {
                if (timer <= 0)
                {

                    states.canShoot = true;
                    if (curBullets > 0 || infiniteBullets)
                    {
                        emptyGun = false;

                        StartCoroutine(NoAimShoot());
                        weaponAnim.SetBool("Shoot", false);
                        states.anim.SetTrigger("Fire");
                        curBullets--;
                    }
                    else
                    {
                        if (emptyGun)
                        {
                            //states.handleAnim.StartReload();
                            curBullets = 3;
                        }
                        else
                        {
                            //states.audioManager.PlayEffect("empty_gun");
                            emptyGun = true;
                        }
                    }
                    timer = gunStats.CurrentFireRate;
                }
                else
                {
                    weaponAnim.SetBool("Shoot", true);
                    timer -= Time.deltaTime;
                    states.canShoot = false;
                }
            }
            else
            {
                timer -= Time.deltaTime;
                //weaponAnim.SetBool("Shoot", false);
            }
        }


        IEnumerator NoAimShoot()
        {
            if (!states.aiming)
                states.shooting = true;
            yield return new WaitForSeconds(0.3f);
            //states.audioManager.PlayGunSound();
            states.shooting = false;
            //GameObject go = Instantiate(casingPrefab, caseSpawn.position, caseSpawn.rotation) as GameObject;
            //Rigidbody rb = go.GetComponent<Rigidbody>();
            //rb.AddForce(transform.right.normalized * 2 + Vector3.up * 1.3f, ForceMode.Impulse);
            //rb.AddRelativeTorque(go.transform.right * 1.5f, ForceMode.Impulse);

            for (int i = 0; i < muzzle.Length; i++)
            {
                muzzle[i].Emit(1);
            }
            RaycastShoot();
        }

        void RaycastShoot()
        {
            GameObject target = null;
            Vector3 direction = states.lookHitPosition - bulletSpawnPoint.position;
            RaycastHit hit;
            if (Physics.Raycast(bulletSpawnPoint.position, direction, out hit,100, states.shotLayerMask))
            {
                GameObject go = Instantiate(smokeParticle, hit.point, Quaternion.identity) as GameObject;
                go.transform.LookAt(bulletSpawnPoint.position);

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Shootable"))
                {

                    if (hit.collider.tag == "CommonWolf" || hit.collider.tag == "WaterWolf" || hit.collider.tag == "BossWolf" || hit.collider.tag == "MoutainWolf")
                    {
                        //Get the target where scripts are attached to 
                        if (hit.collider.tag == "CommonWolf" || hit.collider.tag == "BossWolf")
                        {
                            target = hit.collider.gameObject;
                        }
                        if (hit.collider.tag == "WaterWolf" || hit.collider.tag == "MoutainWolf")
                        {
                            target = hit.collider.transform.gameObject.transform.parent.gameObject;
                        }
                        //Apply damage
                        if (hit.collider.tag == "CommonWolf" || hit.collider.tag == "WaterWolf" || hit.collider.tag == "MoutainWolf")
                        {
                            if (target.GetComponent<WolfHealth>())
                            {
                                //target.GetComponent<WolfHealth>().takeDamage(Mathf.FloorToInt(gunStats.CurrentDamage), true);
                                target.GetComponent<WolfHealth>().takeDamage(20, true);
                                if (!target.GetComponent<WolfHealth>().alive)
                                    StartCoroutine(KillFeedBack());
                            }
                        }
                        if (hit.collider.tag == "BossWolf")
                        {
                            if (target.GetComponent<WolfBossHealth>())
                            {
                                //target.GetComponent<WolfHealth>().takeDamage(Mathf.FloorToInt(gunStats.CurrentDamage), true);
                                target.GetComponent<WolfBossHealth>().takeDamage(20, true);
                                if (!target.GetComponent<WolfBossHealth>().alive)
                                    StartCoroutine(KillFeedBack());
                            }
                        }
                        //Focusing Player, no need for boss cause he will focus player whenever he is alive
                        if (target.tag == "WaterWolf")
                        {
                            if (target.GetComponent<WolfHealth>().alive)
                                target.GetComponent<IA_Water_Wolves>().focusPlayer();
                        }
                        if (target.tag == "MoutainWolf")
                        {
                            if (target.GetComponent<WolfHealth>().alive)
                                target.GetComponent<IA_Moutain_Wolves>().focusPlayer();
                        }
                        if (target.tag == "CommonWolf")
                        {
                            if (target.GetComponent<WolfHealth>().alive)
                                target.GetComponent<IA_Common_Wolves>().focusPlayer();
                        }
                    }
                }
            }
        }

        private IEnumerator KillFeedBack()
        {
            killGo.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            killGo.SetActive(false);
        }
    }
}