using UnityEngine;

public class Item : MonoBehaviour, IEntity
{
    private Cell currentCell;
    private Field field;
    
    public void Initialize(Field fieldReference)
    {
        field = fieldReference;
    }
    
    public Cell GetCurrentCell()
    {
        return currentCell;
    }
    
    public void PlaceOnCell(Cell cell)
    {
        if (currentCell != null)
            currentCell.ClearOccupant();
            
        currentCell = cell;
        cell.SetOccupant(this);
        
        transform.position = field.GetWorldPosition(cell.X, cell.Y);
    }
    
    public void RemoveFromCell()
    {
        if (currentCell != null)
        {
            currentCell.ClearOccupant();
            currentCell = null;
        }
    }
    
    public virtual void Interact(Bloblin bloblin)
    {
        Debug.Log($"Bloblin взаимодействует с {gameObject.name}");
    }
} 