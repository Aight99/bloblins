public abstract class GameAction { }

public class CellClickAction : GameAction
{
    public readonly CellPosition Position;

    public CellClickAction(CellPosition position)
    {
        Position = position;
    }
}

public class LoadLevelAction : GameAction
{
    public readonly int LevelNumber;

    public LoadLevelAction(int levelNumber)
    {
        LevelNumber = levelNumber;
    }
}

public class SelectBloblinAction : GameAction
{
    public readonly Bloblin Bloblin;

    public SelectBloblinAction(Bloblin bloblin)
    {
        Bloblin = bloblin;
    }
}

public class MoveEntityAction : GameAction
{
    public readonly CellPosition From;
    public readonly CellPosition To;

    public MoveEntityAction(CellPosition from, CellPosition to)
    {
        From = from;
        To = to;
    }
}
