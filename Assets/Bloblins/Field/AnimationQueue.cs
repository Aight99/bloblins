using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationQueue : MonoBehaviour
{
    private readonly Queue<IAnimation> animations = new Queue<IAnimation>();
    private bool isPlaying = false;
    private Action onQueueComplete;

    public bool IsPlaying => isPlaying;

    public void EnqueueAnimation(IAnimation animation)
    {
        DebugHelper.Log(DebugHelper.MessageType.Animation, $"Добавляем анимацию {animation.GetType().Name}");
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
        DebugHelper.Log(DebugHelper.MessageType.Animation, $"Начинаем воспроизведение очереди");
        isPlaying = true;

        while (animations.Count > 0)
        {
            var animation = animations.Dequeue();
            yield return animation.Play();
        }

        DebugHelper.Log(DebugHelper.MessageType.Animation, $"Завершаем воспроизведение очереди");
        isPlaying = false;
        onQueueComplete?.Invoke();
    }
}
