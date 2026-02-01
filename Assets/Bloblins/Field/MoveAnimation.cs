using System.Collections;
using UnityEngine;

public class MoveAnimation : IAnimation
{
    private readonly Transform transform;
    private readonly Vector3 targetPosition;
    private readonly MoveAnimationSettings settings;

    public MoveAnimation(Transform transform, Vector3 targetPosition, MoveAnimationSettings settings)
    {
        this.transform = transform;
        this.targetPosition = targetPosition;
        this.settings = settings;
    }

    public IEnumerator Play()
    {
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float duration = distance / settings.Speed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float curveValue = settings.Curve.Evaluate(t);
            transform.position = Vector3.Lerp(startPosition, targetPosition, curveValue);
            yield return null;
        }

        transform.position = targetPosition;
    }
}
