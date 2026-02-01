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
        float duration = settings.FixedDuration > 0f 
            ? settings.FixedDuration 
            : distance / settings.Speed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            float curveValue = settings.Curve.Evaluate(t);
            Vector3 position = Vector3.Lerp(startPosition, targetPosition, curveValue);
            
            if (settings.JumpHeight > 0f)
            {
                float jumpOffset = settings.JumpCurve.Evaluate(t) * settings.JumpHeight;
                position.y += jumpOffset;
            }
            
            transform.position = position;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
