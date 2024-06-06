using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private DoorController doorController;

    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            if(!doorController.isOpen)
            {
                doorController.OpenDoor();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            if(doorController.isOpen)
            {
                doorController.CloseDoor();
            }
        }
    }
}
