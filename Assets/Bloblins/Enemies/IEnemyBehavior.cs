public interface IEnemyBehavior
{
    CellPosition DecideNextMove(IEnemy enemy, GameState state);
}
