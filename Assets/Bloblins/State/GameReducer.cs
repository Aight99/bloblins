public static class GameReducer
{
    public static GameAction Reduce(ref GameState state, GameAction action)
    {
        switch (action)
        {
            case null:
                return null;

            case LoadLevelAction loadLevel:
                state = LevelLoader.LoadLevel(loadLevel.LevelName);
                return null;

            case CellClickAction cellClick:
                return HandleCellClick(ref state, cellClick);

            case HandleTurnChangeAction:
                HandleTurnChange(ref state);
                return null;

            default:
                throw new System.NotImplementedException();
        }
    }

    private static GameAction HandleCellClick(ref GameState state, CellClickAction action)
    {
        var field = state.Field;
        var position = action.Position;
        field.EnvironmentObjects.TryGetValue(position, out var objectOnCell);

        if (state.SelectedObject == null)
        {
            HandleObjectSelection(ref state, objectOnCell);
            return null;
        }

        if (state.SelectedObject.Position == position)
        {
            HandleObjectDeselection(ref state);
            return null;
        }

        if (state.SelectedObject is IBloblin bloblin)
        {
            return HandleBloblinMove(ref state, bloblin, position);
        }
        else if (objectOnCell != null)
        {
            DebugHelper.LogYippee($"Выбираем {objectOnCell.Name}");
            state = state.WithSelectedObject(objectOnCell);
            return null;
        }

        HandleObjectDeselection(ref state);
        return null;
    }

    private static GameAction HandleBloblinMove(
        ref GameState state,
        IBloblin bloblin,
        CellPosition position
    )
    {
        var newField = state.Field.WithMovedBloblin(bloblin, position);
        if (newField == state.Field)
        {
            return null;
        }

        state = state.WithField(newField);
        return new HandleTurnChangeAction();
    }

    private static void HandleObjectSelection(ref GameState state, IEnvironmentObject objectOnCell)
    {
        if (objectOnCell != null)
        {
            DebugHelper.LogYippee($"Выбираем {objectOnCell.Name}");
            state = state.WithSelectedObject(objectOnCell);
        }
    }

    private static void HandleObjectDeselection(ref GameState state)
    {
        DebugHelper.LogFiasco("Снимаем выделение");
        state = state.WithSelectedObject(null);
    }

    private static void HandleTurnChange(ref GameState state)
    {
        // DebugHelper.LogYippee("Смена хода");
        state = state.WithNextTurn();
    }
}
