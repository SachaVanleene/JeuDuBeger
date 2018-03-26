using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerWolves : AudioPlayer {

    public GameObject ownerObject;
    private float lengthClip;
    private float clipLength;

    //SpecificForMountainOrWater
    bool jetSoundPlaying;

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
        if (audioSource != null)
            audioSource.Stop();

        foreach (var clip in Clips)
        {
            if (clip.name.Equals(clipName))
            {
                audioSource = ownerObject.AddComponent<AudioSource>();
                audioSource.maxDistance = GameVariables.Sheep.distMaxSound;
                audioSource.minDistance = GameVariables.Sheep.distMinSound;
                audioSource.spatialBlend = 1f;
                audioSource.PlayOneShot(clip, vol);
                clipLength = clip.length;
                Destroy(audioSource, clipLength);
                //StartCoroutine(DelayedCallback());
                return;
            }
        }
    }

    public void PlayHitSound()
    {
        PlaySound("hit");
    }

    public void PlayAttackCommonWolvesSound()
    {
        int nb_sound = Random.Range(1, 3);
        PlaySound("attack" + nb_sound,0.5f);
    }

    public void PlayAttackMountainWolvesSound()
    {
        if (!jetSoundPlaying)
        {
            PlaySound("blizzard");
            jetSoundPlaying = true;
        }
    }

    public void StopMountainAttack()
    {
        Stop();
        jetSoundPlaying = false;
    }
}
