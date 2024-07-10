using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float delay;
    void Start()
    {
        Destroy(gameObject, delay);
    }
}
