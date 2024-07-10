using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension 
{
    public static T Last<T>(this List<T> list) where T: MonoBehaviour
    {
        return list[^1];
    } 
}
