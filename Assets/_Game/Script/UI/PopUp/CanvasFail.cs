using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFail : UICanvas
{
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("ButtonSetting")]
    [SerializeField] Button mainMenuBut;
    [SerializeField] Button playBut;
    public override void SetUp()
    {
        base.SetUp();
        LevelManager.Instance.SetState(EGameState.Fail);

        SetButton(mainMenuBut, () => MainMenuButton());
        SetButton(playBut, () => AgainButton());
    }
    public override void Open()
    {
        base.Open();
        UpdateScore(LevelManager.Instance.Player.KillCount);
        if (LevelManager.Instance.Player.KillCount > SavePlayerData.Instance.LoadData().best)
        {
            SavePlayerData.Instance.SetBestKill(LevelManager.Instance.Player.KillCount);
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    void MainMenuButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasLoadingScreen>().SetNextCanvas(() => UIManager.Instance.OpenUI<CanvasMainMenu>(), 1.2f);
    }

    void AgainButton()
    {
        Close(0);
        LevelManager.Instance.ResetLevel();
        UIManager.Instance.OpenUI<CanvasGamePlay>();
    }
}
