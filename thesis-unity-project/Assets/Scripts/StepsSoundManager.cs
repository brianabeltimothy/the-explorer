using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsSoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> stepSounds;
    private AudioSource audioSource;

    private void Awake() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayStepSound()
    {
        int i = (int)Mathf.Floor(Random.Range(0, stepSounds.Count));
        audioSource.PlayOneShot(stepSounds[i]);
    }
}
