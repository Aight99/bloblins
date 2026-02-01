using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationQueue : MonoBehaviour
{
    [Header("Default Move Animation")]
    [SerializeField]
    private MoveAnimationSettings defaultMoveSettings = new MoveAnimationSettings();

    private readonly Queue<IAnimation> animations = new Queue<IAnimation>();
    private bool isPlaying = false;
    private Action onQueueComplete;

    public bool IsPlaying => isPlaying;
    public MoveAnimationSettings DefaultMoveSettings => defaultMoveSettings;

    public void EnqueueAnimation(IAnimation animation)
    {
        animations.Enqueue(animation);

        if (!isPlaying)
        {
            StartCoroutine(PlayQueue());
        }
    }

    public void SetOnCompleteCallback(Action callback)
    {
        onQueueComplete = callback;
    }

    public void ClearQueue()
    {
        animations.Clear();
        StopAllCoroutines();
        isPlaying = false;
    }

    private IEnumerator PlayQueue()
    {
        isPlaying = true;

        while (animations.Count > 0)
        {
            var animation = animations.Dequeue();
            yield return animation.Play();
        }

        isPlaying = false;
        onQueueComplete?.Invoke();
    }
}
