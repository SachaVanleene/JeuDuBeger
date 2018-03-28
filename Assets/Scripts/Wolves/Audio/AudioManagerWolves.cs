using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class AudioManagerWolves : AudioPlayer {

    public GameObject ownerObject;
    private float lengthClip;
    private float clipLength;

    //SpecificForMountainOrWater
    bool jetSoundPlaying;
    AudioSource jetSound;

    private void Awake()
    {
        jetSoundPlaying = false;
    }

    private IEnumerator DelayedCallback()
    {
        yield return new WaitForSeconds(clipLength);
    }

    public new void PlaySound(string clipName, float vol = 1)
    {
        Debug.Log(clipName);
        if (audioSource != null)
            audioSource.Stop();

        foreach (var clip in Clips)
        {
            if (clip.name.Equals(clipName))
            {
                Debug.Log("play");

                audioSource = ownerObject.AddComponent<AudioSource>();
                audioSource.spatialBlend = 1f;
                audioSource.maxDistance = GameVariables.Wolf.distMaxSound;
                audioSource.PlayOneShot(clip, vol);
                clipLength = clip.length;
                if(clipName.Equals(GameVariables.Wolf.stringSoundBlizzard))
                {
                    jetSound = audioSource;
                }else
                {
                    Destroy(audioSource, clipLength);
                }
                return;
            }
        }
    }

    public void PlayHitSound()
    {
        PlaySound(GameVariables.Wolf.stringSoundHit);
    }

    public void PlayAttackCommonWolvesSound()
    {
        int nb_sound = Random.Range(1, GameVariables.Wolf.NbDifferentSoundAttack);
        PlaySound(GameVariables.Wolf.stringSoundAttack + nb_sound, GameVariables.Wolf.volumeSoundAttack);
    }

    public void PlayAttackMountainWolvesSound()
    {
        if (!jetSoundPlaying)
        {
            PlaySound(GameVariables.Wolf.stringSoundBlizzard);
            jetSoundPlaying = true;
        }
    }

    public void StopMountainAttack()
    {
        if(jetSoundPlaying && jetSound != null)
        {
            jetSound.Stop();
            Destroy(jetSound);
            jetSoundPlaying = false;
        }
    }
}
