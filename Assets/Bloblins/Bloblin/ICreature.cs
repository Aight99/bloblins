using System;

public interface ICreature : IEnvironmentObject
{
    int MoveRange { get; }
    float IEnvironmentObject.DrawLayer => Layers.Creatures;
    bool CanMoveTo(CellPosition fromPosition, CellPosition toPosition)
    {
        var xShift = Math.Abs(fromPosition.X - toPosition.X);
        var yShift = Math.Abs(fromPosition.Y - toPosition.Y);
        var distance = xShift + yShift;
        return distance <= MoveRange;
    }
}

public interface IBloblin : ICreature { }

public interface IEnemy : ICreature
{
    IEnemyBehavior Behavior { get; }
}
