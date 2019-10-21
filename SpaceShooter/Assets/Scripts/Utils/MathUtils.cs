using UnityEngine;
public static class MathUtils
{
    public static Vector3 GetPositionAtCircle(Vector3 center, float radius, float degreeAngle)
    {
        Vector3 position;
        position.x = center.x + (radius * Mathf.Sin(degreeAngle * Mathf.Deg2Rad));
        position.y = center.y + (radius * Mathf.Cos(degreeAngle * Mathf.Deg2Rad));
        position.z = center.z;
        return position;
    }

    public static Vector3 GetRandomPositionAtCircle(Vector3 center, float radius)
    {
        float degreeAngle = Random.value * 360;
        Vector3 position;
        position.x = center.x + (radius * Mathf.Sin(degreeAngle * Mathf.Deg2Rad));
        position.y = center.y + (radius * Mathf.Cos(degreeAngle * Mathf.Deg2Rad));
        position.z = center.z;
        return position;
    }
    public static bool IsPointInsideCameraView(Vector3 position)
    {
        Vector3 viewPosition = Camera.main.WorldToViewportPoint(position);
        if (viewPosition.x > 1.0f || viewPosition.x < 0.0f || viewPosition.y > 1.0f || viewPosition.y < 0.0f)
        {
            return false;
        }
        return true;
    }
}
