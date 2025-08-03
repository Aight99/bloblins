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
            return state.WithField(field.WithMovedBloblin(bloblin, position));
        }
        else if (objectOnCell != null)
        {
            DebugHelper.LogYippee($"Выбираем {objectOnCell.Name}");
            return state.WithSelectedObject(objectOnCell);
        }

        return HandleObjectDeselection(state);
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
}
