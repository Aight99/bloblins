using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private CellPosition position;
    private Action<CellPosition> onClick;

    internal void Initialize(int x, int y, Action<CellPosition> onClick)
    {
        this.position = new CellPosition(x, y);
        this.onClick = onClick;
    }

    private void OnMouseDown()
    {
        onClick?.Invoke(position);
    }
}
