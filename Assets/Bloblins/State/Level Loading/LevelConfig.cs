using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Bloblins/Level Config", order = 1)]
public class LevelConfig : ScriptableObject
{
    [SerializeField]
    private int width = 10;

    [SerializeField]
    private int height = 10;

    [SerializeField]
    private List<BloblinConfig> bloblins = new List<BloblinConfig>();

    [SerializeField]
    private List<ItemConfig> items = new List<ItemConfig>();

    [SerializeField, TextArea(10, 20)]
    private string mapLayout;

    public int Width => width;
    public int Height => height;
    public string MapLayout => mapLayout;
    public List<BloblinConfig> Bloblins => bloblins;
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
            return CellType.Ground;

        char cellChar = row[x];
        return GetCellTypeFromChar(cellChar);
    }

    private CellType GetCellTypeFromChar(char c)
    {
        switch (c)
        {
            case 'W':
                return CellType.Water;
            default:
                return CellType.Ground;
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
