using UnityEngine;
using System.Collections;

public class Bloblin : MonoBehaviour, IEntity
{
    [SerializeField] private float moveSpeed = 5f;
    
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
    
    public bool MoveTo(Cell targetCell)
    {
        if (targetCell == null || targetCell.IsOccupied)
            return false;
        
        StartCoroutine(MoveCoroutine(targetCell));
        return true;
    }
    
    private IEnumerator MoveCoroutine(Cell targetCell)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = field.GetWorldPosition(targetCell.X, targetCell.Y);
        
        if (currentCell != null)
            currentCell.ClearOccupant();
        
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;
        
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
            
            yield return null;
        }
        
        transform.position = targetPosition;
        currentCell = targetCell;
        targetCell.SetOccupant(this);
    }
} 