using System.Collections;
using UnityEngine;

public class Bloblin : IEntity
{
    public int X { get; set; }
    public int Y { get; set; }
    public string Type { get; private set; }

    public Bloblin(string type)
    {
        Type = type;
    }
}
