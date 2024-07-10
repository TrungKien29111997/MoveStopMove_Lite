using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [field: SerializeField] public Transform CamTrans { get; private set; }
    [SerializeField] Vector3 defaultPosCam;
    [SerializeField] Quaternion defaultPosRot;
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

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        CamTrans.SetLocalPositionAndRotation(defaultPosCam, defaultPosRot);
    }

    void Update()
    {
        if (LevelManager.Instance.Player != null)
        {
            TF.position = LevelManager.Instance.Player.TF.position;
        }
    }
}
