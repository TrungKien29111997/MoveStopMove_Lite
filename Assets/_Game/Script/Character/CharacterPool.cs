using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPool : PoolControl
{
    protected override void Awake()
    {
        poolAmouts = new PoolAmout[objectPoolData.CharacterPrefab.Count];
        for (int i = 0; i < objectPoolData.CharacterPrefab.Count; i++)
        {
            PoolAmout tmpPool = new PoolAmout();
            tmpPool.prefab = objectPoolData.CharacterPrefab[i];
            tmpPool.parent = this.TF;
            poolAmouts[i] = tmpPool;
        }
        base.Awake();
    }
}
