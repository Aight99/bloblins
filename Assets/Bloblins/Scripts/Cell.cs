using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }

    private IEntity occupant;

    public bool IsOccupied => occupant != null;

    public static event Action<Cell> OnCellClicked;

    public void Initialize(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool SetOccupant(IEntity entity)
    {
        if (IsOccupied)
            return false;

        occupant = entity;
        return true;
    }

    public IEntity GetOccupant()
    {
        return occupant;
    }

    public void ClearOccupant()
    {
        occupant = null;
    }

    private void OnMouseDown()
    {
        OnCellClicked?.Invoke(this);
    }
}
