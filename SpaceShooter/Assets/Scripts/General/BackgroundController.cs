using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    //Public
    public GameObject[] m_BackgroundObjects;

    //Private
    private Camera m_MainCamera;
    private Vector2 m_ScreenBounds;
    
    void Start()
    {
        m_MainCamera = Camera.main;
        m_ScreenBounds = m_MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, m_MainCamera.transform.position.z));
        foreach (GameObject obj in m_BackgroundObjects)
        {
            LoadBackgroundObject(obj);
        }
    }

    void LoadBackgroundObject(GameObject obj)
    {
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x;
        int copiesNeeded = (int)Mathf.Ceil(m_ScreenBounds.x * 2 / objectWidth);
        GameObject clone = Instantiate(obj);
        for (int i = 0; i <= copiesNeeded; i++)
        {
            GameObject copy = Instantiate(clone);
            copy.transform.SetParent(obj.transform);
            copy.transform.position = new Vector3(objectWidth * i, obj.transform.position.y, obj.transform.position.z);
            copy.name = obj.name + i;
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    void RepositionBackground(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if (children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x;
            if (transform.position.x + m_ScreenBounds.x > lastChild.transform.position.x + halfObjectWidth)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjectWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);
            }
            else if (transform.position.x - m_ScreenBounds.x < firstChild.transform.position.x - halfObjectWidth)
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfObjectWidth * 2, firstChild.transform.position.y, firstChild.transform.position.z);
            }
        }
    }

    void LateUpdate()
    {
        foreach (GameObject obj in m_BackgroundObjects)
        {
            RepositionBackground(obj);
        }
    }
}