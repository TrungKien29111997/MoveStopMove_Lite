using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolControl : MonoBehaviour
{
    Transform tf;
    public Transform TF
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }
    [SerializeField] protected ObjectPoolScObj objectPoolData;
    [SerializeField] protected PoolAmout[] poolAmouts;

    protected virtual void Awake()
    {
        OnInit();
    }

    public void OnInit()
    {
        for (int i = 0; i < poolAmouts.Length; i++)
        {
            SimplePool.PreLoad(poolAmouts[i].prefab, poolAmouts[0].amount, poolAmouts[0].parent);
        }
    }
}
