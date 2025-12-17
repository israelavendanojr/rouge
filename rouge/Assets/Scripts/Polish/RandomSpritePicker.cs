using UnityEngine;

public class RandomSpritePicker : MonoBehaviour
{
    [Header("Sprites to choose from")]
    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning($"{name} has no SpriteRenderer!");
            return;
        }

        SetRandomSprite();
    }

    /// <summary>
    /// Picks a random sprite from the array and applies it.
    /// </summary>
    public void SetRandomSprite()
    {
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogWarning($"{name} has no sprites assigned!");
            return;
        }

        int index = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[index];
    }
}
