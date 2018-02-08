using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

    public Camera camera;
    public ParticleSystem muzzleFlash;

    public GameObject crosshair;

    Ray shootRay;                                   // A ray from the gun end forwards.
    RaycastHit shootHit;                            // A raycast hit to get information about what was hit.


    private int damage = 10;
    private float gunRange = 100f;
    private float fireRate = 0.25f;
    private float nextFire;
    public LineRenderer gunLine;
    public Light gunLight;
    LayerMask shootableMask;
    public float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.

    private AudioSource gunAudio;

    void Start () {
        gunAudio = GetComponent<AudioSource>();
        gunLine = GetComponent<LineRenderer>();
        gunLight = GetComponent<Light>();
        shootableMask = LayerMask.GetMask("Shootable");
    }

    private void Awake()
    {
        damage = 10;
        gunRange = 100f;
        fireRate = 0.25f;
        effectsDisplayTime = 0.1f;
}



    public void Shoot()
    {
        // Enable the light.
        gunLight.enabled = true;
        // Enable the line renderer and set it's first position to be the end of the gun.
        //gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);
        shootRay.origin = transform.position;

        muzzleFlash.Play();

        gunAudio.Play();


        shootRay.direction = crosshair.transform.position - transform.position;
        //Debug.LogError(crosshair.transform.position);
        //Debug.LogError(camera.transform.localEulerAngles.x);
        //shootRay = camera.ScreenPointToRay(new Vector2(Screen.width/2 , Screen.height/2 * Mathf.Cos(Mathf.Deg2Rad * camera.transform.localEulerAngles.x)));
        //shootRay.direction = crosshair.GetComponent<RectTransform>().localPosition;
        //Vector3 vec = new Vector3(0.5f, 0.5f, 0);
        //shootRay = Camera.main.ViewportPointToRay(vec);
        Debug.DrawRay(transform.position, shootRay.direction * gunRange, Color.green);
        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out shootHit, gunRange, shootableMask))
        {

            //Enemmy interaction
            // Try and find an EnemyHealth script on the gameobject hit.
            if (shootHit.collider.CompareTag("Wolf"))
            {
                Target wolfStats = shootHit.collider.GetComponent<Target>();
                wolfStats.takeDamage(damage);

                // Set the second position of the line renderer to the point the raycast hit.
                //gunLine.SetPosition(1, shootHit.point);
            }
            else
            {
                gunLine.SetPosition(1, shootHit.point);
                Debug.LogError(shootHit.point);
            }

        }
        // If the raycast didn't hit anything on the shootable layer...
        else
        {
            // ... set the second position of the line renderer to the fullest extent of the gun's range.
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * gunRange);
            
        }
    }

    public void DisableEffects(bool t)
    {
        if (t)
        {
            // Disable the line renderer and the light.
            gunLine.enabled = false;
            gunLight.enabled = false;
        }
    }
}
