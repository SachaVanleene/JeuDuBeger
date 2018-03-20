using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {
    public List<AudioClip> Clips;

    protected AudioSource audioSource = null;
    public void Stop()
    {
        if (audioSource != null)
            audioSource.Stop();
    }
    public void PlaySound(string clipName, float vol = 1)
    {
        if (audioSource != null)
            audioSource.Stop();

        foreach(var clip in Clips)
        {
            if(clip.name.Equals(clipName))
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.maxDistance = GameVariables.Sheep.distMaxSound;
                audioSource.PlayOneShot(clip, vol);
                UnityEngine.Object.Destroy(audioSource, clip.length);
                break;
            }
        }
    }
}
