using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour
{
    [field: SerializeField] public EItemType ItemType { get; protected set; }
    [field: SerializeField] public EPooling PoolType { get; private set; }
    [field: SerializeField] public Sprite IconImage { get; protected set; }
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

    public virtual void SetMaterial(Material mat)
    {

    }
}

[System.Serializable]
public class PoolAmout
{
    public GameUnit prefab;
    public Transform parent;
    public int amount;
}
