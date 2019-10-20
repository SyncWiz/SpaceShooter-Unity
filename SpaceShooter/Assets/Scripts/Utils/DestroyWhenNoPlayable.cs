using UnityEngine;

public class DestroyWhenNoPlayable : MonoBehaviour
{
    //private
    Camera m_MainCamera;

    void Start()
    {
        m_MainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 viewPosition = m_MainCamera.WorldToViewportPoint(transform.position);
        if (viewPosition.x <= 0)
        {
            Destroy(gameObject);
        }
    }
}
