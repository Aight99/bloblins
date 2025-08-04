public class TurnState
{
    public readonly bool IsPlayerTurn;
    public bool IsPhaseCompleted => EnergyLeft <= 0;

    // TODO: Не финальное условие
    public readonly int EnergyLeft;

    public TurnState(bool isPlayerTurn = true, int energyLeft = 3)
    {
        IsPlayerTurn = isPlayerTurn;
        EnergyLeft = energyLeft;
    }

    public TurnState WithReducedEnergy()
    {
        return new(IsPlayerTurn, EnergyLeft - 1);
    }
}
