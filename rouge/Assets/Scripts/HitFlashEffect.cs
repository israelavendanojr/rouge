using UnityEngine;
using System.Collections;

public class HitFlashEffect : MonoBehaviour
{
    [Header("References")]
    private Renderer objectRenderer;

    public Material flashMaterial;

    [Header("Settings")]
    public float flashDuration = 0.1f;
    public float invulnerabilityDuration = 0.5f;

    private Material originalMaterial;
    private Collider2D triggerCollider;

    void Awake()
    {
        triggerCollider = GetComponent<Collider2D>();

        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("HitFlashEffect: Renderer not found on this GameObject.", this);
            enabled = false; // Disable the script if it can't run
            return;
        }

        originalMaterial = objectRenderer.material;
        
        if (flashMaterial == null)
        {
            Debug.LogError("HitFlashEffect: Flash Material not assigned.", this);
            enabled = false;
        }
    }

    public void Flash()
    {
        StopAllCoroutines(); 
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        triggerCollider.enabled = false;
        if (flashMaterial != null)
        {
            objectRenderer.material = flashMaterial;
        }

        yield return new WaitForSeconds(flashDuration);

        if (originalMaterial != null)
        {
            objectRenderer.material = originalMaterial;
        }

        yield return new WaitForSeconds(invulnerabilityDuration - flashDuration);
        triggerCollider.enabled = true;
    }

    private void OnDisable()
    {
        if (objectRenderer != null && originalMaterial != null)
        {
            objectRenderer.material = originalMaterial;
        }
    }
}