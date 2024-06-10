using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HidingSarcophagusTrigger : MonoBehaviour
{
    [SerializeField] private HidingSarcophagusController hidingSarcophagusController;
    [SerializeField] private Player player;

    private void Awake() {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerStay(Collider other) {
        if(other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            if(!hidingSarcophagusController.isOpen && player.isHiding)
            {
                hidingSarcophagusController.Out();
            }
        }
    }
}
