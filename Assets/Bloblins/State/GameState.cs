public class GameState
{
    public readonly FieldState Field;
    public readonly IEnvironmentObject SelectedObject;
    public readonly TurnQueue TurnQueue;

    public GameState(FieldState field, IEnvironmentObject selectedObject, TurnQueue turnQueue)
    {
        Field = field ?? new FieldState();
        SelectedObject = selectedObject;
        TurnQueue = turnQueue ?? new TurnQueue();
    }

    public GameState WithField(FieldState field) => new(field, SelectedObject, TurnQueue);

    public GameState WithSelectedObject(IEnvironmentObject selectedObject) =>
        new(Field, selectedObject, TurnQueue);

    public GameState WithNextTurn() =>
        new(Field, SelectedObject, TurnQueue.WithNextTurn());
}
