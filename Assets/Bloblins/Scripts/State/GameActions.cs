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