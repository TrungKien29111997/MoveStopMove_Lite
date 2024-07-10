using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasWeaponInventory : UIInventory
{
    [SerializeField] WeaponScObj weaponScObj;

    [Header("WeaponData")]
    [SerializeField] Image weaponIcon;
    int curWeaponIndex;

    [Header("ButtonSetting")]
    [SerializeField] Button swordShop;
    [SerializeField] Button hammerShop;
    [SerializeField] Button gunShop;
    [SerializeField] Button backBut;
    [SerializeField] Button nextBut;
    [SerializeField] Button previousBut;

    public override void SetUp()
    {
        base.SetUp();
        LevelManager.Instance.SetState(EGameState.Inventory);
        buyObj.SetActive(true);

        anim = waitingHall.CurrentChar.Anim;
        if (anim != null)
        {
            animatorOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
            anim.runtimeAnimatorController = animatorOverrideController;
        }
        waitingHall.ZoomCam.Priority = 10;

        // set up button
        SetButton(backBut, () => BackButton());
        SetButton(swordShop, () => SwordButton());
        SetButton(hammerShop, () => HammerButton());
        SetButton(gunShop, () => GunButton());
        SetButton(nextBut, () => NextWeapon());
        SetButton(previousBut, () => PreviousWeapon());
        SetButton(buyBut, () => BuyButton());
    }

    public override void Open()
    {
        base.Open();
        SwordButton();
    }
    public override void Close(float time)
    {
        base.Close(time);

        itemPrefab.Clear();

        SimplePool.Despawn(waitingHall.CurrentWeapon);

        waitingHall.ZoomCam.Priority = 0;
    }
    void InHandWeapon(EPooling tmpWeapon)
    {
        Weapon tmpWeap;
        if (waitingHall.CurrentWeapon != null)
        {
            SimplePool.Despawn(waitingHall.CurrentWeapon);
        }

        if (tmpWeapon != SavePlayerData.Instance.LoadData().curWeap)
        {
            tmpWeap = SimplePool.Spawn<Weapon>(tmpWeapon, this.transform.position, Quaternion.identity);
        }
        else
        {
            tmpWeap = SimplePool.Spawn<Weapon>(SavePlayerData.Instance.LoadData().curWeap, this.transform.position, Quaternion.identity);
        }

        tmpWeap.TF.SetParent(LevelManager.Instance.WaitingPos.CurrentChar.RightHandPos);
        tmpWeap.TF.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        waitingHall.SetWeapon(tmpWeap);
    }

    void SwordButton()
    {
        AddListItem(EItemType.Sword, objectPoolScObj.SwordPrefab);
        ReadListWeapon(objectPoolScObj.SwordPrefab, 0);
    }

    void HammerButton()
    {
        AddListItem(EItemType.Hammer, objectPoolScObj.HammerPrefab);
        ReadListWeapon(objectPoolScObj.HammerPrefab, 0);
    }

    void GunButton()
    {
        AddListItem(EItemType.Gun, objectPoolScObj.GunPrefab);
        ReadListWeapon(objectPoolScObj.GunPrefab, 0);
    }

    void ReadListWeapon(List<GameUnit> weaponList, int index)
    {
        curWeaponIndex = 0;

        ReadInfoItem(itemPrefab,index, SavePlayerData.Instance.LoadData().weaponList, SavePlayerData.Instance.LoadData().curWeap);

        InHandWeapon(itemPrefab[0].PoolType);

        animatorOverrideController[Constant.ANIMSTATE_DRAWING] = weaponScObj.GetAnim(waitingHall.CurrentWeapon.ItemType).CharPullOutClip;
        anim.speed = 1;
        ChangeAnim(Constant.ANIM_DRAWING);
        Invoke(nameof(ResetIdle), 0.3f);
    }

    protected override EPooling ReadInfoItem(List<ItemBuff> tmpList, int index, List<EPooling> playerList, EPooling currentItem)
    {
        weaponIcon.sprite = itemPrefab[index].IconImage;
        return base.ReadInfoItem(tmpList, index, playerList, currentItem);
    }

    void NextWeapon()
    {
        curWeaponIndex++;
        if (curWeaponIndex < itemPrefab.Count)
        {
            ReadInfoItem(itemPrefab, curWeaponIndex, SavePlayerData.Instance.LoadData().weaponList, SavePlayerData.Instance.LoadData().curWeap);
        }
        else
        {
            curWeaponIndex = 0;
            ReadInfoItem(itemPrefab, 0, SavePlayerData.Instance.LoadData().weaponList, SavePlayerData.Instance.LoadData().curWeap);
        }
        InHandWeapon(itemPrefab[curWeaponIndex].PoolType);
    }

    void PreviousWeapon()
    {
        curWeaponIndex--;
        if (curWeaponIndex >= 0)
        {
            ReadInfoItem(itemPrefab, curWeaponIndex, SavePlayerData.Instance.LoadData().weaponList, SavePlayerData.Instance.LoadData().curWeap);
        }
        else
        {
            curWeaponIndex = itemPrefab.Count - 1;
            ReadInfoItem(itemPrefab, curWeaponIndex, SavePlayerData.Instance.LoadData().weaponList, SavePlayerData.Instance.LoadData().curWeap);
        }
        InHandWeapon(itemPrefab[curWeaponIndex].PoolType);
    }
    void BackButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasLoadingScreen>().SetNextCanvas(() => UIManager.Instance.OpenUI<CanvasMainMenu>(), 0.8f);
    }

    void ResetIdle()
    {
        currentAnimID = Constant.ANIM_NONE;
    }


}
