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
    public readonly string LevelName;

    public LoadLevelAction(string levelName)
    {
        LevelName = levelName;
    }
}

public class HandleTurnChangeAction : GameAction { }

public class ProcessEnemyTurnAction : GameAction { }
