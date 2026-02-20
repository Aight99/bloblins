using UnityEngine;

public class CreatureMaterialLoader : MonoBehaviour
{
    public void LoadMaterialForCreature(ICreature creature)
    {
        var renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogWarning($"No Renderer found on {gameObject.name}");
            return;
        }

        string materialName = GetMaterialNameForCreature(creature);
        Material material = Resources.Load<Material>(materialName);
        
        if (material == null)
        {
            Debug.LogWarning($"Material not found: {materialName}");
            return;
        }

        renderer.material = material;
    }

    private string GetMaterialNameForCreature(ICreature creature)
    {
        if (creature is Baldush)
        {
            return "Bloblin/Baldush";
        }
        
        if (creature is GraySolder)
        {
            return "Enemies/GraySolder";
        }

        Debug.LogWarning($"Unknown creature type: {creature.GetType().Name}");
        return "";
    }
}
