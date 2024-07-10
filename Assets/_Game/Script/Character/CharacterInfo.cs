using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : ItemBuff
{
    Animator anim;
    public Animator Anim
    {
        get
        {
            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }
            return anim;
        }
    }

    [field: SerializeField] public Transform RightHandPos { get; private set; }
    [field: SerializeField] public Transform LefttHandPos { get; private set; }
    [field: SerializeField] public Transform HeadBone { get; private set; }
}
