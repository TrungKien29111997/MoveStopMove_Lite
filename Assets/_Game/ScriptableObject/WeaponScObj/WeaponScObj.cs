using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon Data")]
public class WeaponScObj : ScriptableObject
{
    [field: SerializeField] public AnimWeaponData[] WeaponAnim { get; private set; }
    public AnimWeaponData GetAnim(EItemType weaponType)
    {
        return WeaponAnim[(int)weaponType];
    }

    public float GetAnimSpeed(EItemType weaponType)
    {
        return WeaponAnim[(int)weaponType].MultiSpeed;
    }

    public float GetDelaySpawnBullet(EItemType weaponType)
    {
        return WeaponAnim[(int)weaponType].DelaySpawnBullet;
    }
}
