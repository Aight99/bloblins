public class GameState
{
    public readonly FieldState Field;
    public readonly IBloblin SelectedBloblin;

    public GameState(FieldState field = null, IBloblin selectedBloblin = null)
    {
        Field = field ?? new FieldState();
        SelectedBloblin = selectedBloblin;
    }

    public GameState WithField(FieldState field) => new(field, SelectedBloblin);

    public GameState WithSelectedBloblin(IBloblin bloblin)
    {
        if (bloblin == SelectedBloblin)
        {
            DebugHelper.LogYippee("Снимаем выделение");
            return new(Field, null);
        }
        DebugHelper.LogYippee($"Выбран {bloblin.Name}");
        return new(Field, bloblin);
    }
}
