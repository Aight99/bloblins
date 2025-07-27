using System.Collections.Generic;

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
        var isReachable = bloblin.CanMoveTo(bloblin.Position, selectedCell);
        var isWalkable = CellTypes[selectedCell].IsWalkable();

        if (!isReachable || !isWalkable)
        {
            DebugHelper.Log(
                DebugHelper.MessageType.Fiasco,
                $"нельзя пойти на {selectedCell} (тип: {CellTypes[selectedCell]})"
            );
            return this;
        }

        var newEnvironment = new Dictionary<CellPosition, IEnvironmentObject>(EnvironmentObjects);
        newEnvironment.Remove(bloblin.Position);
        newEnvironment[selectedCell] = bloblin;
        bloblin.Position = selectedCell;

        DebugHelper.LogMovement($"топаем на {selectedCell}");
        return new FieldState(Width, Height, newEnvironment, Bloblins, CellTypes);
    }
}
