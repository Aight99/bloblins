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
        var oldSelectedBloblinPosition = state.SelectedBloblin?.Position;
        var newState = GameReducer.Reduce(state, action);
        if (newState != state)
        {
            state = newState;
            NotifySelectionChanged(oldSelectedBloblinPosition);
            OnStateChanged?.Invoke();
        }
    }

    private void NotifySelectionChanged(CellPosition? oldPosition)
    {
        var newPosition = state.SelectedBloblin?.Position;
        var isBloblinMoved = oldPosition != newPosition;

        if (isBloblinMoved)
        {
            OnBloblinSelectionChanged?.Invoke();
        }
    }
}
