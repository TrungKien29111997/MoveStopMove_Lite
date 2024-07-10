using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class OnOff1Button : MonoBehaviour
{
    [SerializeField] Button soureButton;
    [SerializeField] Image soureImage;
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;
    [SerializeField] UnityAction onAction, offAction;
    bool status;

    public void SetButton(bool tmpStatus, UnityAction onAc, UnityAction offAc)
    {
        status = tmpStatus;

        if (status)
        {
            soureImage.sprite = onSprite;
        }
        else
        {
            soureImage.sprite = offSprite;
        }

        onAction = onAc;
        offAction = offAc;

        soureButton.onClick.RemoveAllListeners();
        soureButton.onClick.AddListener(OnSwitch);
    }

    void OnSwitch()
    {
        if (!status)
        {
            onAction.Invoke();
            soureImage.sprite = onSprite;
            status = true;
        }
        else
        {
            offAction.Invoke();
            soureImage.sprite = offSprite;
            status = false;
        }
    }
}
