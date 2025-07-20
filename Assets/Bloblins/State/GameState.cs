using System.Collections.Generic;

public class GameState
{
    public readonly FieldState Field;

    public GameState(FieldState field = null)
    {
        Field = field ?? new FieldState();
    }

    public GameState WithField(FieldState field) => new(field);
}

public class FieldState
{
    public readonly int Width;
    public readonly int Height;
    public readonly Dictionary<CellPosition, IEnvironmentObject> EnvironmentObjects;
    public readonly List<IBloblin> Bloblins;
    public readonly Dictionary<CellPosition, CellType> CellTypes;

    public FieldState(
        int width,
        int height,
        Dictionary<CellPosition, IEnvironmentObject> environment,
        List<IBloblin> bloblins,
        Dictionary<CellPosition, CellType> cellTypes
    )
    {
        Width = width;
        Height = height;
        EnvironmentObjects = environment;
        Bloblins = bloblins;
        CellTypes = cellTypes;
    }

    public FieldState()
    {
        Width = 0;
        Height = 0;
        EnvironmentObjects = new Dictionary<CellPosition, IEnvironmentObject>();
        Bloblins = new List<IBloblin>();
        CellTypes = new Dictionary<CellPosition, CellType>();
    }

    public FieldState WithEnviroment(CellPosition position, IEnvironmentObject environment)
    {
        var newEnvironment = new Dictionary<CellPosition, IEnvironmentObject>(EnvironmentObjects);
        if (environment == null)
            newEnvironment.Remove(position);
        else
            newEnvironment[position] = environment;
        return new FieldState(Width, Height, newEnvironment, Bloblins, CellTypes);
    }

    public FieldState WithMovedBloblin(IBloblin bloblin, CellPosition selectedCell)
    {
        var positionToMove = bloblin.GetMoveTarget(selectedCell);

        if (!CellTypes[positionToMove].CanMoveTo())
            return this;

        var newEnvironment = new Dictionary<CellPosition, IEnvironmentObject>(EnvironmentObjects);
        newEnvironment.Remove(bloblin.Position);
        newEnvironment[positionToMove] = bloblin;
        bloblin.Position = positionToMove;

        return new FieldState(Width, Height, newEnvironment, Bloblins, CellTypes);
    }
}
