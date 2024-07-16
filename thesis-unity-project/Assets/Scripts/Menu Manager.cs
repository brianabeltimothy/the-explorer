using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public bool menuIsOpen;
    
    private PauseManager pauseManager;
    private InventoryManager inventoryManager;
    private InputManager inputManager;

    private void Awake() 
    {  
        Instance = this;
        inputManager = FindAnyObjectByType<InputManager>();
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        pauseManager = FindAnyObjectByType<PauseManager>();
    }

    private void Update() 
    {  
        if (inputManager.Inventory)
        {
            if (inventoryManager.inventoryIsOpened)
            {
                inventoryManager.CloseInventory();
                menuIsOpen = false;
            }
            else if (!menuIsOpen)
            {
                inventoryManager.OpenInventory();
                menuIsOpen = true;
            }
        }
        else if (inputManager.Pause && !menuIsOpen)
        {
            pauseManager.PauseGame();
            menuIsOpen = true;
        }

        if(menuIsOpen)
        {
            Cursor.lockState = CursorLockMode.None; // Unlock cursor
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Lock cursor
            Cursor.visible = false;
        }
    }
}
