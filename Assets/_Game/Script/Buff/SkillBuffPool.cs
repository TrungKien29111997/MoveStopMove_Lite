using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBuffPool : PoolControl
{
    protected override void Awake()
    {
        poolAmouts = new PoolAmout[objectPoolData.SkillBuffPrefab.Count];
        for (int i = 0; i < objectPoolData.SkillBuffPrefab.Count; i++)
        {
            PoolAmout tmpPool = new PoolAmout();
            tmpPool.prefab = objectPoolData.SkillBuffPrefab[i];
            tmpPool.parent = this.TF;
            poolAmouts[i] = tmpPool;
        }
        base.Awake();
    }
}
