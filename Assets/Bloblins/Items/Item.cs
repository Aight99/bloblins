using UnityEngine;

public class Item : IEnvironmentObject
{
    public CellPosition Position { get; set; }
    public string Name { get; private set; }
    public float DrawLayer => Layers.Items;

    public Item(string name, CellPosition position)
    {
        Name = name;
        Position = position;
    }
}
