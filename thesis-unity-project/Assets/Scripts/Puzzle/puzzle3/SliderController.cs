using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SliderController : MonoBehaviour, IInteractable
{
    public static event Action<int, int> Rotated = delegate { };
    public int numberShown;

    [SerializeField] private int sliderId = 0;

    private GameObject slider;
    private bool coroutineAllowed;
    private float totalRotation = 0f;
    private AudioSource audioSource;

    private void Awake() {
        slider = this.gameObject;
    }

    private void Start()
    {
        coroutineAllowed = true;
        numberShown = 1;
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (coroutineAllowed)
        {
            PlaySound();
            StartCoroutine(RotateSlider());
        }
    }

    void PlaySound()
    {
        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        audioSource.volume = UnityEngine.Random.Range(0.8f, 1.0f);
        audioSource.Play();
    }

    private IEnumerator RotateSlider()
    {
        coroutineAllowed = false;
        
        float elapsedTime = 0f;
        float rotationDuration = 1f;

        while (elapsedTime < rotationDuration)
        {
            slider.transform.rotation *= Quaternion.Euler(0f, 60.0f * Time.deltaTime / rotationDuration, 0f);
            totalRotation += 60.0f * Time.deltaTime / rotationDuration;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        numberShown += 1;

        if (numberShown > 6)
        {
            numberShown = 1;
        }
        Rotated(sliderId, numberShown);
        coroutineAllowed = true;
    }
}
