using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Field field;

    private void Start()
    {
        LoadLevel(1);
    }

    private void LoadLevel(int levelNumber)
    {
        LevelConfig config = CreateLevelConfig(levelNumber);
        field.Initialize(config);
    }

    private LevelConfig CreateLevelConfig(int levelNumber)
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
