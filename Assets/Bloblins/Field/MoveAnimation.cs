using System.Collections;
using UnityEngine;

public class MoveAnimation : IAnimation
{
    private readonly Transform transform;
    private readonly Vector3 targetPosition;
    private readonly float speed;

    public MoveAnimation(Transform transform, Vector3 targetPosition, float speed = 5f)
    {
        this.transform = transform;
        this.targetPosition = targetPosition;
        this.speed = speed;
    }

    public IEnumerator Play()
    {
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float duration = distance / speed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
    }
}
