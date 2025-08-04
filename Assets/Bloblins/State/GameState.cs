public class GameState
{
    public readonly FieldState Field;
    public readonly IEnvironmentObject SelectedObject;
    public readonly TurnState TurnInfo;

    public GameState(FieldState field, IEnvironmentObject selectedObject, TurnState turnInfo)
    {
        Field = field ?? new FieldState();
        SelectedObject = selectedObject;
        TurnInfo = turnInfo ?? new TurnState(true);
    }

    public GameState WithField(FieldState field) => new GameState(field, SelectedObject, TurnInfo);

    public GameState WithSelectedObject(IEnvironmentObject selectedObject) =>
        new GameState(Field, selectedObject, TurnInfo);

    public GameState WithTurnInfo(TurnState turnInfo) =>
        new GameState(Field, SelectedObject, turnInfo);
}
