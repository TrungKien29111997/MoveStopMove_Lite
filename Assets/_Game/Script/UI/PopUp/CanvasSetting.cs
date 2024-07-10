using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSetting : UICanvas
{
    [Header("ButtonSetting")]
    [SerializeField] Button mainMenuBut;
    [SerializeField] Button continueBut;

    [SerializeField] SwitchToggle soundToggle;
    [SerializeField] SwitchToggle vibrationToggle;

    public override void SetUp()
    {
        base.SetUp();
        LevelManager.Instance.SetState(EGameState.Setting);

        SetButton(mainMenuBut, MainMenuButton);
        SetButton(continueBut, ContinueButton);

        // set current status of button
        soundToggle.SetOnToggle(LevelManager.Instance.AudioStatus);
        vibrationToggle.SetOnToggle(LevelManager.Instance.VibrationStatus);

        // set on - off action
        soundToggle.SetButton(() => LevelManager.Instance.SetAudioStatus(true), () => LevelManager.Instance.SetAudioStatus(false));

        vibrationToggle.SetButton(() => LevelManager.Instance.SetVibrationStatus(true), () => LevelManager.Instance.SetVibrationStatus(false));
    }

    void MainMenuButton()
    {
        UIManager.Instance.CloseAll();
        Invoke(nameof(DelayOpenMainMenu), 0.3f);
    }

    void DelayOpenMainMenu()
    {
        UIManager.Instance.OpenUI<CanvasLoadingScreen>().SetNextCanvas(() => UIManager.Instance.OpenUI<CanvasMainMenu>(), 1.2f);
    }

    void ContinueButton()
    {
        Close(0);
        LevelManager.Instance.SetState(EGameState.GamePlay);
    }
}
