using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerMeshPool : PoolControl
{
    protected override void Awake()
    {
        poolAmouts = new PoolAmout[objectPoolData.HammerPrefab.Count];
        for (int i = 0; i < objectPoolData.HammerPrefab.Count; i++)
        {
            PoolAmout tmpPool = new PoolAmout();
            tmpPool.prefab = objectPoolData.HammerPrefab[i];
            tmpPool.parent = this.TF;
            poolAmouts[i] = tmpPool;
        }
        base.Awake();
    }
}
