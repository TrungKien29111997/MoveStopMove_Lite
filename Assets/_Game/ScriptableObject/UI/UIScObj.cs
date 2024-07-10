using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIScObj", menuName = "UI Data")]
public class UIScObj : ScriptableObject
{
    [field: SerializeField] public Color EnoughMoney { get; private set; }
    [field: SerializeField] public Color NotEnoughMoney { get; private set; }
}
