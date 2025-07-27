using System;

public interface IBloblin : IEnvironmentObject
{
    string Name { get; }
    int MoveRange { get; }
    float IEnvironmentObject.DrawLayer => Layers.Bloblins;
    bool CanMoveTo(CellPosition fromPosition, CellPosition toPosition)
    {
        var xShift = Math.Abs(fromPosition.X - toPosition.X);
        var yShift = Math.Abs(fromPosition.Y - toPosition.Y);
        var distance = xShift + yShift;
        return distance <= MoveRange;
    }
}
