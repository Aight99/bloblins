public class GraySolder : IEnemy
{
    public CellPosition Position { get; set; }
    public string Name { get; private set; }
    public int MoveRange => 2;
    public IEnemyBehavior Behavior { get; private set; }

    public GraySolder(CellPosition position, IEnemyBehavior behavior)
    {
        Name = "Серый солдат";
        Position = position;
        Behavior = behavior;
    }
}
