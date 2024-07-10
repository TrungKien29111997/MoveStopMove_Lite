using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pool", menuName = "Object Pool Data")]
public class ObjectPoolScObj : ScriptableObject
{
    [field: SerializeField] public List<GameUnit> CharacterPrefab { get; private set; }
    [field: SerializeField] public List<GameUnit> CharacterInfoPrefab { get; private set; }
    [field: SerializeField] public List<GameUnit> SwordPrefab { get; private set; }
    [field: SerializeField] public List<GameUnit> HammerPrefab { get; private set; }
    [field: SerializeField] public List<GameUnit> GunPrefab { get; private set; }
    [field: SerializeField] public List<GameUnit> SkillBuffPrefab { get; private set; }
    [field: SerializeField] public List<GameUnit> VFXPrePrefab { get; private set; }
    [field: SerializeField] public List<GameUnit> IconPrefab { get; private set; }
    [field: SerializeField] public List<GameUnit> HatPrefab { get; private set; }
    [field: SerializeField] public List<GameUnit> SheildPrefab { get; private set; }
    [field: SerializeField] public List<GameUnit> SkillPrefab { get; private set; }
}
