using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cache
{
    static Dictionary<Collider, Character> dicCharacter = new Dictionary<Collider, Character>();

    public static Character GetCharacter(Collider collider)
    {
        if (!dicCharacter.ContainsKey(collider))
        {
            Character character = collider.GetComponent<Character>();
            dicCharacter.Add(collider,character);
        }
        return dicCharacter[collider];
    }
}
