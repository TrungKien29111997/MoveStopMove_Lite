using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyTest : MonoBehaviour
{
    [SerializeField] ObjectPoolScObj objectPoolData;
    [SerializeField] bool isBuyChar;
    [SerializeField] bool isBuyWeap;
    [SerializeField] EPooling objToBuy;

    private void Update()
    {
        if (isBuyChar)
        {
            if (!SavePlayerData.Instance.LoadData().charList.Contains(objToBuy))
            {
                SavePlayerData.Instance.AddCharName(objToBuy);
            }
            isBuyChar = false;
        }
        if (isBuyWeap)
        {
            if (!SavePlayerData.Instance.LoadData().weaponList.Contains(objToBuy))
            {
                SavePlayerData.Instance.AddWeaponMesh(objToBuy);
            }
            isBuyWeap = false;
        }
    }
}
