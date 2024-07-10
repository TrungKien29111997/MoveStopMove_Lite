using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewRange : MonoBehaviour
{
    [SerializeField] Character parentChar;
    [field: SerializeField] public Renderer Render { get; private set; }
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

    public void SetCharParent(Character tmpChar)
    {
        parentChar = tmpChar;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parentChar != null)
        {
            if (!parentChar.CharInRange.Contains(Cache.GetCharacter(other)))
            {
                parentChar.AddCharInRange(Cache.GetCharacter(other));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (parentChar != null)
        {
            if (parentChar.CharInRange.Contains(Cache.GetCharacter(other)))
            {
                parentChar.RemoveCharInRange(Cache.GetCharacter(other));
            }
        }
    }
}
