using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character Data")]
public class CharacterScObj : ScriptableObject
{
    [field: SerializeField] public HealthBar HealthBar { get; private set; }
    [field: SerializeField] public ViewRange RangeAttack { get; private set; }
    [field: SerializeField] public TargetAim Aim { get; private set; }
    [field: SerializeField] public AngleRing Ring { get; private set; }
    [field: SerializeField] public Vector3 OffsetRing { get; private set; }
    [field: SerializeField] public Quaternion RotateRing { get; private set; }
    // chracter color
    [field: SerializeField] public Material[] CharacterMat { get; private set; }
    [field: SerializeField] public float RankUpScaleModel { get; private set; }
    [field: SerializeField] public float RankUpScaleViewRange { get; private set; }

}
