using UnityEngine;

public class Item : IEntity
{
    public int X { get; set; }
    public int Y { get; set; }
    public string Type { get; private set; }

    public Item(string type)
    {
        Type = type;
    }

    public void Interact(Bloblin bloblin) { }
}
