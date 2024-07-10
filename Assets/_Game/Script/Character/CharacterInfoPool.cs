using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoPool : PoolControl
{
    protected override void Awake()
    {
        poolAmouts = new PoolAmout[objectPoolData.CharacterInfoPrefab.Count];
        for (int i = 0; i < objectPoolData.CharacterInfoPrefab.Count; i++)
        {
            PoolAmout tmpPool = new PoolAmout();
            tmpPool.prefab = objectPoolData.CharacterInfoPrefab[i];
            tmpPool.parent = this.TF;
            poolAmouts[i] = tmpPool;
        }
        base.Awake();
    }
}
