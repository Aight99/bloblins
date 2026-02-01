using System.Collections.Generic;

public class CellHighlighter
{
    private readonly HashSet<Cell> highlightedCells = new HashSet<Cell>();

    public void UpdateHighlights(
        IEnvironmentObject selectedObject,
        Cell[,] cells,
        FieldState fieldState
    )
    {
        ClearHighlights();

        if (selectedObject == null)
            return;

        if (selectedObject is IBloblin bloblin)
        {
            HighlightBloblinMoveRange(bloblin, cells, fieldState);
        }
        else
        {
            HighlightSelectedCell(selectedObject, cells);
        }
    }

    private void HighlightBloblinMoveRange(IBloblin bloblin, Cell[,] cells, FieldState fieldState)
    {
        var moveRange = bloblin.MoveRange;
        for (int x = -moveRange; x <= moveRange; x++)
        {
            for (int y = -moveRange; y <= moveRange; y++)
            {
                var newPosition = new CellPosition(
                    bloblin.Position.X + x,
                    bloblin.Position.Y + y
                );

                if (!fieldState.CellTypes.ContainsKey(newPosition))
                    continue;
                if (!fieldState.CellTypes[newPosition].IsWalkable())
                    continue;
                if (!bloblin.CanMoveTo(bloblin.Position, newPosition))
                    continue;
                if (fieldState.EnvironmentObjects.ContainsKey(newPosition))
                    continue;

                var cell = cells[newPosition.X, newPosition.Y];
                HighlightCell(cell);
            }
        }
    }

    private void HighlightSelectedCell(IEnvironmentObject environmentObject, Cell[,] cells)
    {
        var cell = cells[environmentObject.Position.X, environmentObject.Position.Y];
        HighlightCell(cell);
    }

    private void HighlightCell(Cell cell)
    {
        if (cell != null)
        {
            highlightedCells.Add(cell);
            cell.SetHighlight(true);
        }
    }

    private void ClearHighlights()
    {
        foreach (var cell in highlightedCells)
        {
            cell.SetHighlight(false);
        }

        highlightedCells.Clear();
    }
}
