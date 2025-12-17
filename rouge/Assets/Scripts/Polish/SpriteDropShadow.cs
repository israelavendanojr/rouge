using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteDropShadow : MonoBehaviour
{
    [Header("Shadow Appearance")]
    [Range(0f, 1f)]
    [SerializeField] private float alpha = 0.35f;
    [SerializeField] private Color shadowColor = Color.black;
    [SerializeField] private Vector2 offset = new Vector2(-.3f, -.3f);
    [SerializeField] private Vector2 scaleMultiplier = new Vector2(1f, 0.7f);

    [Header("Sorting")]
    [SerializeField] private int sortingOrderOffset = -1;
    [SerializeField] private string sortingLayerOverride = "";

    [Header("Advanced")]
    [SerializeField] private bool followRotation = false;
    [SerializeField] private bool followScale = true;
    [SerializeField] private bool autoShrinkWithHeight = false;
    [SerializeField] private float heightShrinkMultiplier = 1f;

    private SpriteRenderer source;
    private SpriteRenderer shadow;
    private Transform shadowTransform;

    private float baseY;

    private void Awake()
    {
        source = GetComponent<SpriteRenderer>();
        baseY = transform.position.y;

        CreateShadow();
    }

    private void LateUpdate()
    {
        if (shadow == null)
            return;

        SyncShadow();
    }

    private void CreateShadow()
    {
        GameObject shadowObj = new GameObject($"{gameObject.name}_Shadow");
        shadowObj.transform.SetParent(transform);
        shadowObj.transform.localPosition = Vector3.zero;

        shadowTransform = shadowObj.transform;
        shadow = shadowObj.AddComponent<SpriteRenderer>();

        shadow.sprite = source.sprite;
        shadow.flipX = source.flipX;
        shadow.flipY = source.flipY;

        shadow.color = new Color(
            shadowColor.r,
            shadowColor.g,
            shadowColor.b,
            alpha
        );

        shadow.sortingLayerID = string.IsNullOrEmpty(sortingLayerOverride)
            ? source.sortingLayerID
            : SortingLayer.NameToID(sortingLayerOverride);

        shadow.sortingOrder = source.sortingOrder + sortingOrderOffset;
    }

    private void SyncShadow()
{
    // Sprite & flip
    shadow.sprite = source.sprite;
    shadow.flipX = source.flipX;
    shadow.flipY = source.flipY;

    // ✅ LOCAL position (fixes jitter)
    shadowTransform.localPosition = offset;

    // Rotation
    shadowTransform.localRotation = followRotation
        ? Quaternion.identity
        : Quaternion.identity; // shadow should usually NOT rotate

    // Scale
    Vector3 scale = followScale
        ? transform.localScale
        : Vector3.one;

    scale.x *= scaleMultiplier.x;
    scale.y *= scaleMultiplier.y;

    // Height-based squash
    if (autoShrinkWithHeight)
    {
        float height = Mathf.Abs(transform.position.y - baseY);
        float shrink = Mathf.Clamp01(height * heightShrinkMultiplier);
        scale *= Mathf.Lerp(1f, 0.6f, shrink);
    }

    shadowTransform.localScale = scale;
}

    public void SetAlpha(float value)
    {
        alpha = Mathf.Clamp01(value);
        Color c = shadow.color;
        c.a = alpha;
        shadow.color = c;
    }

    public void SetOffset(Vector2 newOffset)
    {
        offset = newOffset;
    }

    public void SetColor(Color color)
    {
        shadowColor = color;
        Color c = shadow.color;
        c.r = color.r;
        c.g = color.g;
        c.b = color.b;
        shadow.color = c;
    }
}
