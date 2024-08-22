using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource audioSource;
    private float fadeDuration = 2.0f;

    private void Awake() 
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void FadeOutAudio()
    {
        StartCoroutine(FadeOutAudioCoroutine());
    }

    private IEnumerator FadeOutAudioCoroutine()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
