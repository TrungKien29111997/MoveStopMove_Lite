using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGamePlay : UICanvas
{
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] PlayerController controller;

    [Header("ButtonSetting")]
    [SerializeField] Button settingBut;

    IEnumerator UpdateAlive()
    {
        while(true)
        {
            UpdateCoin(LevelManager.Instance.Alive);
            if (LevelManager.Instance.IsPlayerDead)
            {
                Close(0);
                UIManager.Instance.OpenUI<CanvasFail>();
                break;
            }
            else if (LevelManager.Instance.IsPlayerSurvive)
            {
                Close(0);
                UIManager.Instance.OpenUI<CanvasVictory>();
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public override void SetUp()
    {
        base.SetUp();
        SetButton(settingBut, SettingButton);
    }

    public override void Open()
    {
        base.Open();
        LevelManager.Instance.SetState(EGameState.GamePlay);

        LevelManager.Instance.Player.SetController(controller);
        controller.SetJoyStickAlpha(0);
        StartCoroutine(UpdateAlive());
    }

    public override void Close(float time)
    {
        base.Close(time);
        LevelManager.Instance.Player.SetController(null);
        controller.SetJoyStickAlpha(0);
    }

    public void UpdateCoin(int coin)
    {
        coinText.text = coin.ToString();
    }

    public void SettingButton()
    {
        UIManager.Instance.OpenUI<CanvasSetting>();
    }
}
