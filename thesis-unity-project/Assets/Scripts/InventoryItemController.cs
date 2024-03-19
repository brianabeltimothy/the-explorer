using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemController : MonoBehaviour
{
    public static InventoryItemController Instance;

    // [SerializeField] private Sprite image;
    // [SerializeField] private TMP_Text description;
    // [SerializeField] private GameObject inventoryItem; 

    private Sprite itemImage;
    private string itemDescription;

    private void Awake() {
        Instance = this;
    }

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        Debug.Log("Item is clicked");
        Debug.Log(itemDescription);
    }

    public void GetItemData(Item item)
    {
        itemImage = item.icon;
        itemDescription = item.description;
    }
}
