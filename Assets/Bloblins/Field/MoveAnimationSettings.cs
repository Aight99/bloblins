using UnityEngine;

[System.Serializable]
public class MoveAnimationSettings
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    public float Speed => speed;
    public AnimationCurve Curve => curve;

    public MoveAnimationSettings() { }

    public MoveAnimationSettings(float speed, AnimationCurve curve = null)
    {
        this.speed = speed;
        this.curve = curve ?? AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }
}
