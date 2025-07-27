using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [Header("Cell Prefabs")]
    [SerializeField]
    private GameObject groundCellPrefab;

    [SerializeField]
    private GameObject waterCellPrefab;

    [Space(5)]
    [Header("Entity Prefabs")]
    [SerializeField]
    private GameObject bloblinVisualPrefab;

    [SerializeField]
    private GameObject itemVisualPrefab;

    [Space(5)]
    [Header("Layout")]
    [SerializeField]
    private float cellsXShift = 1f;

    [SerializeField]
    private float cellsZShift = 1f;

    [Space(5)]
    [Header("Debug")]
    [SerializeField]
    private float gizmoHeight = 0.5f;

    private int width;
    private int height;
    private Cell[,] cells;
    private GameStore store;
    private Dictionary<CellPosition, GameObject> entityVisuals =
        new Dictionary<CellPosition, GameObject>();
    private Dictionary<CellType, GameObject> cellPrefabs = new Dictionary<CellType, GameObject>();
    private HashSet<Cell> highlightedCells = new HashSet<Cell>();

    public void Initialize(GameStore store)
    {
        this.store = store;
        store.OnStateChanged += OnStateChanged;
        store.OnBloblinSelectionChanged += OnBloblinSelectionChanged;
    }

    private void Awake()
    {
        cellPrefabs[CellType.Ground] = groundCellPrefab;
        cellPrefabs[CellType.Water] = waterCellPrefab;
    }

    [ContextMenu("Redraw Grid")]
    private void RedrawGrid()
    {
        ClearGrid();
        CreateGrid();
    }

    private void OnBloblinSelectionChanged()
    {
        ClearHighlightedCells();
        HighlightCells();
    }

    private void ClearHighlightedCells()
    {
        foreach (var cell in highlightedCells)
        {
            cell.SetHighlight(false);
        }

        highlightedCells.Clear();
    }

    private void HighlightCells()
    {
        var selectedBloblin = store.State.SelectedBloblin;
        if (selectedBloblin != null)
        {
            var moveRange = selectedBloblin.MoveRange;
            for (int x = -moveRange; x <= moveRange; x++)
            {
                for (int y = -moveRange; y <= moveRange; y++)
                {
                    var newX = selectedBloblin.Position.X + x;
                    var newY = selectedBloblin.Position.Y + y;
                    if (newX < 0 || newX >= width || newY < 0 || newY >= height)
                        continue;

                    var cell = cells[newX, newY];
                    if (cell != null)
                    {
                        highlightedCells.Add(cell);
                        cell.SetHighlight(true);
                    }
                }
            }
        }
    }

    private void OnStateChanged()
    {
        var field = store.State.Field;

        if (width != field.Width || height != field.Height)
        {
            ClearGrid();
            width = field.Width;
            height = field.Height;
            CreateGrid();
        }

        UpdateEntityVisuals(field);
    }

    private void ClearGrid()
    {
        if (cells != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (cells[x, y] != null)
                    {
                        Destroy(cells[x, y].gameObject);
                    }
                }
            }
        }

        foreach (var visual in entityVisuals.Values)
        {
            Destroy(visual);
        }

        entityVisuals.Clear();
    }

    private void CreateGrid()
    {
        cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CellType cellType = GetCellTypeFromState(x, y);
                if (cellType == CellType.Void)
                    continue;

                Vector3 position = GetWorldPosition(x, y);

                GameObject cellObject = Instantiate(
                    cellPrefabs[cellType],
                    position,
                    Quaternion.identity,
                    transform
                );
                cellObject.name = $"Cell_{x}_{y}_{cellType}";

                Cell cell = cellObject.GetComponent<Cell>();
                if (cell == null)
                    cell = cellObject.AddComponent<Cell>();

                cell.Initialize(x, y, OnCellClicked);
                cells[x, y] = cell;
            }
        }
    }

    private CellType GetCellTypeFromState(int x, int y)
    {
        var position = new CellPosition(x, y);
        return store.State.Field.CellTypes[position];
    }

    private void UpdateEntityVisuals(FieldState fieldState)
    {
        var currentPositions = new HashSet<CellPosition>();
        foreach (var pair in fieldState.EnvironmentObjects)
        {
            currentPositions.Add(pair.Key);

            if (entityVisuals.ContainsKey(pair.Key))
                continue;

            GameObject prefab = null;
            string name = "";

            if (pair.Value is IBloblin bloblin)
            {
                prefab = bloblinVisualPrefab;
                name = $"Bloblin_{bloblin.Name}_{pair.Key.X}_{pair.Key.Y}";
            }
            else if (pair.Value is Item item)
            {
                prefab = itemVisualPrefab;
                name = $"Item_{item.Type}_{pair.Key.X}_{pair.Key.Y}";
            }

            if (prefab != null)
            {
                Vector3 worldPos = GetWorldPosition(pair.Value);
                GameObject visual = Instantiate(prefab, worldPos, Quaternion.identity, transform);
                visual.name = name;

                EntityVisual entityVisual = visual.GetComponent<EntityVisual>();
                if (entityVisual == null)
                    entityVisual = visual.AddComponent<EntityVisual>();

                entityVisual.Initialize(pair.Value, OnCellClicked);
                entityVisuals[pair.Key] = visual;
            }
        }

        var positionsToRemove = new List<CellPosition>();
        foreach (var pair in entityVisuals)
        {
            if (!currentPositions.Contains(pair.Key))
            {
                Destroy(pair.Value);
                positionsToRemove.Add(pair.Key);
            }
        }

        foreach (var pos in positionsToRemove)
        {
            entityVisuals.Remove(pos);
        }

        foreach (var bloblin in fieldState.Bloblins)
        {
            if (entityVisuals.TryGetValue(bloblin.Position, out var visual))
            {
                Vector3 targetPos = GetWorldPosition(bloblin);
                visual.transform.position = targetPos;
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y, float layer = Layers.Land)
    {
        float xPos = x * -cellsXShift;
        float zPos = y * cellsZShift;
        return new Vector3(xPos, layer, zPos);
    }

    public Vector3 GetWorldPosition(IEnvironmentObject obj)
    {
        return GetWorldPosition(obj.Position.X, obj.Position.Y, obj.DrawLayer);
    }

    private void OnCellClicked(CellPosition position)
    {
        store.Send(new CellClickAction(position));
    }

    private void OnDrawGizmos()
    {
        if (cells == null)
            return;

        Gizmos.color = Color.white;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (cells[x, y] != null)
                {
                    Vector3 position = GetWorldPosition(x, y);
                    position.y += gizmoHeight;

                    UnityEditor.Handles.color = Color.black;
                    UnityEditor.Handles.Label(position, $"{x},{y}");
                }
            }
        }
    }
}
