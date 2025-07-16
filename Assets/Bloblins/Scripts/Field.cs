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
        foreach (var pair in fieldState.Entities)
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
                name = $"Bloblin_{bloblin.Type}_{pair.Key.X}_{pair.Key.Y}";
            }
            else if (pair.Value is Item item)
            {
                prefab = itemVisualPrefab;
                name = $"Item_{item.Type}_{pair.Key.X}_{pair.Key.Y}";
            }

            if (prefab != null)
            {
                Vector3 worldPos = GetWorldPosition(pair.Key);
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
        foreach (var pair in fieldState.Entities)
        {
            if (pair.Value is Bloblin bloblin)
            {
                var bloblinPos = new CellPosition(bloblin.X, bloblin.Y);
                if (pair.Key.X != bloblin.X || pair.Key.Y != bloblin.Y)
                {
                    // Блоблин переместился, обновляем его позицию
                    if (entityVisuals.TryGetValue(pair.Key, out var visual))
                    {
                        Vector3 targetPos = GetWorldPosition(bloblinPos);
                        StartCoroutine(MoveVisualCoroutine(visual, targetPos));
                    }
                }
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        float xPos = (x - y) * 0.5f;
        float zPos = (x + y) * 0.25f;
        return new Vector3(xPos, 0, zPos);
    }

    public Vector3 GetWorldPosition(CellPosition position)
    {
        return GetWorldPosition(position.X, position.Y);
    }

    private void OnCellClicked(CellPosition position)
    {
        store.Dispatch(new CellClickAction(position));
    }

    private IEnumerator MoveVisualCoroutine(GameObject visual, Vector3 targetPosition)
    {
        Vector3 startPosition = visual.transform.position;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float moveSpeed = 5f;
        float startTime = Time.time;

        while (Vector3.Distance(visual.transform.position, targetPosition) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            visual.transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                fractionOfJourney
            );

            yield return null;
        }

        visual.transform.position = targetPosition;
    }
}
