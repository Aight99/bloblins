using UnityEngine;

[System.Serializable]
public class MoveAnimationSettings
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [SerializeField]
    private float jumpHeight = 0f;

    [SerializeField]
    private AnimationCurve jumpCurve = AnimationCurve.Linear(0f, 0f, 1f, 0f);

    [SerializeField]
    private float fixedDuration = 0f;

    public float Speed => speed;
    public AnimationCurve Curve => curve;
    public float JumpHeight => jumpHeight;
    public AnimationCurve JumpCurve => jumpCurve;
    public float FixedDuration => fixedDuration;

    public MoveAnimationSettings() { }

    public MoveAnimationSettings(
        float speed,
        AnimationCurve curve = null,
        float jumpHeight = 0f,
        AnimationCurve jumpCurve = null,
        float fixedDuration = 0f
    )
    {
        this.speed = speed;
        this.curve = curve ?? AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        this.jumpHeight = jumpHeight;
        this.jumpCurve = jumpCurve ?? AnimationCurve.Linear(0f, 0f, 1f, 0f);
        this.fixedDuration = fixedDuration;
    }
}
