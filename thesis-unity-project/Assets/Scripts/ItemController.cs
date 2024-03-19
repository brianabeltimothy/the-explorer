using UnityEngine;

public class ItemController : MonoBehaviour, IInteractable
{
    public Item item;
    public static ItemController instance;

    private void Awake() 
    {
        instance = this;
    }

    public void Interact()
    {
        Destroy(gameObject);
        InventoryManager.Instance.Add(item);
    }
}
