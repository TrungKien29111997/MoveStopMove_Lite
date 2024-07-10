using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveShopData : MonoBehaviour
{
    [Header("GeneralSetting")]
    [SerializeField] bool encode;
    [SerializeField] ShopData shopData;
    const string shopDataPath = "ShopData.lol";
    private void Awake()
    {
        shopData = LoadData();
    }
    void SaveData()
    {
        SaveGame.Save(shopDataPath, shopData, encode);
    }
    public ShopData LoadData()
    {
        return SaveGame.Load(shopDataPath, new ShopData(), encode);
    }
    public void PushItem(List<EPooling> tmpItem)
    {
        shopData.item.Clear();
        for (int i = 0; i < tmpItem.Count; i++)
        {
            shopData.item.Add((int)tmpItem[i]);
        }
        SaveData();
    }
}

[System.Serializable]
public class ShopData
{
    [field: SerializeField] public List<int> item { get; private set; } = new List<int>();
    public ShopData()
    {
        item.Add(0);
    }
}
