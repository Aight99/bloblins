public class TurnQueue
{
    // public readonly Queue<TurnState> Turns;

    public TurnQueue()
    {
        // Turns = new Queue<TurnState>();
    }

    public TurnQueue WithNextTurn()
    {
        return new TurnQueue();
    }
}
