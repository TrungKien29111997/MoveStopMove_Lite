using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CanvasShop : UIInventory
{
    [SerializeField] AnimationClip skillDrawing;
    [SerializeField] RectTransform scrollRect;
    [SerializeField] RectTransform scrollTrans;
    [SerializeField] float offsetIcon;

    [Header("TabSetting")]
    [SerializeField] Color tabChoseColor;
    [SerializeField] Color tabDefaultColor;
    [SerializeField] Image charactrImage;
    [SerializeField] Image hatImage;
    [SerializeField] Image sheilImage;
    [SerializeField] Image skillImage;


    // tempValue
    [SerializeField] List<Icon> icons = new List<Icon>();

    [Header("ButtonSetting")]
    [SerializeField] Button backBut;
    [SerializeField] Button characterBut;
    [SerializeField] Button hatBut;
    [SerializeField] Button sheildBut;
    [SerializeField] Button skillBut;

    public override void SetUp()
    {
        base.SetUp();
        LevelManager.Instance.SetState(EGameState.Inventory);

        anim = waitingHall.CurrentChar.Anim;
        ResetAnimOverride();


        anim.speed = 1;
        currentAnimID = Constant.ANIM_NONE;
        ChangeAnim(Constant.ANIM_IDLE);

        waitingHall.ZoomCam.Priority = 10;

        // set up button
        SetButton(backBut, () => BackButton());
        SetButton(buyBut, () => BuyButton());
        SetButton(characterBut, () => CharacterButton());
        SetButton(hatBut, () => HatButton());
        SetButton(sheildBut, () => SheildButton());
        SetButton(skillBut, () => SkillButton());
    }

    public override void Open()
    {
        base.Open();
        CharacterButton();
    }

    void ResetAnimOverride()
    {
        if (anim != null)
        {
            animatorOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
            anim.runtimeAnimatorController = animatorOverrideController;
        }
    }

    public override void Close(float time)
    {
        base.Close(time);
        if (waitingHall.CurrentSkill != null)
        {
            DespawnAccessory(waitingHall.CurrentSkill);
        }
    }

    void BackButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasLoadingScreen>().SetNextCanvas(() => UIManager.Instance.OpenUI<CanvasMainMenu>(), 0.8f);
        waitingHall.ZoomCam.Priority = 0;
    }

    void CharacterButton()
    {
        ResetInfoItem();

        charactrImage.color = tabChoseColor;

        AddListItem(EItemType.Char, objectPoolScObj.CharacterInfoPrefab);

        currentAnimID = Constant.ANIM_NONE;
        ChangeAnim(Constant.ANIM_IDLE);

        if (waitingHall.CurrentSkill != null)
        {
            DespawnAccessory(waitingHall.CurrentSkill);
        }

        SpwanIcon(objectPoolScObj.CharacterInfoPrefab, EItemType.Char);

        for (int i = 0; i < icons.Count; i++)
        {
            int tmpIndex = i;
            icons[i].SetSelectButton(() => SwitchCharacter(tmpIndex));
        }

        DeactiveBoder();
    }
    void SwitchCharacter(int index)
    {
        ActiveBoder(index);

        if (waitingHall.CurrentChar != null)
        {
            DespawnCharacter();
        }

        CharacterInfo tmpChar = SimplePool.Spawn<CharacterInfo>(objectPoolScObj.CharacterInfoPrefab[index].PoolType, waitingHall.CharPos.position, waitingHall.CharPos.rotation);
        waitingHall.SetChar(tmpChar);

        ReadInfoItem(itemPrefab, index, SavePlayerData.Instance.LoadData().charList, SavePlayerData.Instance.LoadData().curChar);

        if (SavePlayerData.Instance.LoadData().curHat != EPooling.None)
        {
            ItemBuff tmpAccessory = InstanceAccessory(SavePlayerData.Instance.LoadData().curHat, waitingHall.CurrentChar.HeadBone, Vector3.zero);
            waitingHall.SetHat(tmpAccessory);
        }

        if (SavePlayerData.Instance.LoadData().curSheild != EPooling.None)
        {
            ItemBuff tmpAccessory = InstanceAccessory(SavePlayerData.Instance.LoadData().curSheild, waitingHall.CurrentChar.LefttHandPos, Vector3.zero);
            waitingHall.SetSheild(tmpAccessory);
        }

        anim = waitingHall.CurrentChar.Anim;
        ResetAnimOverride();
        ChangeAnim(Constant.ANIM_IDLE);
    }

    void DespawnCharacter()
    {
        SimplePool.Despawn(waitingHall.CurrentChar);
        DespawnAccessory(waitingHall.CurrentHat);
        DespawnAccessory(waitingHall.CurrentSheild);
        DespawnAccessory(waitingHall.CurrentSkill);
    }

    void HatButton()
    {
        ResetInfoItem();

        hatImage.color = tabChoseColor;

        AddListItem(EItemType.Hat, objectPoolScObj.HatPrefab);

        currentAnimID = Constant.ANIM_NONE;
        ChangeAnim(Constant.ANIM_IDLE);
        if (waitingHall.CurrentSkill != null)
        {
            DespawnAccessory(waitingHall.CurrentSkill);
        }

        SpwanIcon(objectPoolScObj.HatPrefab, EItemType.Hat);

        for (int i = 0; i < icons.Count; i++)
        {
            int tmpIndex = i;
            icons[i].SetSelectButton(() => SwitchHat(tmpIndex));
        }

        DeactiveBoder();
    }
    void SheildButton()
    {
        ResetInfoItem();

        sheilImage.color = tabChoseColor;

        AddListItem(EItemType.Sheild, objectPoolScObj.SheildPrefab);

        currentAnimID = Constant.ANIM_NONE;
        ChangeAnim(Constant.ANIM_IDLE);

        if (waitingHall.CurrentSkill != null)
        {
            DespawnAccessory(waitingHall.CurrentSkill);
        }

        SpwanIcon(objectPoolScObj.SheildPrefab, EItemType.Sheild);

        for (int i = 0; i < icons.Count; i++)
        {
            int tmpIndex = i;
            icons[i].SetSelectButton(() => SwitchSheild(tmpIndex));
        }
        DeactiveBoder();
    }

    void SkillButton()
    {
        ResetInfoItem();

        skillImage.color = tabChoseColor;

        AddListItem(EItemType.Skill, objectPoolScObj.SkillPrefab);

        if (waitingHall.CurrentSkill != null)
        {
            DespawnAccessory(waitingHall.CurrentSkill);
        }

        animatorOverrideController[Constant.ANIMSTATE_DRAWING] = skillDrawing;
        currentAnimID = Constant.ANIM_NONE;
        ChangeAnim(Constant.ANIM_DRAWING);
        Invoke(nameof(ResetIdle), 0.3f);

        SpwanIcon(objectPoolScObj.SkillPrefab, EItemType.Skill);

        for (int i = 0; i < icons.Count; i++)
        {
            int tmpIndex = i;
            icons[i].SetSelectButton(() => SwitchSkill(tmpIndex));
        }
        SwitchSkill(0);
        DeactiveBoder();
    }

    void SwitchHat(int tmpIndex)
    {
        ActiveBoder(tmpIndex);
        waitingHall.SetHat(SwitchAccessory(waitingHall.CurrentHat, objectPoolScObj.HatPrefab, tmpIndex, waitingHall.CurrentChar.HeadBone, Vector3.zero, SavePlayerData.Instance.LoadData().curHat));
    }

    void SwitchSheild(int tmpIndex)
    {
        ActiveBoder(tmpIndex);
        waitingHall.SetSheild(SwitchAccessory(waitingHall.CurrentSheild, objectPoolScObj.SheildPrefab, tmpIndex, waitingHall.CurrentChar.LefttHandPos, Vector3.zero, SavePlayerData.Instance.LoadData().curSheild));
    }

    void SwitchSkill(int tmpIndex)
    {
        ActiveBoder(tmpIndex);
        waitingHall.SetSkill(SwitchAccessory(waitingHall.CurrentSkill, objectPoolScObj.SkillPrefab, tmpIndex, waitingHall.CurrentChar.RightHandPos, Vector3.up * 0.1f, SavePlayerData.Instance.LoadData().curSkill));
    }

    ItemBuff SwitchAccessory(ItemBuff currentAcc, List<GameUnit> listPrefab, int index, Transform pos, Vector3 localPos, EPooling currentItem)
    {
        DespawnAccessory(currentAcc);
        ItemBuff tmpAccessory = InstanceAccessory(listPrefab[index].PoolType, pos, localPos);
        ReadInfoItem(itemPrefab, index, SavePlayerData.Instance.LoadData().accessoryList, currentItem);
        return tmpAccessory;
    }

    ItemBuff InstanceAccessory(EPooling tmpType, Transform parent, Vector3 localPos)
    {
        ItemBuff tmpAccessory = SimplePool.Spawn<ItemBuff>(tmpType, Vector3.zero, Quaternion.identity);
        tmpAccessory.TF.SetParent(parent);
        tmpAccessory.TF.SetLocalPositionAndRotation(localPos, Quaternion.identity);
        return tmpAccessory;
    }

    void DespawnAccessory(ItemBuff tmpAccessory)
    {
        if (tmpAccessory != null)
        {
            SimplePool.Despawn(tmpAccessory);
            tmpAccessory.TF.SetParent(null);
        }
    }

    void SpwanIcon(List<GameUnit> tmpList, EItemType tmpType)
    {
        if (icons.Count > 0)
        {
            for (int i = 0; i < icons.Count; i++)
            {
                SimplePool.Despawn(icons[i]);
            }
            icons.Clear();
        }

        if (tmpList.Count <= 2)
        {
            scrollTrans.sizeDelta = new Vector2(2 * offsetIcon, scrollRect.rect.height);
        }
        else
        {
            if (tmpList.Count % 2 == 0)
            {
                scrollTrans.sizeDelta = new Vector2((tmpList.Count / 2) * offsetIcon, scrollRect.rect.height);
            }
            else
            {
                scrollTrans.sizeDelta = new Vector2(((tmpList.Count / 2) + 1) * offsetIcon, scrollRect.rect.height);
            }
        }

        for (int i = 0; i < tmpList.Count; i++)
        {
            Icon tmpIcon = SimplePool.Spawn<Icon>(EPooling.IconItem, this.transform.position, Quaternion.identity);
            icons.Add(tmpIcon);

            tmpIcon.SetIcon(tmpList[i].IconImage, this, tmpType, tmpList[i].PoolType);

            tmpIcon.TF.SetParent(scrollTrans);
            tmpIcon.TF.localScale = Vector3.one;
        }
    }

    void ActiveBoder(int index)
    {
        icons[index].Boder.SetActive(true);
        for (int i = 0; i < icons.Count; i++)
        {
            if (i != index)
            {
                icons[i].Boder.SetActive(false);
            }
        }
    }

    void DeactiveBoder()
    {
        if (icons.Count > 0)
        {
            for (int i = 0; i < icons.Count; i++)
            {
                icons[i].Boder.SetActive(false);
            }
        }
    }
    void ResetIdle()
    {
        currentAnimID = Constant.ANIM_NONE;
    }

    void ResetInfoItem()
    {
        if(itemPrice != null) itemPrice.text = "";
        if (itemName != null) itemName.text = "";
        if (itemPower != null) itemPower.text = "";
        if (itemStatus != null) itemStatus.text = "";
        if (unequipObj != null) unequipObj.SetActive(false);
        if (equipObj != null) equipObj.SetActive(false);
        if (buyObj != null) buyObj.SetActive(false);

        if (charactrImage != null) charactrImage.color = tabDefaultColor;
        if (hatImage != null) hatImage.color = tabDefaultColor;
        if (sheilImage != null) sheilImage.color = tabDefaultColor;
        if (skillImage != null) skillImage.color = tabDefaultColor;
    }
}
