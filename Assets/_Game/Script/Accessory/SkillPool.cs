using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPool : PoolControl
{
    protected override void Awake()
    {
        poolAmouts = new PoolAmout[objectPoolData.SkillPrefab.Count];
        for (int i = 0; i < objectPoolData.SkillPrefab.Count; i++)
        {
            PoolAmout tmpPool = new PoolAmout();
            tmpPool.prefab = objectPoolData.SkillPrefab[i];
            tmpPool.parent = this.TF;
            poolAmouts[i] = tmpPool;
        }
        base.Awake();
    }
}
