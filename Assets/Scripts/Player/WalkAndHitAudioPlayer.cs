using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using TPC;
using Assets.Script.Managers;

public class WalkAndHitAudioPlayer : MonoBehaviour {

    StateManager states;

    public GameObject ownerObject;

    AudioSource walk_sound;
    AudioSource hit_sound;

    public AudioClip walk;
    public AudioClip hit;

    float walkSpeed;
    float runSpeed;

    private void Awake()
    {
        states = GetComponent<StateManager>();

        walkSpeed = 1.0f;

        runSpeed = 1.6f;
    }

    void Start () {
        walk_sound = ownerObject.AddComponent<AudioSource>();
        walk_sound.playOnAwake = false;
        walk_sound.clip = walk;
        walk_sound.pitch = walkSpeed;
        walk_sound.volume = 0.2f;

        hit_sound = ownerObject.AddComponent<AudioSource>();
        hit_sound.playOnAwake = false;
        hit_sound.clip = hit;
        hit_sound.volume = 0.5f;
    }

    public void PlayHitSound()
    {
        if (!hit_sound.isPlaying)
        {
            hit_sound.Play();
        }
    }

	void Update () {
        if (states.moving && !walk_sound.isPlaying && !GameManager.instance.IsPaused)
        {
            walk_sound.Play();
        }
        if (!states.moving || GameManager.instance.IsPaused)
        {
            walk_sound.Stop();
        }

        if (states.run)
        {
            walk_sound.pitch = runSpeed;
        }
        else
        {
            walk_sound.pitch = walkSpeed;
        }
	}
}
