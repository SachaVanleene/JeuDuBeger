using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {
    public List<AudioClip> Clips;

    protected AudioSource audioSource = null;
    public void Stop()
    {
        if (audioSource != null)
            audioSource.Stop();
    }
    public void PlaySound(string clipName, float vol = 1, float distMin = 0f, float distMax = 30f, bool bender2D = false)
    {
        if (audioSource != null)
            audioSource.Stop();

        foreach(var clip in Clips)
        {
            if(clip.name.Equals(clipName))
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.maxDistance = distMax;
                if (bender2D)
                    audioSource.spatialBlend = 1f;
                else
                    audioSource.minDistance = distMin;

                audioSource.PlayOneShot(clip, vol);
                UnityEngine.Object.Destroy(audioSource, clip.length);
                break;
            }
        }
    }
}
