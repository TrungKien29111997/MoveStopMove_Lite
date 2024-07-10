using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : UICanvas
{
    protected Animator anim;
    protected AnimatorOverrideController animatorOverrideController;
    protected string currentAnimID;
    protected WaitingHall waitingHall;
    [SerializeField] protected TextMeshProUGUI coin;

    [SerializeField] protected ObjectPoolScObj objectPoolScObj;
    [SerializeField] protected UIScObj uIScObj;

    [Header("ItemData")]
    [SerializeField] protected EItemType itemType;
    [SerializeField] protected TextMeshProUGUI itemName;
    [SerializeField] protected TextMeshProUGUI itemPower;
    [SerializeField] protected TextMeshProUGUI itemPrice;
    [SerializeField] protected TextMeshProUGUI itemStatus;
    [SerializeField] protected Image buyImage;

    [Header("Buy/Equip Button")]
    [SerializeField] protected Button buyBut;
    [SerializeField] protected GameObject buyObj;

    [SerializeField] protected Button equipBut;
    [SerializeField] protected GameObject equipObj;

    [SerializeField] protected Button unequipBut;
    [SerializeField] protected GameObject unequipObj;

    // tempValue
    protected int price;
    protected EPooling itemNum;
    protected List<ItemBuff> itemPrefab = new List<ItemBuff>();

    public override void SetUp()
    {
        base.SetUp();
        waitingHall = LevelManager.Instance.WaitingPos;
        if (coin != null)
        {
            UpdateCoin();
        }
        if (equipObj != null) SetButton(equipBut, () => EquipButton());
        if (unequipObj != null) SetButton(unequipBut, () => UnEquipButton());
    }

    public override void Open()
    {
        base.Open();
        LevelManager.Instance.IndicatorGroup.alpha = 0;
        if (coin != null)
        {
            coin.text = SavePlayerData.Instance.LoadData().coin.ToString();
        }
        if (equipObj != null) equipObj.SetActive(false);
        if (unequipObj != null) unequipObj.SetActive(false);
        if (buyObj != null) buyObj.SetActive(false);
    }

    public override void Close(float time)
    {
        base.Close(time);
        LevelManager.Instance.IndicatorGroup.alpha = 1;
    }

    public virtual void UpdateCoin()
    {
        coin.text = SavePlayerData.Instance.LoadData().coin.ToString();
    }
    public virtual void BuyButton()
    {
        UpdateCoin();

        SavePlayerData.Instance.IncreaseCoin(-price);

        if (itemType == EItemType.Char)
        {
            SavePlayerData.Instance.AddCharName(itemNum);
        }

        if (itemType == EItemType.Hat || itemType == EItemType.Sheild || itemType == EItemType.Skill)
        {
            SavePlayerData.Instance.AddAccessory(itemNum);
        }

        if (itemType == EItemType.Gun || itemType == EItemType.Hammer || itemType == EItemType.Sword)
        {
            SavePlayerData.Instance.AddWeaponMesh(itemNum);
        }

        if (equipObj != null) equipObj.SetActive(true);
        if (unequipObj != null) unequipObj.SetActive(false);
        if (buyObj != null) buyObj.SetActive(false);

        this.UpdateCoin();
    }

    public virtual void EquipButton()
    {
        switch(itemType)
        {
            case EItemType.Hat:
                SavePlayerData.Instance.SetCurrentHat(itemNum);
                break;
            case EItemType.Sheild:
                SavePlayerData.Instance.SetCurrentSheild(itemNum);
                break;
            case EItemType.Skill:
                SavePlayerData.Instance.SetCurrentSkill(itemNum);
                break;
            case EItemType.Char:
                SavePlayerData.Instance.SetCurrentChar(itemNum);
                break;
            case EItemType.Sword:
                SavePlayerData.Instance.SetCurrentWeapon(itemNum);
                break;
            case EItemType.Hammer:
                SavePlayerData.Instance.SetCurrentWeapon(itemNum);
                break;
            case EItemType.Gun:
                SavePlayerData.Instance.SetCurrentWeapon(itemNum);
                break;
        }
        if (equipObj != null) equipObj.SetActive(false);
        if (unequipObj != null) unequipObj.SetActive(true);
        if (buyObj != null) buyObj.SetActive(false);
    }

    public virtual void UnEquipButton()
    {
        switch (itemType)
        {
            case EItemType.Hat:
                SavePlayerData.Instance.SetCurrentHat(EPooling.None);
                break;
            case EItemType.Sheild:
                SavePlayerData.Instance.SetCurrentSheild(EPooling.None);
                break;
            case EItemType.Skill:
                SavePlayerData.Instance.SetCurrentSkill(EPooling.None);
                break;
        }
        if (equipObj != null) equipObj.SetActive(true);
        if (unequipObj != null) unequipObj.SetActive(false);
        if (buyObj != null) buyObj.SetActive(false);
    }

    protected virtual void AddListItem(EItemType tmpType, List<GameUnit> scObjList)
    {
        itemType = tmpType;
        itemPrefab.Clear();
        for (int i = 0; i < scObjList.Count; i++)
        {
            itemPrefab.Add(scObjList[i] as ItemBuff);
        }
    }

    protected void ChangeAnim(string animID)
    {
        if (currentAnimID != animID)
        {
            anim.ResetTrigger(currentAnimID);
            currentAnimID = animID;
            anim.SetTrigger(currentAnimID);
        }
    }

    protected virtual EPooling ReadInfoItem(List<ItemBuff> tmpList, int index, List<EPooling> playerList, EPooling currentItem)
    {
        itemPrice.text = tmpList[index].Price.ToString();
        if (itemName != null)
        {
            itemName.text = tmpList[index].ItemName.ToString();
        }
        price = tmpList[index].Price;
        if (tmpList[index].BuffRange > 0)
        {
            itemPower.text = "+" + tmpList[index].BuffRange + " Range";
        }
        else if (tmpList[index].BuffSpeed > 0)
        {
            itemPower.text = "+" + tmpList[index].BuffSpeed + " Speed";
        }
        else
        {
            itemPower.text = "No power";
        }

        // if player had then unlock icon
        if (playerList.Contains(tmpList[index].PoolType))
        {
            itemStatus.text = "";
            buyObj.SetActive(false);
        }
        else
        {
            itemStatus.text = "(Lock)";
            buyObj.SetActive(true);
        }

        if (playerList.Contains(tmpList[index].PoolType))
        {
            if (currentItem == tmpList[index].PoolType)
            {
                unequipObj.SetActive(true);
                equipObj.SetActive(false);
                buyObj.SetActive(false);
            }
            else
            {
                unequipObj.SetActive(false);
                equipObj.SetActive(true);
                buyObj.SetActive(false);
            }
        }
        else
        {
            // if player had enough money of not
            if (SavePlayerData.Instance.LoadData().coin >= price)
            {
                buyBut.enabled = true;
                buyImage.color = uIScObj.EnoughMoney;

                equipObj.SetActive(false);
                unequipObj.SetActive(false);
                buyObj.SetActive(true);
            }
            else
            {
                buyBut.enabled = false;
                buyImage.color = uIScObj.NotEnoughMoney;

                equipObj.SetActive(false);
                unequipObj.SetActive(false);
                buyObj.SetActive(true);
            }
        }

        itemNum = tmpList[index].PoolType;

        return itemNum;
    }
}
