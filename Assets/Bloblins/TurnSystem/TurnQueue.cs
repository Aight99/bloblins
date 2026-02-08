using System.Collections.Generic;

public class TurnQueue
{
    private readonly IReadOnlyList<ICreature> creatures;

    public TurnQueue()
    {
        creatures = new List<ICreature>();
    }

    public TurnQueue(IReadOnlyList<ICreature> creatures)
    {
        this.creatures = creatures ?? new List<ICreature>();
    }

    public ICreature CurrentCreature => creatures.Count > 0 ? creatures[0] : null;

    public TurnQueue WithNextTurn()
    {
        if (creatures.Count == 0)
            return this;

        var newCreatures = new List<ICreature>(creatures);
        var first = newCreatures[0];
        newCreatures.RemoveAt(0);
        newCreatures.Add(first);

        return new TurnQueue(newCreatures);
    }
}
