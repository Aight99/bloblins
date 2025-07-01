using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField]
    private int width = 10;

    [SerializeField]
    private int height = 10;

    [SerializeField]
    private GameObject cellPrefab;

    private Cell[,] grid;

    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Cell[width, height];

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

                cell.Initialize(x, y);
                grid[x, y] = cell;
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return grid[x, y];
        return null;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        float xPos = (x - y) * 0.5f;
        float zPos = (x + y) * 0.25f;
        return new Vector3(xPos, 0, zPos);
    }

    public Cell GetCellFromWorldPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x + worldPosition.z * 2);
        int y = Mathf.RoundToInt(worldPosition.z * 2 - worldPosition.x);

        return GetCell(x, y);
    }
}
