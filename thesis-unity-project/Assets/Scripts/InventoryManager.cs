using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private InputManager inputManager;

    [SerializeField] private GameObject canvasBackground;
    [SerializeField] private GameObject inventoryObject;
    [SerializeField] private GameObject inventoryItem; 
    [SerializeField] private Transform keyItemContent;
    [SerializeField] private Transform artifactContent;


    private bool inventoryIsOpened = false;
    [SerializeField] private List<Item> keyItems = new List<Item>();
    [SerializeField] private List<Item> artifactItems = new List<Item>();
    [SerializeField] public List<Item> notes = new List<Item>();

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
        
        canvasBackground.SetActive(true);
        inventoryObject.SetActive(true);

        inventoryIsOpened = true;

        ListItems();
    }

    private void CloseInventory()
    {
        Time.timeScale = 1; // Resume game
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor
        Cursor.visible = false;

        canvasBackground.SetActive(false);
        inventoryObject.SetActive(false);

        inventoryIsOpened = false;
    }

    public void Add(Item item)
    {
        switch (item.id)
        {
            case 1:
                keyItems.Add(item);
                break;
            case 2:
                artifactItems.Add(item);
                break;
            case 3:
                notes.Add(item);
                break;
        }
    }

    // public void Remove(Item item)
    // {
    //     keyItems.Remove(item);
    // }

    public void ListItems()
    {
        foreach(Transform item in keyItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach(Transform item in artifactContent)
        {
            Destroy(item.gameObject);
        }

        // Key items
        foreach(var item in keyItems)
        {
            GameObject obj = Instantiate(inventoryItem, keyItemContent);
            var itemName = obj.GetComponentInChildren<TextMeshProUGUI>();

            InventoryItemController itemController = obj.GetComponent<InventoryItemController>(); // Get the InventoryItemController component
            itemController.GetItemData(item); // Set the item data

            itemName.text = item.itemName.ToUpper();
        }

        // Artifacts
        foreach(var item in artifactItems)
        {
            GameObject obj = Instantiate(inventoryItem, artifactContent);
            var itemName = obj.GetComponentInChildren<TextMeshProUGUI>();

            InventoryItemController itemController = obj.GetComponent<InventoryItemController>(); // Get the InventoryItemController component
            itemController.GetItemData(item); // Set the item data

            itemName.text = item.itemName.ToUpper();
        }
    }
}