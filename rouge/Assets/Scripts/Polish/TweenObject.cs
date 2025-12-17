using UnityEngine;
using UnityEngine.Events;
using System;

public class TweenObject : MonoBehaviour
{
    public enum TweenType
    {
        Move,
        Scale,
        Rotate,
        Fade
    }

    public enum PlayMode
    {
        Manual,
        OnEnable,
        OnDisable
    }

    [Serializable]
    public class TweenData
    {
        [Header("Type")]
        public TweenType tweenType;

        [Header("Playback")]
        public PlayMode playMode = PlayMode.Manual;

        [Header("Values")]
        public Vector3 from;
        public Vector3 to;
        public bool useFrom = true;
        public bool relative;

        [Header("Timing")]
        public float duration = 0.3f;
        public float delay = 0f;

        [Header("Ease")]
        public LeanTweenType easeType = LeanTweenType.easeOutQuad;

        [Header("Loop")]
        public bool loop;
        public bool pingPong;

        [Header("Events")]
        public UnityEvent onComplete;
    }

    [Header("Target")]
    public GameObject target;

    [Header("Tweens")]
    public TweenData[] tweens;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (target == null)
            target = gameObject;

        spriteRenderer = target.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        PlayByMode(PlayMode.OnEnable, forward: true);
    }

    private void OnDisable()
    {
        PlayByMode(PlayMode.OnDisable, forward: false);
    }

    // =============================
    // PUBLIC API
    // =============================

    public void PlayForward()
    {
        PlayManualTweens(true);
    }

    public void PlayBackward()
    {
        PlayManualTweens(false);
    }

    public void PlayTweenForward(int index)
    {
        if (!IsValidIndex(index))
            return;

        LeanTween.cancel(target);
        CreateTween(tweens[index], true);
    }

    public void PlayTweenBackward(int index)
    {
        if (!IsValidIndex(index))
            return;

        LeanTween.cancel(target);
        CreateTween(tweens[index], false);
    }

    public void PlayTweens(params int[] indices)
    {
        LeanTween.cancel(target);

        foreach (int index in indices)
        {
            if (IsValidIndex(index))
                CreateTween(tweens[index], true);
        }
    }

    public void PlayTweens(int fromIndex, int toIndex, bool forward = true)
    {
        LeanTween.cancel(target);

        if (fromIndex > toIndex)
            (fromIndex, toIndex) = (toIndex, fromIndex);

        for (int i = fromIndex; i <= toIndex; i++)
        {
            if (IsValidIndex(i))
                CreateTween(tweens[i], forward);
        }
    }

    // =============================
    // INTERNAL LOGIC
    // =============================

    private void PlayManualTweens(bool forward)
    {
        LeanTween.cancel(target);

        foreach (TweenData tween in tweens)
        {
            if (tween.playMode == PlayMode.Manual)
                CreateTween(tween, forward);
        }
    }

    private void PlayByMode(PlayMode mode, bool forward)
    {
        LeanTween.cancel(target);

        foreach (TweenData tween in tweens)
        {
            if (tween.playMode == mode)
                CreateTween(tween, forward);
        }
    }

    private void CreateTween(TweenData data, bool forward)
    {
        Vector3 start = forward ? data.from : data.to;
        Vector3 end   = forward ? data.to   : data.from;

        if (data.useFrom)
            ApplyFromState(data, start);

        LTDescr tween = null;

        switch (data.tweenType)
        {
            case TweenType.Move:
                tween = LeanTween.move(
                    target,
                    data.relative ? target.transform.position + end : end,
                    data.duration
                );
                break;

            case TweenType.Scale:
                tween = LeanTween.scale(
                    target,
                    data.relative ? target.transform.localScale + end : end,
                    data.duration
                );
                break;

            case TweenType.Rotate:
                tween = LeanTween.rotate(
                    target,
                    data.relative ? target.transform.eulerAngles + end : end,
                    data.duration
                );
                break;

            case TweenType.Fade:
                if (spriteRenderer == null)
                {
                    Debug.LogWarning($"{name} has Fade tween but no SpriteRenderer.");
                    return;
                }

                tween = LeanTween.alpha(
                    target,
                    end.x,
                    data.duration
                );
                break;
        }

        if (tween == null) return;

        tween.setDelay(data.delay)
             .setEase(data.easeType);

        if (data.pingPong)
            tween.setLoopPingPong();
        else if (data.loop)
            tween.setLoopClamp();

        if (data.onComplete != null)
            tween.setOnComplete(data.onComplete.Invoke);
    }

    private void ApplyFromState(TweenData data, Vector3 value)
    {
        switch (data.tweenType)
        {
            case TweenType.Move:
                target.transform.position = value;
                break;

            case TweenType.Scale:
                target.transform.localScale = value;
                break;

            case TweenType.Rotate:
                target.transform.eulerAngles = value;
                break;

            case TweenType.Fade:
                if (spriteRenderer != null)
                {
                    Color c = spriteRenderer.color;
                    c.a = value.x;
                    spriteRenderer.color = c;
                }
                break;
        }
    }

    private bool IsValidIndex(int index)
    {
        if (tweens == null || index < 0 || index >= tweens.Length)
        {
            Debug.LogWarning($"{name} tried to play tween at invalid index {index}");
            return false;
        }
        return true;
    }
}
