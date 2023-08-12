using System;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;


    public void SetAudioClip(AudioClip clip)
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = clip;
    }

    public void PlaySelectedRegion(int totalSamples, float duration, float startSample, float endSample)
    {
        audioSource.Stop();
        audioSource.time = 0;

        float startTime = startSample * duration / totalSamples;
        float endTime = endSample * duration / totalSamples;
        audioSource.time = startTime;
        audioSource.Play();
        StartCoroutine(StopAfterDuration(endTime - startTime));
    }

    private System.Collections.IEnumerator StopAfterDuration(float newDuration)
    {
        yield return new WaitForSeconds(newDuration);
        audioSource.Stop();
    }

    public void AdjustVolume(float newVolume)
    {
        audioSource.volume = newVolume;
    }
}