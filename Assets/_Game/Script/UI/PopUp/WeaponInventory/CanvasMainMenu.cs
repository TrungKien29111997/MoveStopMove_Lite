using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasMainMenu : UIInventory
{
    [SerializeField] TextMeshProUGUI bestKillText;

    [Header("ButtonSetting")]
    [SerializeField] Button playBut;
    [SerializeField] Button weaponBut;
    [SerializeField] Button shopBut;
    [SerializeField] OnOff1Button soundBut;
    [SerializeField] OnOff1Button vibrateBut;


    public override void SetUp()
    {
        base.SetUp();
        LevelManager.Instance.SetState(EGameState.MainMenu);
        LevelManager.Instance.OnDespawn();

        // set up button
        SetButton(playBut, PlayButton);
        SetButton(weaponBut,  WeaponButton);
        SetButton(shopBut, ShopButton);

        // set up control bool and on - off action
        soundBut.SetButton(LevelManager.Instance.AudioStatus, () => LevelManager.Instance.SetAudioStatus(true), () => LevelManager.Instance.SetAudioStatus(false));

        vibrateBut.SetButton(LevelManager.Instance.VibrationStatus, () => LevelManager.Instance.SetVibrationStatus(true), () => LevelManager.Instance.SetVibrationStatus(false));
    }

    public override void Open()
    {
        base.Open();
        LevelManager.Instance.OnInit();
        LevelManager.Instance.IndicatorGroup.alpha = 0;

        if (coin != null)
        {
            UpdateCoin();
        }

        bestKillText.text = SavePlayerData.Instance.LoadData().best.ToString();

        Invoke(nameof(DelaySpawnChar), 0.2f);
    }

    void DelaySpawnChar()
    {
        if (waitingHall.CurrentChar == null)
        {
            CharacterInfo tmpChar = SimplePool.Spawn<CharacterInfo>(SavePlayerData.Instance.LoadData().curChar, waitingHall.CharPos.position, waitingHall.CharPos.rotation);
            waitingHall.SetChar(tmpChar);
        }

        anim = waitingHall.CurrentChar.Anim;
        if (anim != null)
        {
            animatorOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
            anim.runtimeAnimatorController = animatorOverrideController;
        }

        currentAnimID = Constant.ANIM_NONE;
        ChangeAnim(Constant.ANIM_IDLE);
        waitingHall.WaitingHallCam.Priority = 10;
    }

    public override void Close(float time)
    {
        base.Close(time);
        waitingHall.WaitingHallCam.Priority = 0;
        LevelManager.Instance.IndicatorGroup.alpha = 1;
    }

    void PlayButton()
    {
        Close(0);
        LevelManager.Instance.WaitingPos.WaitingHallCam.Priority = 0;
        LevelManager.Instance.NewLevel();
        waitingHall.RemoveAll();
        UIManager.Instance.OpenUI<CanvasLoadingScreen>().SetNextCanvas(() => UIManager.Instance.OpenUI<CanvasGamePlay>(), 1.5f);
    }
    void WeaponButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasWeaponInventory>();
    }

    void ShopButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasShop>();
    }
}
