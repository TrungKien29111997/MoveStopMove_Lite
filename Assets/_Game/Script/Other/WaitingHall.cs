using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaitingHall : MonoBehaviour
{
    //[Header("InventorySettings")]
    [field: SerializeField] public Transform CharPos { get; private set; }
    [field: SerializeField] public CinemachineVirtualCamera WaitingHallCam { get; private set; }
    [field: SerializeField] public CinemachineVirtualCamera ZoomCam { get; private set; }
    [field: SerializeField] public CharacterInfo CurrentChar { get; private set; }
    [field: SerializeField] public Weapon CurrentWeapon { get; private set; }
    [field: SerializeField] public ItemBuff CurrentHat { get; private set; }
    [field: SerializeField] public ItemBuff CurrentSheild { get; private set; }
    [field: SerializeField] public ItemBuff CurrentSkill{ get; private set; }

    // waiting hall
    public void SetChar(CharacterInfo tmpChar)
    {
        CurrentChar = tmpChar;
    }

    public void SetWeapon(Weapon tmpMesh)
    {
        CurrentWeapon = tmpMesh;
    }

    public void SetHat(ItemBuff tmpAcc)
    {
        CurrentHat = tmpAcc;
    }

    public void SetSheild(ItemBuff tmpAcc)
    {
        CurrentSheild = tmpAcc;
    }
    public void SetSkill(ItemBuff tmpAcc)
    {
        CurrentSkill = tmpAcc;
    }

    // clear
    public void RemoveAll()
    {
        if (CurrentChar != null)
        {
            SimplePool.Despawn(CurrentChar);
            CurrentChar = null;
        }
        if (CurrentWeapon != null)
        {
            SimplePool.Despawn(CurrentWeapon);
            CurrentWeapon = null;
        }
    }

    public void ClearWeapon()
    {
        CurrentWeapon = null;
    }
}
