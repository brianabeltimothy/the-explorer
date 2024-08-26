using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private VideoClip cutscene2;
    [SerializeField] private GameObject mummy;

    private BoxCollider boxCollider;

    private void Awake() 
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CutsceneManager.Instance.PlayCutscene(cutscene2, OnCutsceneEnd);
            boxCollider.enabled = false;
        }
    }

    private void OnCutsceneEnd()
    {
        mummy.SetActive(true);
        StartCoroutine(DisplayInstructionText());
    }

    private IEnumerator DisplayInstructionText()
    {
        UIManager.Instance.ChangeInstructionText("");
        UIManager.Instance.ChangeInstructionText("I need to hide!");
        yield return new WaitForSeconds(3f);
        UIManager.Instance.ChangeInstructionText("");
        Destroy(gameObject);
    }
}
