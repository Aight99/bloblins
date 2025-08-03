public interface IEnvironmentObject
{
    string Name { get; }
    CellPosition Position { get; set; }
    float DrawLayer { get; }
}

public static class Layers
{
    public const float Land = 0;
    public const float Creatures = 0.85f;
    public const float Items = 0.85f;
}
