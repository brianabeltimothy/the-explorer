using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private InputManager inputManager;

    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private GameObject canvasBg;
    [SerializeField] private GameObject inventoryObj;

    private bool inventoryIsOpened = false;

    public Transform itemContent;
    public GameObject inventoryItem; 

    private void Awake() {
        Instance = this;
        inputManager = FindAnyObjectByType<InputManager>();
    }

    private void Update()
    {
        if(inputManager.Inventory)
        {
            if (!inventoryIsOpened)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        Time.timeScale = 0; // Pause
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Cursor.visible = true;
        
        canvasBg.SetActive(true);
        inventoryObj.SetActive(true);

        inventoryIsOpened = true;

        ListItems();
    }

    private void CloseInventory()
    {
        Time.timeScale = 1; // Resume game
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor
        Cursor.visible = false;

        canvasBg.SetActive(false);
        inventoryObj.SetActive(false);

        inventoryIsOpened = false;
    }

    public void Add(Item item)
    {
        items.Add(item);
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }

    public void ListItems()
    {
        foreach(Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }

        foreach(var item in items)
        {
            GameObject obj = Instantiate(inventoryItem, itemContent);
            var itemName = obj.GetComponentInChildren<TextMeshProUGUI>();

            itemName.text = item.itemName;
        }
    }
}
