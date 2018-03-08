﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance = null;
        public GameObject AudioSourceObject;
        public List<AudioClip> AudioClips;
        public List<AudioClip> AudioAmbuances;

        private AudioSource audioS;
        void Awake()
        {
            audioS = AudioSourceObject.GetComponent<AudioSource>();
            //Check if instance already exists
            if (instance == null)
                //if not, set instance to this
                instance = this;
            //If instance already exists and it's not this:
            else if (instance != this)
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
        }
        private void Start()
        {
        }

        public void PlaySound(string name, float intensity = 1f)
        {
            foreach (var clip in AudioClips)
            {
                if (clip.name.Equals(name))
                {
                    AudioSource audio = gameObject.AddComponent<AudioSource>();
                    audio.PlayOneShot(clip, intensity);
                    UnityEngine.Object.Destroy(audio, clip.length);
                    break;
                }
            }
        }
        public void PlayAmbuanceMusic(string name, float intensity = .5f)
        {
            audioS.Stop();
            foreach (var clip in AudioAmbuances)
            {
                if (clip.name.Equals(name))
                {
                    audioS.loop = true;
                    audioS.clip = clip;
                    audioS.Play();
                    break;
                }
            }
        }
    }
}