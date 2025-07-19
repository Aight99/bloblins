using System;

public interface IBloblin : IEnvironmentObject
{
    string Name { get; }
    float IEnvironmentObject.DrawLayer => Layers.Bloblins;
    CellPosition GetMoveTarget(CellPosition position);
}
