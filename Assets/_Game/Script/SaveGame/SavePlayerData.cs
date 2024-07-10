using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayerData : Singleton<SavePlayerData>
{
    [Header("GeneralSetting")]
    [SerializeField] bool encode;
    [SerializeField] PlayerData playerData = new PlayerData();

    const string playerDataPath = "PlayerData.lol";

    private void Awake()
    {
        playerData = LoadData();
    }
    void SaveData()
    {
        SaveGame.Save(playerDataPath, playerData, encode);
    }
    public PlayerData LoadData()
    {
        return SaveGame.Load(playerDataPath, new PlayerData(), encode);
    }

    // add info to inventory
    public void AddCharName(EPooling tmpChar)
    {
        playerData.charList.Add(tmpChar);
        SaveData();
    }
    public void AddWeaponMesh(EPooling tmpWeapon)
    {
        playerData.weaponList.Add(tmpWeapon);
        SaveData();
    }
    public void AddAccessory(EPooling tmpAcc)
    {
        playerData.accessoryList.Add(tmpAcc);
        SaveData();
    }

    public void IncreaseCoin(int tmpCoin)
    {
        playerData.coin += tmpCoin;
        SaveData();
    }

    // set current info to player
    public void SetCurrentChar(EPooling tmpChar)
    {
        playerData.curChar = tmpChar;
        SaveData();
    }
    public void SetCurrentWeapon(EPooling tmpWeapon)
    {
        playerData.curWeap = tmpWeapon;
        SaveData();
    }

    public void SetCurrentHat(EPooling tmpAcc)
    {
        playerData.curHat = tmpAcc;
        SaveData();
    }

    public void SetCurrentSheild(EPooling tmpAcc)
    {
        playerData.curSheild = tmpAcc;
        SaveData();
    }

    public void SetCurrentSkill(EPooling tmpAcc)
    {
        playerData.curSkill = tmpAcc;
        SaveData();
    }

    public void SetBestKill(int tmpKill)
    {
        playerData.best = tmpKill;
        SaveData();
    }

}
[System.Serializable]
public class PlayerData
{
    public EPooling curChar;
    public EPooling curWeap;
    public EPooling curHat;
    public EPooling curSheild;
    public EPooling curSkill;
    public int best;
    public int coin;
    public List<EPooling> charList = new List<EPooling>();
    public List<EPooling> weaponList = new List<EPooling>();
    public List<EPooling> accessoryList = new List<EPooling>();

    public PlayerData()
    {
        curChar = EPooling.CharHoshino;
        curWeap = EPooling.MeshSword1;
        curHat = EPooling.Hat1;
        best = 0;
        coin = 0;
        charList.Add(EPooling.CharHoshino);
        weaponList.Add(EPooling.MeshSword1);
        weaponList.Add(EPooling.MeshHammer1);
        weaponList.Add(EPooling.MeshGun1);
        accessoryList.Add(EPooling.Hat1);
    }
}
