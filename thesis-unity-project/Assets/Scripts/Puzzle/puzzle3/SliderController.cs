using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SliderController : MonoBehaviour, IInteractable
{
    public static event Action<int, int> Rotated = delegate { };
    [SerializeField] private GameObject slider;
    private bool coroutineAllowed;
    public int numberShown;
    private AudioSource AS;
    [SerializeField] private int sliderId = 0;
    float totalRotation = 0f;

    private void Awake() {
        slider = this.gameObject;
    }

    private void Start()
    {
        coroutineAllowed = true;
        numberShown = 1;

        AS = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (coroutineAllowed)
        {
            // PlaySound();
            StartCoroutine(RotateSlider());
        }
    }

    void PlaySound()
    {
        AS.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        AS.volume = UnityEngine.Random.Range(0.8f, 1.0f);
        AS.Play();
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
