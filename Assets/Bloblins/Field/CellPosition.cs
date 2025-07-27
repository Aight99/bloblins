using System;

public readonly struct CellPosition
{
    public readonly int X;
    public readonly int Y;

    public CellPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(CellPosition left, CellPosition right)
    {
        return left.X == right.X && left.Y == right.Y;
    }

    public static bool operator !=(CellPosition left, CellPosition right)
    {
        return !(left == right);
    }

    public override bool Equals(object obj)
    {
        if (obj is CellPosition position)
        {
            return this == position;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"[{X}; {Y}]";
    }
}
