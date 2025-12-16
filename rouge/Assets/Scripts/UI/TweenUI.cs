using UnityEngine;
using UnityEngine.UI;

public class TweenUI : MonoBehaviour
{
    public enum AnimationType
    {
        Fade,
        Move,
        Scale
    }

    [Header("References")]
    public GameObject objectToAnimate;

    [Header("Animation Settings")]
    public AnimationType animationType;
    public float duration = 0.25f;
    public float delay = 0f;
    public bool loop;
    public bool pingPong;

    [Header("Position / Scale")]
    public bool startPositionOffset;
    public Vector3 from;
    public Vector3 to;

    [Header("Auto Play")]
    public bool showOnEnable;
    public bool workOnDisable;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private int tweenId = -1;

    private void Awake()
    {
        if (objectToAnimate == null)
            objectToAnimate = gameObject;

        rectTransform = objectToAnimate.GetComponent<RectTransform>();
        canvasGroup = objectToAnimate.GetComponent<CanvasGroup>();

        if (animationType == AnimationType.Fade && canvasGroup == null)
            canvasGroup = objectToAnimate.AddComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        if (showOnEnable)
            PlayForward();
    }

    private void OnDisable()
    {
        if (workOnDisable)
            PlayBackward();
    }

    public void PlayForward()
    {
        LeanTween.cancel(objectToAnimate);
        ApplyFromState();
        CreateTween(from, to);
    }

    public void PlayBackward()
    {
        LeanTween.cancel(objectToAnimate);
        CreateTween(to, from);
    }

    private void ApplyFromState()
    {
        if (!startPositionOffset)
            return;

        switch (animationType)
        {
            case AnimationType.Move:
                rectTransform.anchoredPosition = from;
                break;

            case AnimationType.Scale:
                rectTransform.localScale = from;
                break;

            case AnimationType.Fade:
                canvasGroup.alpha = from.x;
                break;
        }
    }

    private void CreateTween(Vector3 start, Vector3 end)
    {
        LTDescr tween = null;

        switch (animationType)
        {
            case AnimationType.Move:
                tween = LeanTween.move(rectTransform, end, duration);
                break;

            case AnimationType.Scale:
                tween = LeanTween.scale(objectToAnimate, end, duration);
                break;

            case AnimationType.Fade:
                tween = LeanTween.alphaCanvas(canvasGroup, end.x, duration);
                break;
        }

        if (tween == null)
            return;

        tween.setDelay(delay);

        if (pingPong)
            tween.setLoopPingPong();
        else if (loop)
            tween.setLoopClamp();
    }
}
