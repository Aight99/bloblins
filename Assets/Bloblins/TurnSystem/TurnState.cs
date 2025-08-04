public class TurnState
{
    public readonly bool IsPlayerTurn;
    public readonly bool IsPhaseCompleted;

    // TODO: Не финальное условие
    public readonly int EnergyLeft;

    public TurnState(bool isPlayerTurn = true, bool isPhaseCompleted = false, int energyLeft = 3)
    {
        IsPlayerTurn = isPlayerTurn;
        IsPhaseCompleted = isPhaseCompleted;
        EnergyLeft = energyLeft;
    }

    public TurnState WithReducedEnergy()
    {
        if (EnergyLeft <= 0)
            return new(IsPlayerTurn, true, 0);

        return new(IsPlayerTurn, IsPhaseCompleted, EnergyLeft - 1);
    }
}
