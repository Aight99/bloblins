using System.Collections;
using UnityEngine;

public class Bloblin : IEnvironmentObject
{
    public CellPosition Position { get; set; }
    public string Name { get; private set; }
    public float DrawLayer => Layers.Bloblins;

    public Bloblin(string name, CellPosition position)
    {
        Name = name;
        Position = position;
    }
}
