using System;
using System.Collections.Generic;

public class GameStore
{
    private GameState state;
    public GameState State => state;

    public event Action<GameState> OnStateChanged;

    public GameStore(GameState initialState)
    {
        state = initialState;
    }

    public void Dispatch(GameAction action)
    {
        var newState = GameReducer.Reduce(state, action);
        if (newState != state)
        {
            state = newState;
            OnStateChanged?.Invoke(state);
        }
    }
}
