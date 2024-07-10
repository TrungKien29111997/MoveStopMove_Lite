using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpeed : Buff
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Cache.GetCharacter(other).IncreaseSpeed(increaseNum);
    }
}
