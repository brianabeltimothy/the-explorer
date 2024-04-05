using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemController : MonoBehaviour
{
    public static InventoryItemController Instance;
    public Item item;

    //key item content
    [SerializeField] private Image keyItemImage;
    [SerializeField] private TMP_Text keyItemName;
    [SerializeField] private TMP_Text keyItemDescription;

    //artifact content
    [SerializeField] private Image artifactImage;
    [SerializeField] private TMP_Text artifactName;
    [SerializeField] private TMP_Text artifactDescription;

    private void Awake() {
        Instance = this;
    }

    private void Start()
    {
        //get key items content
        keyItemDescription = GameObject.Find("Canvas/Inventory/Tab Content/Key Items Content/Item Details/Item Description Text").GetComponent<TMP_Text>();
        keyItemName = GameObject.Find("Canvas/Inventory/Tab Content/Key Items Content/Item Details/Item Name Text").GetComponent<TMP_Text>();
        keyItemImage = GameObject.Find("Canvas/Inventory/Tab Content/Key Items Content/Item Details/Item Image").GetComponent<Image>();

        //artifact content
        artifactDescription = GameObject.Find("Canvas/Inventory/Tab Content/Artifacts Content/Item Details/Item Description Text").GetComponent<TMP_Text>();
        artifactName = GameObject.Find("Canvas/Inventory/Tab Content/Artifacts Content/Item Details/Item Name Text").GetComponent<TMP_Text>();
        artifactImage = GameObject.Find("Canvas/Inventory/Tab Content/Artifacts Content/Item Details/Item Image").GetComponent<Image>();
        
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        if (item != null)
        {
            if (item.id == 1)
            {
                Color keyItemImageColor = keyItemImage.color;
                if (keyItemImageColor.a < 1f)
                {
                    keyItemImageColor.a = 1f;
                    keyItemImage.color = keyItemImageColor;
                }

                keyItemImage.sprite = this.item.icon;
                keyItemName.text = this.item.itemName.ToUpper();
                keyItemDescription.text = this.item.description;
            }
            else if (item.id == 2)
            {
                Color artifactImageColor = artifactImage.color;
                if (artifactImageColor.a < 1f)
                {
                    artifactImageColor.a = 1f;
                    artifactImage.color = artifactImageColor;
                }

                artifactImage.sprite = this.item.icon;
                artifactName.text = this.item.itemName.ToUpper();
                artifactDescription.text = this.item.description;
            }
        }
        else
        {
            Debug.LogWarning("Item is null!");
        }
    }

    public void GetItemData(Item item)
    {
        this.item = item;
    }
}
