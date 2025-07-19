using UnityEngine;

public class Item : IEnvironmentObject
{
    public CellPosition Position { get; private set; }
    public string Type { get; private set; }

    public Item(string type, CellPosition position)
    {
        Type = type;
        Position = position;
    }
}
