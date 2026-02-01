using UnityEngine;

public class AnimatableVisual : MonoBehaviour
{
    [Header("Move Animation")]
    [SerializeField]
    private MoveAnimationSettings moveSettings = new MoveAnimationSettings();

    public MoveAnimationSettings MoveSettings => moveSettings;
}
