using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMeshPool : PoolControl
{
    protected override void Awake()
    {
        poolAmouts = new PoolAmout[objectPoolData.SwordPrefab.Count];
        for (int i = 0; i < objectPoolData.SwordPrefab.Count; i++)
        {
            PoolAmout tmpPool = new PoolAmout();
            tmpPool.prefab = objectPoolData.SwordPrefab[i];
            tmpPool.parent = this.TF;
            poolAmouts[i] = tmpPool;
        }
        base.Awake();
    }
}
