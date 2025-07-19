using System.Collections.Generic;

public class GameState
{
    public readonly FieldState Field;

    public GameState(FieldState field)
    {
        Field = field;
    }

    public GameState WithField(FieldState field) => new(field);
}

public class FieldState
{
    public readonly int Width;
    public readonly int Height;
    public readonly Dictionary<CellPosition, IEnvironmentObject> EnvironmentObjects;
    public readonly List<Bloblin> Bloblins;

    public FieldState(
        int width,
        int height,
        Dictionary<CellPosition, IEnvironmentObject> enviroment,
        List<Bloblin> bloblins
    )
    {
        Width = width;
        Height = height;
        EnvironmentObjects = enviroment;
        Bloblins = bloblins;
    }

    public FieldState WithEnviroment(CellPosition position, IEnvironmentObject environment)
    {
        var newEnvironment = new Dictionary<CellPosition, IEnvironmentObject>(EnvironmentObjects);
        if (environment == null)
            newEnvironment.Remove(position);
        else
            newEnvironment[position] = environment;
        return new FieldState(Width, Height, newEnvironment, Bloblins);
    }

    public FieldState WithMovedBloblin(Bloblin bloblin, CellPosition target)
    {
        var newEnvironment = new Dictionary<CellPosition, IEnvironmentObject>(EnvironmentObjects);
        newEnvironment.Remove(bloblin.Position);
        newEnvironment[target] = bloblin;
        bloblin.Position = target;
        return new FieldState(Width, Height, newEnvironment, Bloblins);
    }
}
