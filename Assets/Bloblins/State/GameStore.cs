using System;

public class GameStore
{
    private GameState state;
    public GameState State => state;

    public event Action OnRedrawFieldNeeded;
    public event Action OnTurnInfoChanged;

    public GameStore(GameState initialState)
    {
        state = initialState;
    }

    public void Send(GameAction action)
    {
        var oldState = state;
        var newState = GameReducer.Reduce(state, action);
        if (newState != state)
        {
            state = newState;
            OnRedrawFieldNeeded?.Invoke();
            // FIXME: Надо что-то другое сделать
            NotifyTurnInfoChanged(oldState.TurnInfo);
        }
    }

    private void NotifyTurnInfoChanged(TurnState oldTurnState)
    {
        if (oldTurnState != state.TurnInfo)
        {
            OnTurnInfoChanged?.Invoke();
        }
    }
}
