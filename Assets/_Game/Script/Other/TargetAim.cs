using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAim : MonoBehaviour
{
    [field: SerializeField] public Renderer Render { get; private set; }
    Transform tf;
    public Transform TF
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }
}
