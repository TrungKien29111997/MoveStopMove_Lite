using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPool : PoolControl
{
    protected override void Awake()
    {
        poolAmouts = new PoolAmout[objectPoolData.VFXPrePrefab.Count];
        for (int i = 0; i < objectPoolData.VFXPrePrefab.Count; i++)
        {
            PoolAmout tmpPool = new PoolAmout();
            tmpPool.prefab = objectPoolData.VFXPrePrefab[i];
            tmpPool.parent = this.TF;
            poolAmouts[i] = tmpPool;
        }
        base.Awake();
    }
}
