using System.Collections.Generic;
using System.Linq;

public static class GameReducer
{
    public static GameState Reduce(GameState state, GameAction action)
    {
        switch (action)
        {
            case LoadLevelAction loadLevel:
                return HandleLoadLevel(loadLevel);

            case CellClickAction cellClick:
                return HandleCellClick(state, cellClick);

            default:
                return state;
        }
    }

    private static GameState HandleLoadLevel(LoadLevelAction action)
    {
        var config = CreateLevelConfig(action.LevelNumber);
        var objects = new Dictionary<CellPosition, IEnvironmentObject>();
        var bloblins = new List<IBloblin>();

        foreach (var bloblinConfig in config.Bloblins)
        {
            var position = new CellPosition(bloblinConfig.X, bloblinConfig.Y);
            IBloblin bloblin = null;

            switch (bloblinConfig.Type)
            {
                case BloblinType.Baldush:
                    bloblin = new Baldush(position);
                    break;
                // Add other bloblin types here when implemented
            }

            if (bloblin != null)
            {
                objects[position] = bloblin;
                bloblins.Add(bloblin);
            }
        }

        foreach (var itemConfig in config.Items)
        {
            var position = new CellPosition(itemConfig.X, itemConfig.Y);
            objects[position] = new Item(itemConfig.Type, position);
        }

        var fieldState = new FieldState(config.Width, config.Height, objects, bloblins);
        return new GameState(fieldState);
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
            DebugHelper.LogMovement($"топаем на [{position.X};{position.Y}]");
            var bloblin = field.Bloblins.First();
            return state.WithField(field.WithMovedBloblin(bloblin, position));
        }

        return state;
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
                bloblins.Add(new BloblinConfig(BloblinType.Baldush, 4, 6));
                break;
            case 2:
                bloblins.Add(new BloblinConfig(BloblinType.Baldush, 2, 2));
                bloblins.Add(new BloblinConfig(BloblinType.Baldush, 4, 4));
                items.Add(new ItemConfig("Coin", 6, 6));
                items.Add(new ItemConfig("Gem", 8, 8));
                break;
            default:
                bloblins.Add(new BloblinConfig(BloblinType.Baldush, 1, 1));
                items.Add(new ItemConfig("Coin", 5, 5));
                break;
        }

        return new LevelConfig(width, height, bloblins, items);
    }
}
