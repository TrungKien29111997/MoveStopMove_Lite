using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SimplePool
{
    public static Dictionary<EPooling, Pool> poolInstance = new Dictionary<EPooling, Pool>();

    // Khoi tao Pool moi
    public static void PreLoad(GameUnit prefab, int amount, Transform parent)
    {
        if (prefab == null)
        {
            Debug.LogError("PREFABS IS EMPTY");
            return;
        }

        if (!poolInstance.ContainsKey(prefab.PoolType) || poolInstance[prefab.PoolType] == null)
        {
            Pool p = new Pool();
            p.PreLoad(prefab, amount, parent);
            poolInstance[prefab.PoolType] = p;
        }
    }

    // Lay phan tu ra
    public static T Spawn<T>(EPooling poolType, Vector3 pos, Quaternion rot) where T : GameUnit
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + "IS NOT PRELOAD");
            return null;
        }
        return poolInstance[poolType].Spawn(pos, rot) as T;
    }

    // tra phan tu vao
    public static void Despawn(GameUnit unit)
    {
        if (!poolInstance.ContainsKey(unit.PoolType))
        {
            Debug.LogError(unit.PoolType + "IS NOT PRELOAD");
        }
        poolInstance[unit.PoolType].Despawn(unit);
    }

    //thu thap phan tu
    public static void Collect(EPooling poolType)
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + "IS NOT PRELOAD");
        }
        poolInstance[poolType].Collect();
    }

    //thu thap tat ca
    public static void CollectAll()
    {
        foreach(var item in poolInstance.Values)
        {
            item.Collect();
        }
    }

    // Destroy 1 pool
    public static void Release(EPooling poolType)
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + "IS NOT PRELOAD");
        }
        poolInstance[poolType].Release();
    }

    // Destroy tat ca
    public static void ReleaseAll()
    {
        foreach (var item in poolInstance.Values)
        {
            item.Release(); 
        }
    }
}


public class Pool : MonoBehaviour
{
    Transform parent;
    GameUnit prefab;
    // List chua cac Unit o trong Pool
    Queue<GameUnit> inActives = new Queue<GameUnit>();

    //List chua cac Unit dang su dung
    List<GameUnit> actives = new List<GameUnit>();
    public List<GameUnit> Actives => actives;

    // khoi tao Poll
    public void PreLoad(GameUnit prefab, int amount, Transform parent)
    {
        this.parent = parent;
        this.prefab = prefab;
        
        for (int i = 0; i < amount; i++)
        {
            Despawn(Instantiate(prefab, parent));
        }
    }

    // Active phan tu tu Pool thay vi Instantiate
    public GameUnit Spawn(Vector3 pos, Quaternion rot)
    {
        GameUnit unit;
        if (inActives.Count <= 0)
        {
            unit = Instantiate(prefab, parent);
        }
        else
        {
            unit = inActives.Dequeue();
        }
        unit.TF.SetPositionAndRotation(pos, rot);
        actives.Add(unit);
        unit.gameObject.SetActive(true);
        return unit;
    }

    // Nap phan tu vao Pool thay vi Destroy
    public void Despawn(GameUnit unit)
    {
        if (unit != null && unit.gameObject.activeSelf)
        {
            actives.Remove(unit);
            inActives.Enqueue(unit);
            unit.gameObject.SetActive(false);
        }
    }

    // thu thap phan tu Instantiate Clone ve Pool
    public void Collect()
    {
        while(actives.Count > 0)
        {
            Despawn(actives[0]);
        }
    }

    // Destroy tat ca phan tu Clone dang ton tai
    public void Release()
    {
        Collect();
        while(inActives.Count > 0)
        {
            Destroy(inActives.Dequeue().gameObject);
        }
        inActives.Clear();
    }
}
