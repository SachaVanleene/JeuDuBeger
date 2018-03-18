using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioPlayer : MonoBehaviour {
    public List<AudioClip> Clips;
    public float Volume = 1f;

    private AudioSource audioSource = null;

    public void PlaySound(string clipName, float vol = -1)
    {
        if (vol <= 0)
            vol = Volume;

        foreach(var clip in Clips)
        {
            if(clip.name.Equals(clipName))
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.PlayOneShot(clip, vol);
                UnityEngine.Object.Destroy(audioSource, clip.length);
                break;
            }
        }
    }
}
