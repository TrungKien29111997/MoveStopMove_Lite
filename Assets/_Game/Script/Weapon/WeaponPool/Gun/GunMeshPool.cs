using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMeshPool : PoolControl
{
    protected override void Awake()
    {
        poolAmouts = new PoolAmout[objectPoolData.GunPrefab.Count];
        for (int i = 0; i < objectPoolData.GunPrefab.Count; i++)
        {
            PoolAmout tmpPool = new PoolAmout();
            tmpPool.prefab = objectPoolData.GunPrefab[i];
            tmpPool.parent = this.TF;
            poolAmouts[i] = tmpPool;
        }
        base.Awake();
    }
}
