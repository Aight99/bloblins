using System.Collections.Generic;

public class LevelConfig
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public List<BloblinConfig> Bloblins { get; private set; }
    public List<ItemConfig> Items { get; private set; }

    public LevelConfig(int width, int height, List<BloblinConfig> bloblins, List<ItemConfig> items)
    {
        Width = width;
        Height = height;
        Bloblins = bloblins;
        Items = items;
    }
}

public class BloblinConfig
{
    public string Type { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    public BloblinConfig(string type, int x, int y)
    {
        Type = type;
        X = x;
        Y = y;
    }
}

public class ItemConfig
{
    public string Type { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    public ItemConfig(string type, int x, int y)
    {
        Type = type;
        X = x;
        Y = y;
    }
}
