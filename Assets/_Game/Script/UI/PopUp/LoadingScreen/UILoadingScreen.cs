using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UILoadingScreen : UICanvas
{
    [SerializeField] protected Animator aim;
    [SerializeField] UnityAction action;

    public override void SetUp()
    {
        base.SetUp();
        LevelManager.Instance.SetState(EGameState.None);
    }

    public void SetNextCanvas(UnityAction tmpAction, float delayTime)
    {
        action = tmpAction;
        Invoke(nameof(Fade), delayTime);
    }

    protected virtual void Fade()
    {
        if (aim != null)
        {
            aim.SetTrigger(Constant.ANIM_END);
            action.Invoke();
            Close(1);
        }
    }
}
