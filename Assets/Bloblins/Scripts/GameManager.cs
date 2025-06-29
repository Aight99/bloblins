using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Field field;
    [SerializeField] private GameObject bloblinPrefab;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int initialBloblinCount = 3;
    
    private List<Bloblin> bloblins = new List<Bloblin>();
    private List<Item> items = new List<Item>();
    
    void Start()
    {
        if (field == null)
        {
            Debug.LogError("Field reference is not set in GameManager!");
            return;
        }
        
        Invoke("SpawnEntities", 0.1f);
    }
    
    private void SpawnEntities()
    {
        for (int i = 0; i < initialBloblinCount; i++)
        {
            SpawnBloblin(GetRandomEmptyCell());
        }
        
        for (int i = 0; i < initialBloblinCount; i++)
        {
            SpawnItem(GetRandomEmptyCell());
        }
    }
    
    private Cell GetRandomEmptyCell()
    {
        int maxAttempts = 100;
        int attempts = 0;
        
        while (attempts < maxAttempts)
        {
            int x = Random.Range(0, 10);
            int y = Random.Range(0, 10);
            
            Cell cell = field.GetCell(x, y);
            if (cell != null && !cell.IsOccupied)
            {
                return cell;
            }
            
            attempts++;
        }
        
        Debug.LogWarning("Could not find empty cell after " + maxAttempts + " attempts");
        return null;
    }
    
    private Bloblin SpawnBloblin(Cell cell)
    {
        if (cell == null || cell.IsOccupied || bloblinPrefab == null)
            return null;
            
        GameObject bloblinObject = Instantiate(bloblinPrefab);
        Bloblin bloblin = bloblinObject.GetComponent<Bloblin>();
        
        if (bloblin != null)
        {
            bloblin.Initialize(field);
            bloblin.PlaceOnCell(cell);
            bloblins.Add(bloblin);
            return bloblin;
        }
        
        return null;
    }
    
    private Item SpawnItem(Cell cell)
    {
        if (cell == null || cell.IsOccupied || itemPrefab == null)
            return null;
            
        GameObject itemObject = Instantiate(itemPrefab);
        Item item = itemObject.GetComponent<Item>();
        
        if (item != null)
        {
            item.Initialize(field);
            item.PlaceOnCell(cell);
            items.Add(item);
            return item;
        }
        
        return null;
    }
} 