using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PuzzleManager : MonoBehaviour
{
    private int[] result, correctCombination;
    [SerializeField] private GameObject stoneCover;
    private AudioSource audioSource;

    private void Start()
    {
        result = new int[]{1, 1, 1, 1};
        correctCombination = new int[] {1, 5, 3, 4};
        SliderController.Rotated += CheckResults;
        audioSource = stoneCover.GetComponent<AudioSource>();
    }

    private void CheckResults(int wheelId, int numberShown)
    {   
        switch (wheelId)
        {
            case 1:
                result[0] = numberShown;
                break;
            case 2:
                result[1] = numberShown;
                break;
            case 3:
                result[2] = numberShown;
                break;
            case 4:
                result[3] = numberShown;
                break;
        }

        if (result[0] == correctCombination[0] && 
            result[1] == correctCombination[1] &&
            result[2] == correctCombination[2] &&
            result[3] == correctCombination[3]) 
        {
            StartCoroutine(OpenStoneCoroutine());
        }
    }

    private IEnumerator OpenStoneCoroutine()
    {
        yield return new WaitForSeconds(1.5f);

        float elapsedTime = 0f;
        float transitionDuration = 2.0f;

        Vector3 stoneCoverInitialPos = stoneCover.transform.position;
        Vector3 stoneCoverTargetPos = new Vector3(stoneCover.transform.position.x + 1, stoneCover.transform.position.y, stoneCover.transform.position.z);
        
        audioSource.Play();

        while (elapsedTime < transitionDuration)
        {
            stoneCover.transform.position = Vector3.Lerp(stoneCoverInitialPos, stoneCoverTargetPos, elapsedTime / transitionDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        stoneCover.transform.position = stoneCoverTargetPos;
        Destroy(stoneCover);
    }

    private void OnDestroy()
    {
        SliderController.Rotated += CheckResults;
    }
}
