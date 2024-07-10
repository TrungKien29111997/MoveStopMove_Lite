using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasVictory : UICanvas
{
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("ButtonSetting")]
    [SerializeField] Button mainMenuBut;
    [SerializeField] Button nextBut;

    public override void SetUp()
    {
        base.SetUp();
        LevelManager.Instance.SetState(EGameState.Victory);

        SetButton(mainMenuBut, () => MainMenuButton());
        SetButton(nextBut, () => NextButton());
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

    public void MainMenuButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasLoadingScreen>().SetNextCanvas(() => UIManager.Instance.OpenUI<CanvasMainMenu>(), 1.2f);
    }
    public void NextButton()
    {
        Close(0);
        LevelManager.Instance.NextLevel();
        UIManager.Instance.OpenUI<CanvasLoadingScreen>().SetNextCanvas(() => UIManager.Instance.OpenUI<CanvasGamePlay>(), 1.2f);
    }
}
