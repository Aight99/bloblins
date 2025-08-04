public class GameState
{
    public readonly FieldState Field;
    public readonly IEnvironmentObject SelectedObject;
    public readonly TurnState TurnInfo;

    public GameState(
        FieldState field = null,
        IEnvironmentObject selectedObject = null,
        TurnState turnInfo = null
    )
    {
        Field = field ?? new FieldState();
        SelectedObject = selectedObject;
        TurnInfo = turnInfo ?? new TurnState(true, false);
    }

    public GameState WithField(FieldState field) => new(field, SelectedObject);

    public GameState WithSelectedObject(IEnvironmentObject selectedObject) =>
        new(Field, selectedObject);

    public GameState WithTurnInfo(TurnState turnInfo) => new(Field, SelectedObject, turnInfo);
}
