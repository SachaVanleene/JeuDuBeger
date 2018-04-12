using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using TPC;

public class AudioManagerPlayer : AudioPlayer {

    public GameObject ownerObject;
    private float lengthClip;
    private float clipLength;



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
                audioSource.maxDistance = GameVariables.Player.distMaxSound;
                audioSource.spatialBlend = 1f;
                audioSource.PlayOneShot(clip, vol);
                clipLength = clip.length;
                Destroy(audioSource, clipLength);
                return;
            }
        }
    }



    public void PlayFreeze()
    {
        PlaySound(GameVariables.Player.stringSoundFreezing);
    }

    public void StopFreeze()
    {
        Stop();
    }


}
