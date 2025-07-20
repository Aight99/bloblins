using System.Collections.Generic;
using UnityEngine;

public static class LevelLoader
{
    public static GameState LoadLevel(int levelNumber)
    {
        var config = Resources.Load<LevelConfig>($"Levels/Level_{levelNumber}");
        if (config == null)
        {
            Debug.LogError($"Level configuration not found for level {levelNumber}");
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
        var bloblins = new List<IBloblin>();
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

        var fieldState = new FieldState(config.Width, config.Height, objects, bloblins, cellTypes);
        return new GameState(fieldState);
    }
}
