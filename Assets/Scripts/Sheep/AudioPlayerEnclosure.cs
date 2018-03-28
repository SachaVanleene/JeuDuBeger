using Assets.Scripts.Enclosures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts;
using UnityEngine;

public class AudioPlayerEnclosure : AudioPlayer
{
    public GameObject ownerObject;
    private float lengthClip;
    private float clipLength;
    
    private IEnumerator DelayedCallback()
    {
        yield return new WaitForSeconds(clipLength);
        ownerObject.GetComponent<EnclosureScript>().RemoveAllSheeps();
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
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.maxDistance = GameVariables.Enclosure.distMaxSound;
                audioSource.minDistance = GameVariables.Enclosure.distMinSound;
                audioSource.spatialBlend = 1f;
                audioSource.PlayOneShot(clip, vol);
                clipLength = clip.length;
                Destroy(audioSource, clipLength);
                StartCoroutine(DelayedCallback());
                return;
            }
        }
    }
}
