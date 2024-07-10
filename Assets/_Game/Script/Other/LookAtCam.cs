using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    Transform camTrans;
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

    protected virtual void Start()
    {
        camTrans = LevelManager.Instance.MainCamera.transform;
    }

    protected virtual void Update()
    {
        TF.forward = camTrans.forward;
    }
}
