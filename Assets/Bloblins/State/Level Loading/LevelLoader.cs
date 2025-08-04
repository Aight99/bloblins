using System.Collections.Generic;
using UnityEngine;

public static class LevelLoader
{
    public static GameState LoadLevel(string levelName)
    {
        var config = Resources.Load<LevelConfig>($"Levels/{levelName}");
        if (config == null)
        {
            Debug.LogError($"Level configuration not found for level {levelName}");
            return null;
        }

        return CreateGameStateFromConfig(config);
    }

    public static GameState LoadLevel(LevelConfig config)
    {
        if (config == null)
        {
            Debug.LogError("Level configuration is null");
            return null;
        }

        return CreateGameStateFromConfig(config);
    }

    private static GameState CreateGameStateFromConfig(LevelConfig config)
    {
        var objects = new Dictionary<CellPosition, IEnvironmentObject>();
        var creatures = new List<ICreature>();
        var cellTypes = new Dictionary<CellPosition, CellType>();

        for (int x = 0; x < config.Width; x++)
        {
            for (int y = 0; y < config.Height; y++)
            {
                var position = new CellPosition(x, y);
                CellType cellType = config.GetCellTypeAt(x, y);
                cellTypes[position] = cellType;
            }
        }

        foreach (var bloblinConfig in config.Bloblins)
        {
            var position = new CellPosition(bloblinConfig.X, bloblinConfig.Y);
            ICreature creature = null;

            switch (bloblinConfig.Type)
            {
                case BloblinType.Baldush:
                    creature = new Baldush(position);
                    break;
                default:
                    throw new System.NotImplementedException();
            }

            if (creature != null)
            {
                objects[position] = creature;
                creatures.Add(creature);
            }
        }

        foreach (var itemConfig in config.Items)
        {
            var position = new CellPosition(itemConfig.X, itemConfig.Y);
            objects[position] = new Item(itemConfig.Type, position);
        }

        var fieldState = new FieldState(config.Width, config.Height, objects, creatures, cellTypes);
        return new GameState(fieldState, null, null);
    }
}
