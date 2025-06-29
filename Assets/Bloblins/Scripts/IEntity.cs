public interface IEntity
{
    void PlaceOnCell(Cell cell);
    void RemoveFromCell();
    Cell GetCurrentCell();
} 