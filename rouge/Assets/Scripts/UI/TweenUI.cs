using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class TweenUI : MonoBehaviour
{
    public enum TweenType
    {
        Move,
        Scale,
        Fade,
        Rotate
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
        public TweenType tweenType;

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

    [Header("Playback")]
    public PlayMode playMode = PlayMode.Manual;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (target == null)
            target = gameObject;

        rectTransform = target.GetComponent<RectTransform>();

        if (NeedsCanvasGroup())
            canvasGroup = target.GetComponent<CanvasGroup>() ?? target.AddComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        if (playMode == PlayMode.OnEnable)
            PlayForward();
    }

    private void OnDisable()
    {
        if (playMode == PlayMode.OnDisable)
            PlayBackward();
    }

    // =============================
    // PUBLIC API
    // =============================

    public void PlayForward()
    {
        PlayTweens(forward: true);
    }

    public void PlayBackward()
    {
        PlayTweens(forward: false);
    }

    public void Toggle()
    {
        PlayForward();
    }

    // =============================
    // CORE LOGIC
    // =============================

    private void PlayTweens(bool forward)
    {
        LeanTween.cancel(target);

        foreach (TweenData tween in tweens)
        {
            CreateTween(tween, forward);
        }
    }

    private void CreateTween(TweenData data, bool forward)
    {
        Vector3 start = forward ? data.from : data.to;
        Vector3 end = forward ? data.to : data.from;

        if (data.useFrom)
            ApplyFromState(data, start);

        LTDescr tween = null;

        switch (data.tweenType)
        {
            case TweenType.Move:
                tween = LeanTween.move(
                    rectTransform,
                    data.relative ? rectTransform.anchoredPosition + (Vector2)end : end,
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
                tween = LeanTween.alphaCanvas(
                    canvasGroup,
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
                rectTransform.anchoredPosition = value;
                break;

            case TweenType.Scale:
                target.transform.localScale = value;
                break;

            case TweenType.Rotate:
                target.transform.eulerAngles = value;
                break;

            case TweenType.Fade:
                canvasGroup.alpha = value.x;
                break;
        }
    }

    private bool NeedsCanvasGroup()
    {
        foreach (TweenData tween in tweens)
        {
            if (tween.tweenType == TweenType.Fade)
                return true;
        }
        return false;
    }
}
