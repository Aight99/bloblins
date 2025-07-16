using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameReducer
{
    public static GameState Reduce(GameState state, GameAction action)
    {
        switch (action)
        {
            case LoadLevelAction loadLevel:
                return HandleLoadLevel(state, loadLevel);

            case CellClickAction cellClick:
                return HandleCellClick(state, cellClick);

            case SelectBloblinAction selectBloblin:
                return state.WithField(state.Field.WithSelectedBloblin(selectBloblin.Bloblin));

            case MoveEntityAction moveEntity:
                return state.WithField(state.Field.WithMovedEntity(moveEntity.From, moveEntity.To));

            default:
                return state;
        }
    }

    private static GameState HandleLoadLevel(GameState state, LoadLevelAction action)
    {
        var config = CreateLevelConfig(action.LevelNumber);
        var entities = new Dictionary<CellPosition, IEntity>();

        foreach (var bloblinConfig in config.Bloblins)
        {
            var position = new CellPosition(bloblinConfig.X, bloblinConfig.Y);
            entities[position] = new Bloblin(bloblinConfig.Type) { X = position.X, Y = position.Y };
        }

        foreach (var itemConfig in config.Items)
        {
            var position = new CellPosition(itemConfig.X, itemConfig.Y);
            entities[position] = new Item(itemConfig.Type) { X = position.X, Y = position.Y };
        }

        var fieldState = new FieldState(config.Width, config.Height, entities);
        return new GameState(fieldState, action.LevelNumber);
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

        field.Entities.TryGetValue(position, out var entity);

        if (entity != null)
        {
            if (entity is Bloblin bloblin)
            {
                return state.WithField(field.WithSelectedBloblin(bloblin));
            }
            else if (entity is Item item && field.SelectedBloblin != null)
            {
                var bloblinPosition = new CellPosition(
                    field.SelectedBloblin.X,
                    field.SelectedBloblin.Y
                );
                if (IsAdjacent(bloblinPosition, position))
                {
                    item.Interact(field.SelectedBloblin);
                    return state;
                }
            }
        }
        else if (field.SelectedBloblin != null)
        {
            var bloblinPosition = new CellPosition(
                field.SelectedBloblin.X,
                field.SelectedBloblin.Y
            );
            if (CanMoveTo(field, position))
            {
                return state.WithField(field.WithMovedEntity(bloblinPosition, position));
            }
        }

        return state;
    }

    private static bool IsAdjacent(CellPosition pos1, CellPosition pos2)
    {
        int dx = Mathf.Abs(pos1.X - pos2.X);
        int dy = Mathf.Abs(pos1.Y - pos2.Y);
        return dx <= 1 && dy <= 1 && dx + dy > 0;
    }

    private static bool CanMoveTo(FieldState field, CellPosition position)
    {
        if (
            position.X < 0
            || position.X >= field.Width
            || position.Y < 0
            || position.Y >= field.Height
        )
            return false;

        return !field.Entities.ContainsKey(position);
    }

    private static LevelConfig CreateLevelConfig(int levelNumber)
    {
        int width = 10;
        int height = 10;
        List<BloblinConfig> bloblins = new List<BloblinConfig>();
        List<ItemConfig> items = new List<ItemConfig>();

        switch (levelNumber)
        {
            case 1:
                bloblins.Add(new BloblinConfig("Standard", 1, 1));
                bloblins.Add(new BloblinConfig("Standard", 3, 3));
                items.Add(new ItemConfig("Coin", 5, 5));
                items.Add(new ItemConfig("Potion", 7, 7));
                break;
            case 2:
                bloblins.Add(new BloblinConfig("Standard", 2, 2));
                bloblins.Add(new BloblinConfig("Elite", 4, 4));
                items.Add(new ItemConfig("Coin", 6, 6));
                items.Add(new ItemConfig("Gem", 8, 8));
                break;
            default:
                bloblins.Add(new BloblinConfig("Standard", 1, 1));
                items.Add(new ItemConfig("Coin", 5, 5));
                break;
        }

        return new LevelConfig(width, height, bloblins, items);
    }
}
