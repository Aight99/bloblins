using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Bloblins/Level Config", order = 1)]
public class LevelConfig : ScriptableObject
{
    [SerializeField]
    private List<BloblinConfig> bloblins = new List<BloblinConfig>();

    [SerializeField]
    private List<EnemyConfig> enemies = new List<EnemyConfig>();

    [SerializeField]
    private List<ItemConfig> items = new List<ItemConfig>();

    [SerializeField, TextArea(10, 20)]
    private string mapLayout;

    public int Width
    {
        get
        {
            if (string.IsNullOrEmpty(mapLayout))
                return 0;

            string[] rows = mapLayout.Split('\n');
            int maxWidth = 0;
            foreach (string row in rows)
            {
                if (row.TrimEnd().Length > maxWidth)
                    maxWidth = row.TrimEnd().Length;
            }
            return maxWidth;
        }
    }

    public int Height
    {
        get
        {
            if (string.IsNullOrEmpty(mapLayout))
                return 0;

            return mapLayout.Split('\n').Length;
        }
    }

    public string MapLayout => mapLayout;
    public List<BloblinConfig> Bloblins => bloblins;
    public List<EnemyConfig> Enemies => enemies;
    public List<ItemConfig> Items => items;

    public CellType GetCellTypeAt(int x, int y)
    {
        if (string.IsNullOrEmpty(mapLayout))
            return CellType.Ground;

        string[] rows = mapLayout.Split('\n');

        if (y < 0 || y >= rows.Length || x < 0)
            return CellType.Ground;

        string row = rows[y].TrimEnd();
        if (x >= row.Length)
            return CellType.Void;

        char cellChar = row[x];
        return GetCellTypeFromChar(cellChar);
    }

    private CellType GetCellTypeFromChar(char c)
    {
        switch (c)
        {
            case 'G':
                return CellType.Ground;
            case 'W':
                return CellType.Water;
            case '_':
                return CellType.Void;
            default:
                throw new System.NotImplementedException();
        }
    }
}

[System.Serializable]
public class BloblinConfig
{
    [SerializeField]
    private BloblinType type;

    [SerializeField]
    private int x;

    [SerializeField]
    private int y;

    public BloblinType Type => type;
    public int X => x;
    public int Y => y;
}

[System.Serializable]
public class EnemyConfig
{
    [SerializeField]
    private EnemyType type;

    [SerializeField]
    private int x;

    [SerializeField]
    private int y;

    public EnemyType Type => type;
    public int X => x;
    public int Y => y;
}

[System.Serializable]
public class ItemConfig
{
    [SerializeField]
    private string type;

    [SerializeField]
    private int x;

    [SerializeField]
    private int y;

    public string Type => type;
    public int X => x;
    public int Y => y;
}
