public class Baldush : IBloblin
{
    public CellPosition Position { get; set; }
    public string Name { get; private set; }
    public int MoveRange => 3;

    public Baldush(CellPosition position)
    {
        Name = "Балдуш";
        Position = position;
    }
}
