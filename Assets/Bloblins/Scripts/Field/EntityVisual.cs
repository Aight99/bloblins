using System;
using UnityEngine;

public class EntityVisual : MonoBehaviour
{
    private IEnvironmentObject environmentObject;
    private Action<CellPosition> onClick;

    public void Initialize(IEnvironmentObject environmentObject, Action<CellPosition> onClick)
    {
        this.environmentObject = environmentObject;
        this.onClick = onClick;
    }

    private void OnMouseDown()
    {
        onClick?.Invoke(environmentObject.Position);
    }
}
