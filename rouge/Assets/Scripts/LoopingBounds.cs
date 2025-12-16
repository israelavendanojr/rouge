using UnityEngine;

public class LoopingBounds : MonoBehaviour
{
    [Header("Horizontal Bounds")]
    public float leftX;
    public float rightX;

    [Header("Vertical Bounds")]
    public float bottomY;
    public float topY;

    private void LateUpdate()
    {
        Vector3 pos = transform.position;

        // Left → Right
        if (pos.x < leftX)
            pos.x = rightX;

        // Right → Left
        else if (pos.x > rightX)
            pos.x = leftX;

        // Down → Up
        if (pos.y < bottomY)
            pos.y = topY;

        // Up → Down
        else if (pos.y > topY)
            pos.y = bottomY;

        transform.position = pos;
    }
}
