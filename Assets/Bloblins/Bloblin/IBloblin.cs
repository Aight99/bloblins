public interface IBloblin : IEnvironmentObject
{
    string Name { get; }
    int MoveRange { get; }
    float IEnvironmentObject.DrawLayer => Layers.Bloblins;
    CellPosition GetMoveTarget(CellPosition position);
}
