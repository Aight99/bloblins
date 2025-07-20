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
                return LevelLoader.LoadLevel(loadLevel.LevelNumber);

            case CellClickAction cellClick:
                return HandleCellClick(state, cellClick);

            default:
                return state;
        }
    }

    private static GameState HandleCellClick(GameState state, CellClickAction action)
    {
        var field = state.Field;
        var position = action.Position;

        if (
            position.X < 0
            || position.X >= field.Width
            || position.Y < 0
            || position.Y >= field.Height
        )
            return state;

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
            // Проверяем, можно ли перемещаться на выбранную клетку
            CellType cellType = field.CellTypes[position];
            if (!cellType.CanMoveTo())
            {
                Debug.LogError($"нельзя ходить на [{position.X};{position.Y}] (тип: {cellType})");
                return state;
            }

            DebugHelper.LogMovement($"топаем на [{position.X};{position.Y}]");
            var bloblin = field.Bloblins.First();
            return state.WithField(field.WithMovedBloblin(bloblin, position));
        }

        return state;
    }
}
