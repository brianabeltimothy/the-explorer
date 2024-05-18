using UnityEngine;

public class ChildColliderHandler : MonoBehaviour
{
    private MummyAI parentScript;

    private void Start()
    {
        parentScript = GetComponentInParent<MummyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parentScript != null)
        {
            parentScript.OnChildTriggerEnter(other);
        }
    }
}
