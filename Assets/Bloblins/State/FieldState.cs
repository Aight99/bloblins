using System.Collections.Generic;

public class FieldState
{
    public readonly int Width;
    public readonly int Height;
    public readonly Dictionary<CellPosition, IEnvironmentObject> EnvironmentObjects;
    public readonly List<ICreature> Creatures;
    public readonly Dictionary<CellPosition, CellType> CellTypes;

    public FieldState(
        int width,
        int height,
        Dictionary<CellPosition, IEnvironmentObject> environment,
        List<ICreature> creatures,
        Dictionary<CellPosition, CellType> cellTypes
    )
    {
        Width = width;
        Height = height;
        EnvironmentObjects = environment;
        Creatures = creatures;
        CellTypes = cellTypes;
    }

    public FieldState()
    {
        Width = 0;
        Height = 0;
        EnvironmentObjects = new Dictionary<CellPosition, IEnvironmentObject>();
        Creatures = new List<ICreature>();
        CellTypes = new Dictionary<CellPosition, CellType>();
    }

    public FieldState WithEnviroment(CellPosition position, IEnvironmentObject environment)
    {
        var newEnvironment = new Dictionary<CellPosition, IEnvironmentObject>(EnvironmentObjects);
        if (environment == null)
            newEnvironment.Remove(position);
        else
            newEnvironment[position] = environment;
        return new FieldState(Width, Height, newEnvironment, Creatures, CellTypes);
    }

    public FieldState WithMovedBloblin(IBloblin bloblin, CellPosition selectedCell)
    {
        if (!CheckIsReachable(bloblin, selectedCell))
        {
            return this;
        }

        var newEnvironment = new Dictionary<CellPosition, IEnvironmentObject>(EnvironmentObjects);
        newEnvironment.Remove(bloblin.Position);
        newEnvironment[selectedCell] = bloblin;
        bloblin.Position = selectedCell;

        DebugHelper.LogMovement($"топаем на {selectedCell}");
        return new FieldState(Width, Height, newEnvironment, Creatures, CellTypes);
    }

    private bool CheckIsReachable(IBloblin bloblin, CellPosition selectedCell)
    {
        var isReachable = bloblin.CanMoveTo(bloblin.Position, selectedCell);
        var isWalkable = CellTypes[selectedCell].IsWalkable();
        var isOccupied = EnvironmentObjects.ContainsKey(selectedCell);

        if (!isReachable)
        {
            DebugHelper.Log(
                DebugHelper.MessageType.Fiasco,
                $"нельзя пойти на {selectedCell}, слишком далеко"
            );
            return false;
        }

        if (!isWalkable)
        {
            DebugHelper.Log(
                DebugHelper.MessageType.Fiasco,
                $"нельзя пойти на {selectedCell}, {CellTypes[selectedCell]}"
            );
            return false;
        }

        if (isOccupied)
        {
            DebugHelper.Log(
                DebugHelper.MessageType.Fiasco,
                $"нельзя пойти на {selectedCell}, занято"
            );
            return false;
        }
        return true;
    }
}
