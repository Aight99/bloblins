using UnityEngine;
using System.Collections.Generic;

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
    private GameObject creatureVisualPrefab;

    [SerializeField]
    private GameObject itemVisualPrefab;

    [Space(5)]
    [Header("Layout")]
    [SerializeField]
    private float cellsXShift = 1f;

    [SerializeField]
    private float cellsZShift = 1f;

    [Space(5)]
    [Header("Animation")]
    [SerializeField]
    private float moveSpeed = 5f;

    [Space(5)]
    [Header("Debug")]
    [SerializeField]
    private float gizmoHeight = 0.5f;

    private int width;
    private int height;
    private GameStore store;

    private GridManager gridManager;
    private EntityVisualManager entityVisualManager;
    private CellHighlighter cellHighlighter;

    public void Initialize(GameStore store)
    {
        this.store = store;
        store.OnRedrawFieldNeeded += RedrawField;
    }

    private void Awake()
    {
        var cellPrefabs = new Dictionary<CellType, GameObject>
        {
            [CellType.Ground] = groundCellPrefab,
            [CellType.Water] = waterCellPrefab
        };

        GameObject queueObject = new GameObject("AnimationQueue");
        queueObject.transform.SetParent(transform);
        AnimationQueue animationQueue = queueObject.AddComponent<AnimationQueue>();

        gridManager = new GridManager(transform, cellPrefabs, GetWorldPosition, GetCellTypeFromState);

        entityVisualManager = new EntityVisualManager(
            transform,
            creatureVisualPrefab,
            itemVisualPrefab,
            animationQueue,
            moveSpeed,
            GetWorldPosition
        );

        cellHighlighter = new CellHighlighter();
    }

    [ContextMenu("Redraw Grid")]
    private void RedrawGrid()
    {
        gridManager.ClearGrid();
        gridManager.CreateGrid(width, height, OnCellClicked);
    }

    private void RedrawField()
    {
        var field = store.State.Field;

        if (width != field.Width || height != field.Height)
        {
            width = field.Width;
            height = field.Height;
            gridManager.CreateGrid(width, height, OnCellClicked);
        }

        entityVisualManager.UpdateVisuals(field, OnCellClicked);
        cellHighlighter.UpdateHighlights(store.State.SelectedObject, gridManager.Cells, field);
    }


    private CellType GetCellTypeFromState(int x, int y)
    {
        var position = new CellPosition(x, y);
        return store.State.Field.CellTypes[position];
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
        if (gridManager?.Cells == null)
            return;

        Gizmos.color = Color.white;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gridManager.Cells[x, y] != null)
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
