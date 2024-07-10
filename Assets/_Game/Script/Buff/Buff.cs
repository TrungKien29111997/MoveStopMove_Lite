using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : GameUnit
{
    [SerializeField] protected float increaseNum;
    protected virtual void OnTriggerEnter(Collider other)
    {
        LevelManager.Instance.PlayerHadBuff();
        SimplePool.Despawn(this);
    }
}
