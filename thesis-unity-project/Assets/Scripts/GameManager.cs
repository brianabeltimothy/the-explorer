using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UIManager uiManager;
    private Player player;
    
    private void Awake() 
    {
        uiManager = FindAnyObjectByType<UIManager>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<Player>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(player.currentHits == player.maxHits)
        {
            Debug.Log("player dies");
            StartCoroutine(uiManager.ShowGameOverScreen());
        }
    }
}
