using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        if (objectOnCell != null)
        {
            if (objectOnCell is IBloblin bloblin)
            {
                DebugHelper.LogYippee($"это {bloblin.Name}");
            }
            else if (objectOnCell is Item item)
            {
                DebugHelper.LogYippee("это чевота");
            }
        }
        else
        {
            var bloblin = field.Bloblins.First();
            return state.WithField(field.WithMovedBloblin(bloblin, position));
        }

        return state;
    }
}
