public interface IEnvironmentObject
{
    CellPosition Position { get; set; }
    float DrawLayer { get; }
}

public static class Layers
{
    public const float Land = 0;
    public const float Bloblins = 0.5f;
    public const float Items = 0.5f;
}
