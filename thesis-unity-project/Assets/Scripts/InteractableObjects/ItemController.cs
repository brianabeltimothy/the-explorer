using UnityEngine;

public class ItemController : MonoBehaviour, IInteractable
{
    public Item item;
    public static ItemController instance;

    private string instruction = "[E] <br> Collect";

    private void Awake() 
    {
        instance = this;
    }

    public void Interact()
    {
        InventoryManager.Instance.Add(item);
        //show the item for a sec
        Destroy(gameObject);
    }

    public string GivesInstructionText()
    {
        return instruction;
    }
}
