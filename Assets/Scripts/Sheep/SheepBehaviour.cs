using Assets.Script.Managers;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class SheepBehaviour : MonoBehaviour {
    public GameObject SmokeEffect;
    public GameObject GodHaseObject;
    public List<AudioClip> Clips;

    private AudioPlayer audioPlayer;
    private Vector3 direction = new Vector3(0, 0, 1);
    private bool fly = false;
    private GameObject godHaseInstance;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        audioPlayer = gameObject.AddComponent(typeof(AudioPlayer)) as AudioPlayer;
        audioPlayer.Clips = this.Clips;

        StartCoroutine(BeehSound(Random.Range(10, 100)));
    }
    void Update()
    {
        // hack fix
        if (Vector3.Distance(startPosition, transform.position) > GameVariables.Sheep.maxDistanceBeforReturningToStartPosition)
        {
            transform.position = startPosition;
        }
        Vector3 dir = direction * Time.deltaTime;
        if (fly)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            dir *= GameVariables.Sheep.flySpeed;
        }
        else
            dir *= GameVariables.Sheep.walkSpeed;

        transform.Translate(dir.x, dir.y, dir.z);
    }
    private IEnumerator BeehSound(float seconds)
    {
        yield return new WaitForSeconds(seconds/10f);
        audioPlayer.PlaySound(GameVariables.Sheep.stringSheepSound, GameVariables.Sheep.volumeSound, 
            0, GameVariables.Sheep.distMaxSound, true);
        StartCoroutine(BeehSound(Random.Range(50, 300)));
    }
    private IEnumerator KillAfterScream(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DestroyMe();
    }

    public void Kill()
    {
        if (fly || GameManager.instance.IsTheSunAwakeAndTheBirdAreSinging)
        {
            Destroy(godHaseInstance);
            DestroyMe();
        }
        else
        {
            audioPlayer.PlaySound(GameVariables.Sheep.stringSheepSoundDeath, GameVariables.Sheep.volumeSoundDeath,
                0, GameVariables.Sheep.distMaxSound);
            StopAllCoroutines();
            bool killNow = true;
            foreach(var clip in Clips)
            {
                if(clip.name.Equals(GameVariables.Sheep.stringSheepSoundDeath))
                {
                    killNow = false;
                    StartCoroutine(KillAfterScream(clip.length));
                    break;
                }
            }
            if (killNow)
                DestroyMe();
        }
    }
    public void DestroyMe()
    {
        GameObject boom = CFX_SpawnSystem.GetNextObject(SmokeEffect);
        boom.transform.position = transform.position;
        Destroy(this.gameObject);
    }
    public void SpiritInTheSky()
    {
        fly = true;
        var colliders = gameObject.GetComponents<BoxCollider>();
        foreach(var coll in colliders)
        {
            coll.enabled = false;
        }
        transform.rotation = new Quaternion(0,0,0,0);
        direction = new Vector3(0, 1, 0);
        godHaseInstance = Instantiate(GodHaseObject, gameObject.transform);
        StopAllCoroutines();
    }

    private float ignoreCollisionSheep = 1;

    private void OnTriggerEnter(Collider collider)
    {
        if (fly)
            return;
        if (collider.gameObject.CompareTag("Fences"))
        {
            transform.LookAt(startPosition);
        }
        else if(collider.gameObject.CompareTag("Sheep"))
        {
            transform.Rotate(0, Random.Range(110, 250), 0);
        }
    }
}


