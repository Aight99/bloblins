using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField]
    private GameObject cellPrefab;

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

    public void Initialize(GameStore store)
    {
        this.store = store;
        store.OnStateChanged += OnStateChanged;
    }

    [ContextMenu("Redraw Grid")]
    private void RedrawGrid()
    {
        ClearGrid();
        CreateGrid();
    }

    private void OnStateChanged(GameState state)
    {
        var field = state.Field;

        // Если размеры поля изменились, пересоздаем сетку
        if (width != field.Width || height != field.Height)
        {
            ClearGrid();
            width = field.Width;
            height = field.Height;
            CreateGrid();
        }

        // Обновляем визуальное представление сущностей
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
                Vector3 position = GetWorldPosition(x, y);
                GameObject cellObject = Instantiate(
                    cellPrefab,
                    position,
                    Quaternion.identity,
                    transform
                );
                cellObject.name = $"Cell_{x}_{y}";

                Cell cell = cellObject.GetComponent<Cell>();
                if (cell == null)
                    cell = cellObject.AddComponent<Cell>();

                cell.Initialize(x, y, OnCellClicked);
                cells[x, y] = cell;
            }
        }
    }

    private void UpdateEntityVisuals(FieldState fieldState)
    {
        // Собираем текущие позиции
        var currentPositions = new HashSet<CellPosition>();
        foreach (var pair in fieldState.EnvironmentObjects)
        {
            currentPositions.Add(pair.Key);

            // Если визуал уже существует в этой позиции, пропускаем
            if (entityVisuals.ContainsKey(pair.Key))
                continue;

            // Создаем новый визуал
            GameObject prefab = null;
            string name = "";

            if (pair.Value is Bloblin bloblin)
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
                entityVisuals[pair.Key] = visual;
            }
        }

        // Удаляем визуалы для позиций, где больше нет сущностей
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

        // Обрабатываем перемещения сущностей
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
        float xPos = x * cellsXShift;
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
