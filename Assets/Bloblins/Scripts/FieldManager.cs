using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager
{
    public event Action<Bloblin> OnBloblinSelected;
    public event Action<Item, Bloblin> OnItemInteraction;

    private readonly int width;
    private readonly int height;
    private readonly IEntity[,] entities;
    private Bloblin selectedBloblin;
    private readonly Field field;

    public FieldManager(int width, int height, Field field)
    {
        this.width = width;
        this.height = height;
        this.field = field;
        entities = new IEntity[width, height];
    }

    public void OnCellClicked(CellPosition position)
    {
        if (position.X < 0 || position.X >= width || position.Y < 0 || position.Y >= height)
            return;

        IEntity entity = entities[position.X, position.Y];

        if (entity != null)
        {
            if (entity is Bloblin bloblin)
            {
                SelectBloblin(bloblin);
            }
            else if (entity is Item item && selectedBloblin != null)
            {
                CellPosition bloblinPosition = new CellPosition(
                    selectedBloblin.X,
                    selectedBloblin.Y
                );
                if (IsAdjacent(bloblinPosition, position))
                {
                    OnItemInteraction?.Invoke(item, selectedBloblin);
                    item.Interact(selectedBloblin);
                }
            }
        }
        else if (selectedBloblin != null)
        {
            MoveBloblin(selectedBloblin, position);
        }
    }

    private void SelectBloblin(Bloblin bloblin)
    {
        selectedBloblin = bloblin;
        OnBloblinSelected?.Invoke(bloblin);
    }

    public void PlaceEntity(IEntity entity, CellPosition position)
    {
        if (position.X < 0 || position.X >= width || position.Y < 0 || position.Y >= height)
            return;

        if (entities[position.X, position.Y] != null)
            return;

        entities[position.X, position.Y] = entity;

        if (entity is Bloblin bloblin)
        {
            bloblin.X = position.X;
            bloblin.Y = position.Y;
        }
    }

    public void RemoveEntity(CellPosition position)
    {
        if (position.X < 0 || position.X >= width || position.Y < 0 || position.Y >= height)
            return;

        entities[position.X, position.Y] = null;
    }

    public bool MoveBloblin(Bloblin bloblin, CellPosition targetPosition)
    {
        if (
            targetPosition.X < 0
            || targetPosition.X >= width
            || targetPosition.Y < 0
            || targetPosition.Y >= height
        )
            return false;

        if (entities[targetPosition.X, targetPosition.Y] != null)
            return false;

        CellPosition currentPosition = new CellPosition(bloblin.X, bloblin.Y);

        RemoveEntity(currentPosition);
        PlaceEntity(bloblin, targetPosition);

        Vector3 worldPosition = field.GetWorldPosition(targetPosition);
        field.MoveBloblinVisual(bloblin, worldPosition);

        return true;
    }

    private bool IsAdjacent(CellPosition pos1, CellPosition pos2)
    {
        int dx = Mathf.Abs(pos1.X - pos2.X);
        int dy = Mathf.Abs(pos1.Y - pos2.Y);

        return dx <= 1 && dy <= 1 && dx + dy > 0;
    }

    public IEntity GetEntityAt(CellPosition position)
    {
        if (position.X < 0 || position.X >= width || position.Y < 0 || position.Y >= height)
            return null;

        return entities[position.X, position.Y];
    }
}
