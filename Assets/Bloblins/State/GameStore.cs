using System;

public class GameStore
{
    private GameState state;
    public GameState State => state;

    public event Action OnStateChanged;
    public event Action OnBloblinSelectionChanged;

    public GameStore(GameState initialState)
    {
        state = initialState;
    }

    public void Send(GameAction action)
    {
        var oldSelectedObjectPosition = state.SelectedObject?.Position;
        var newState = GameReducer.Reduce(state, action);
        if (newState != state)
        {
            state = newState;
            NotifySelectionChanged(oldSelectedObjectPosition);
            OnStateChanged?.Invoke();
        }
    }

    private void NotifySelectionChanged(CellPosition? oldPosition)
    {
        var newPosition = state.SelectedObject?.Position;
        var isObjectMoved = oldPosition != newPosition;

        if (isObjectMoved)
        {
            OnBloblinSelectionChanged?.Invoke();
        }
    }
}
