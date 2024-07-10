using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffRange : Buff
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Cache.GetCharacter(other).IncreaseRange(increaseNum);
    }
}
