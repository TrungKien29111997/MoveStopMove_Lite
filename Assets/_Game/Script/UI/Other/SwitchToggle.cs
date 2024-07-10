using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] RectTransform handle;
    [SerializeField] Toggle toggle;
    [SerializeField] UnityAction onAction, offAction;
    [SerializeField] Vector2 hanldePos;

    public void SetButton(UnityAction onAc, UnityAction offAc)
    {
        onAction = onAc;
        offAction = offAc;

        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(OnSwitch);

        if (toggle.isOn)
        {
            handle.anchoredPosition = hanldePos * -1;
        }
        else
        {
            handle.anchoredPosition = hanldePos;
        }
    }

    void OnSwitch(bool on)
    {
        if (on)
        {
            handle.anchoredPosition = hanldePos * -1;
            onAction.Invoke();
        }
        else
        {
            handle.anchoredPosition = hanldePos;
            offAction.Invoke();
        }
    }

    public void SetOnToggle(bool on)
    {
        toggle.SetIsOnWithoutNotify(on);
    }
}
