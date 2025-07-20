using System;

public class Baldush : IBloblin
{
    public CellPosition Position { get; set; }
    public string Name { get; private set; }

    public Baldush(CellPosition position)
    {
        Name = "Балдуш";
        Position = position;
    }

    public CellPosition GetMoveTarget(CellPosition position)
    {
        return new CellPosition(position.X + 1, position.Y - 1);
        // return position;
    }
}
