using System;
using UnityEngine;

[RequireComponent(typeof(Highlighter))]
public class Cell : MonoBehaviour
{
    private CellPosition position;
    private Action<CellPosition> onClick;

    internal void Initialize(int x, int y, Action<CellPosition> onClick)
    {
        this.position = new CellPosition(x, y);
        this.onClick = onClick;
    }

    public void SetHighlight(bool needHighlight)
    {
        GetComponent<Highlighter>().SetHighlight(needHighlight);
    }

    private void OnMouseDown()
    {
        onClick?.Invoke(position);
    }
}
