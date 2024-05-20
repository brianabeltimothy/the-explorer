using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName ="Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id; //1 for key items, 2 for artifacts, 3 for sir alex's notes
    public string itemName;
    public Sprite icon;
    public string description;
}
