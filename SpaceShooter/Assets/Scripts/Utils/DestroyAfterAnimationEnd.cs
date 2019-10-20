using UnityEngine;

public class DestroyAfterAnimationEnd : MonoBehaviour
{
    public float m_Delay;

    void Start()
    {
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + m_Delay);
    }
}
