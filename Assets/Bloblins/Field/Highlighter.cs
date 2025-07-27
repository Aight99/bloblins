using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Highlighter : MonoBehaviour
{
    private Material material;
    private bool isHighlighted;

    private readonly int highlightEnabledId = Shader.PropertyToID("_isHighlighted");

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    public void SetHighlight(bool needHighlight)
    {
        if (needHighlight)
        {
            EnableHighlight();
        }
        else
        {
            DisableHighlight();
        }
    }

    public void EnableHighlight()
    {
        material.SetFloat(highlightEnabledId, 1f);
        isHighlighted = true;
    }

    public void DisableHighlight()
    {
        material.SetFloat(highlightEnabledId, 0f);
        isHighlighted = false;
    }

    public bool IsHighlighted => isHighlighted;
}
