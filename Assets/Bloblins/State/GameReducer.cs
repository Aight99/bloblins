public static class GameReducer
{
    public static GameState Reduce(GameState state, GameAction action)
    {
        switch (action)
        {
            case LoadLevelAction loadLevel:
                return LevelLoader.LoadLevel(loadLevel.LevelName);

            case CellClickAction cellClick:
                return HandleCellClick(state, cellClick);

            case HandleTurnChangeAction:
                return HandleTurnChange(state);

            default:
                throw new System.NotImplementedException();
        }
    }

    private static GameState HandleCellClick(GameState state, CellClickAction action)
    {
        var field = state.Field;
        var position = action.Position;
        field.EnvironmentObjects.TryGetValue(position, out var objectOnCell);

        if (state.SelectedObject == null)
        {
            return HandleObjectSelection(state, objectOnCell);
        }

        if (state.SelectedObject.Position == position)
        {
            return HandleObjectDeselection(state);
        }

        if (state.SelectedObject is IBloblin bloblin)
        {
            return HandleBloblinMove(state, bloblin, position);
        }
        else if (objectOnCell != null)
        {
            DebugHelper.LogYippee($"Выбираем {objectOnCell.Name}");
            return state.WithSelectedObject(objectOnCell);
        }

        return HandleObjectDeselection(state);
    }

    private static GameState HandleBloblinMove(
        GameState state,
        IBloblin bloblin,
        CellPosition position
    )
    {
        var newField = state.Field.WithMovedBloblin(bloblin, position);
        if (newField == state.Field)
        {
            return state;
        }

        var newTurnInfo = state.TurnInfo.WithReducedEnergy();
        return state.WithField(newField).WithTurnInfo(newTurnInfo);
    }

    private static GameState HandleObjectSelection(GameState state, IEnvironmentObject objectOnCell)
    {
        if (objectOnCell != null)
        {
            DebugHelper.LogYippee($"Выбираем {objectOnCell.Name}");
            return state.WithSelectedObject(objectOnCell);
        }
        return state;
    }

    private static GameState HandleObjectDeselection(GameState state)
    {
        DebugHelper.LogFiasco("Снимаем выделение");
        return state.WithSelectedObject(null);
    }

    private static GameState HandleTurnChange(GameState state)
    {
        if (state.TurnInfo.IsPhaseCompleted)
        {
            DebugHelper.LogYippee("Смена хода");
            return state.WithTurnInfo(new TurnState());
        }

        return state;
    }
}
