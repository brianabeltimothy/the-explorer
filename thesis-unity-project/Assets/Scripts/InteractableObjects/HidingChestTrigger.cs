using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HidingChestTrigger : MonoBehaviour
{
    [SerializeField] private HidingChestController hidingChestController;
    private Player player;

    private void Awake() {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerStay(Collider other) {
        if(other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            if(!hidingChestController.isOpen && player.isHiding)
            {
                hidingChestController.Out();
            }
        }
    }
}
