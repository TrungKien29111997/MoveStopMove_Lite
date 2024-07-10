using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheildPool : PoolControl
{
    protected override void Awake()
    {
        poolAmouts = new PoolAmout[objectPoolData.SheildPrefab.Count];
        for (int i = 0; i < objectPoolData.SheildPrefab.Count; i++)
        {
            PoolAmout tmpPool = new PoolAmout();
            tmpPool.prefab = objectPoolData.SheildPrefab[i];
            tmpPool.parent = this.TF;
            poolAmouts[i] = tmpPool;
        }
        base.Awake();
    }
}
