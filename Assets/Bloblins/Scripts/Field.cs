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
    private FieldManager fieldManager;
    private Dictionary<Bloblin, GameObject> bloblinVisuals = new Dictionary<Bloblin, GameObject>();
    private Dictionary<Item, GameObject> itemVisuals = new Dictionary<Item, GameObject>();

    public void Initialize(LevelConfig config)
    {
        this.width = config.Width;
        this.height = config.Height;

        fieldManager = new FieldManager(width, height, this);
        CreateGrid();

        foreach (BloblinConfig bloblinConfig in config.Bloblins)
        {
            CreateBloblin(bloblinConfig);
        }

        foreach (ItemConfig itemConfig in config.Items)
        {
            CreateItem(itemConfig);
        }
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

    private void CreateBloblin(BloblinConfig config)
    {
        Bloblin bloblin = new Bloblin(config.Type);
        fieldManager.PlaceEntity(bloblin, new CellPosition(config.X, config.Y));

        GameObject visual = Instantiate(
            bloblinVisualPrefab,
            GetWorldPosition(config.X, config.Y),
            Quaternion.identity,
            transform
        );
        visual.name = $"Bloblin_{config.Type}_{config.X}_{config.Y}";

        bloblinVisuals.Add(bloblin, visual);
    }

    private void CreateItem(ItemConfig config)
    {
        Item item = new Item(config.Type);
        fieldManager.PlaceEntity(item, new CellPosition(config.X, config.Y));

        GameObject visual = Instantiate(
            itemVisualPrefab,
            GetWorldPosition(config.X, config.Y),
            Quaternion.identity,
            transform
        );
        visual.name = $"Item_{config.Type}_{config.X}_{config.Y}";

        itemVisuals.Add(item, visual);
    }

    private void OnCellClicked(CellPosition position)
    {
        fieldManager.OnCellClicked(position);
    }

    public void MoveBloblinVisual(Bloblin bloblin, Vector3 targetPosition)
    {
        if (bloblinVisuals.TryGetValue(bloblin, out GameObject visual))
        {
            StartCoroutine(MoveBloblinVisualCoroutine(visual, targetPosition));
        }
    }

    private IEnumerator MoveBloblinVisualCoroutine(GameObject visual, Vector3 targetPosition)
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

public class LevelConfig
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public List<BloblinConfig> Bloblins { get; private set; }
    public List<ItemConfig> Items { get; private set; }

    public LevelConfig(int width, int height, List<BloblinConfig> bloblins, List<ItemConfig> items)
    {
        Width = width;
        Height = height;
        Bloblins = bloblins;
        Items = items;
    }
}

public class BloblinConfig
{
    public string Type { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    public BloblinConfig(string type, int x, int y)
    {
        Type = type;
        X = x;
        Y = y;
    }
}

public class ItemConfig
{
    public string Type { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    public ItemConfig(string type, int x, int y)
    {
        Type = type;
        X = x;
        Y = y;
    }
}
