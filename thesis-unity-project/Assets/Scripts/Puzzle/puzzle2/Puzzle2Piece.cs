using System.Collections;
using UnityEngine;
using System;

public class Puzzle2Piece : MonoBehaviour
{
    public static event Action<int, int> Rotated = delegate { };
    public int numberShown = 1;

    [SerializeField] private int pieceId = 0;

    private GameObject slider;
    private bool coroutineAllowed;
    private float totalRotation = 0f;
    // private AudioSource audioSource;

    private void Awake() {
        slider = this.gameObject;
        // audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        coroutineAllowed = true;
    }

    public void OnMouseDown()
    {
        if (coroutineAllowed)
        {
            PlaySound();
            StartCoroutine(RotateCoroutine());
        }
    }

    void PlaySound()
    {
        // audioSource.Play();
    }

    private IEnumerator RotateCoroutine()
    {
        coroutineAllowed = false;
        
        float elapsedTime = 0f;
        float rotationDuration = 1f;

        while (elapsedTime < rotationDuration)
        {
            slider.transform.rotation *= Quaternion.Euler(72.0f * Time.deltaTime / rotationDuration, 0f, 0f);
            totalRotation += 60.0f * Time.deltaTime / rotationDuration;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        numberShown += 1;

        if (numberShown > 5)
        {
            numberShown = 1;
        }
        Rotated(pieceId, numberShown);
        coroutineAllowed = true;
    }
}
