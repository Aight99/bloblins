using System.Collections.Generic;

public class GameState
{
    public readonly FieldState Field;
    public readonly int CurrentLevel;

    public GameState(FieldState field, int currentLevel)
    {
        Field = field;
        CurrentLevel = currentLevel;
    }

    public GameState WithField(FieldState field) => new GameState(field, CurrentLevel);

    public GameState WithLevel(int level) => new GameState(Field, level);
}

public class FieldState
{
    public readonly int Width;
    public readonly int Height;
    public readonly Dictionary<CellPosition, IEntity> Entities;
    public readonly Bloblin SelectedBloblin;

    public FieldState(
        int width,
        int height,
        Dictionary<CellPosition, IEntity> entities,
        Bloblin selectedBloblin = null
    )
    {
        Width = width;
        Height = height;
        Entities = entities;
        SelectedBloblin = selectedBloblin;
    }

    public FieldState WithSelectedBloblin(Bloblin bloblin) =>
        new FieldState(Width, Height, Entities, bloblin);

    public FieldState WithEntity(CellPosition position, IEntity entity)
    {
        var newEntities = new Dictionary<CellPosition, IEntity>(Entities);
        if (entity == null)
            newEntities.Remove(position);
        else
            newEntities[position] = entity;
        return new FieldState(Width, Height, newEntities, SelectedBloblin);
    }

    public FieldState WithMovedEntity(CellPosition from, CellPosition to)
    {
        if (!Entities.TryGetValue(from, out var entity))
            return this;

        var newEntities = new Dictionary<CellPosition, IEntity>(Entities);
        newEntities.Remove(from);
        newEntities[to] = entity;

        if (entity is Bloblin bloblin)
        {
            bloblin.X = to.X;
            bloblin.Y = to.Y;
        }

        return new FieldState(Width, Height, newEntities, SelectedBloblin);
    }
}
