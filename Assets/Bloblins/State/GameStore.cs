using System;

public class GameStore
{
    private GameState state;
    public GameState State => state;

    public event Action OnRedrawFieldNeeded;

    public GameStore(GameState initialState)
    {
        state = initialState;
    }

    public void Send(GameAction action)
    {
        var oldState = state;

        HadleAction(action);

        if (oldState != state)
        {
            OnRedrawFieldNeeded?.Invoke();
        }
    }

    private void HadleAction(GameAction action)
    {
        var feedback = GameReducer.Reduce(ref state, action);
        while (feedback != null)
        {
            feedback = GameReducer.Reduce(ref state, feedback);
        }
    }
}
