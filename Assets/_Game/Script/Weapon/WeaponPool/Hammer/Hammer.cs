using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Bullet
{
    [SerializeField] Transform modelTrans;
    [SerializeField] float rotateSpeed;

    protected override void Update()
    {
        base.Update();
        modelTrans.localRotation *= Quaternion.Euler(0, rotateSpeed * LevelManager.Instance.FPS * Time.deltaTime, 0);
    }
}
