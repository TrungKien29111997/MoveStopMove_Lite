using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuff : GameUnit
{
    [field: SerializeField] public string ItemName { get; private set; }
    [field: SerializeField] public float BuffSpeed { get; private set; }
    [field: SerializeField] public float BuffRange { get; private set; }
    [field: SerializeField] public int Price { get; protected set; }

    public void BuffCharacter(Character tmpChar)
    {
        tmpChar.IncreaseRange(BuffRange);
        tmpChar.IncreaseSpeed(BuffSpeed);
    }
}
