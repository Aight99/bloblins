using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    private readonly Transform parent;
    private readonly Dictionary<CellType, GameObject> cellPrefabs;
    private readonly Func<int, int, float, Vector3> getWorldPosition;
    private readonly Func<int, int, CellType> getCellType;

    private Cell[,] cells;
    private int width;
    private int height;

    public Cell[,] Cells => cells;

    public GridManager(
        Transform parent,
        Dictionary<CellType, GameObject> cellPrefabs,
        Func<int, int, float, Vector3> getWorldPosition,
        Func<int, int, CellType> getCellType
    )
    {
        this.parent = parent;
        this.cellPrefabs = cellPrefabs;
        this.getWorldPosition = getWorldPosition;
        this.getCellType = getCellType;
    }

    public void CreateGrid(int width, int height, Action<CellPosition> onCellClicked)
    {
        ClearGrid();
        this.width = width;
        this.height = height;
        cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CellType cellType = getCellType(x, y);
                if (cellType == CellType.Void)
                    continue;

                Vector3 position = getWorldPosition(x, y, Layers.Land);

                GameObject cellObject = UnityEngine.Object.Instantiate(
                    cellPrefabs[cellType],
                    position,
                    Quaternion.identity,
                    parent
                );
                cellObject.name = $"Cell_{x}_{y}_{cellType}";

                Cell cell = cellObject.GetComponent<Cell>();
                if (cell == null)
                    cell = cellObject.AddComponent<Cell>();

                cell.Initialize(x, y, onCellClicked);
                cells[x, y] = cell;
            }
        }
    }

    public void ClearGrid()
    {
        if (cells != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (cells[x, y] != null)
                    {
                        UnityEngine.Object.Destroy(cells[x, y].gameObject);
                    }
                }
            }
        }
    }
}
