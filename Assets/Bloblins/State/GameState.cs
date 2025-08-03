public class GameState
{
    public readonly FieldState Field;
    public readonly IEnvironmentObject SelectedObject;

    public GameState(FieldState field = null, IEnvironmentObject selectedObject = null)
    {
        Field = field ?? new FieldState();
        SelectedObject = selectedObject;
    }

    public GameState WithField(FieldState field) => new(field, SelectedObject);

    public GameState WithSelectedObject(IEnvironmentObject selectedObject) =>
        new(Field, selectedObject);
}
