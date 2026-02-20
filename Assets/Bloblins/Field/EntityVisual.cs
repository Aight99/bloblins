using System;
using UnityEngine;

public class EntityVisual : MonoBehaviour
{
    private IEnvironmentObject environmentObject;
    private Action<CellPosition> onClick;

    public IEnvironmentObject EnvironmentObject => environmentObject;

    public void Initialize(IEnvironmentObject environmentObject, Action<CellPosition> onClick)
    {
        this.environmentObject = environmentObject;
        this.onClick = onClick;

        if (environmentObject is ICreature creature)
        {
            var materialLoader = GetComponent<CreatureMaterialLoader>();
            if (materialLoader != null)
            {
                materialLoader.LoadMaterialForCreature(creature);
            }
        }
    }

    private void OnMouseDown()
    {
        onClick?.Invoke(environmentObject.Position);
    }
}
