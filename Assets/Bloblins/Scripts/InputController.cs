using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private Field field;

    private Bloblin selectedBloblin;

    private void OnEnable()
    {
        Cell.OnCellClicked += HandleCellClick;
    }

    private void OnDisable()
    {
        Cell.OnCellClicked -= HandleCellClick;
    }

    private void HandleCellClick(Cell clickedCell)
    {
        if (clickedCell == null)
            return;

        if (clickedCell.IsOccupied)
        {
            IEntity entity = clickedCell.GetOccupant();

            if (entity is Bloblin bloblin)
            {
                SelectBloblin(bloblin);
            }
            else if (entity is Item item && selectedBloblin != null)
            {
                if (IsAdjacent(selectedBloblin.GetCurrentCell(), clickedCell))
                {
                    item.Interact(selectedBloblin);
                }
            }
        }
        else if (selectedBloblin != null)
        {
            selectedBloblin.MoveTo(clickedCell);
        }
    }

    private void SelectBloblin(Bloblin bloblin)
    {
        if (selectedBloblin != null) { }

        selectedBloblin = bloblin;

        Debug.Log($"Выбран Bloblin: {bloblin.gameObject.name}");
    }

    private bool IsAdjacent(Cell cell1, Cell cell2)
    {
        if (cell1 == null || cell2 == null)
            return false;

        int dx = Mathf.Abs(cell1.X - cell2.X);
        int dy = Mathf.Abs(cell1.Y - cell2.Y);

        return dx <= 1 && dy <= 1 && dx + dy > 0;
    }
}
