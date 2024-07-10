using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Icon : GameUnit
{
    [field: SerializeField] public RectTransform RTF { get; private set; }
    [field: SerializeField] public EPooling TypeItem { get; private set; }
    [field: SerializeField] public UIInventory CurrentCanvas { get; private set; }
    [field: SerializeField] public GameObject Boder { get; private set; }
    [SerializeField] Image mainImage;
    [SerializeField] Button selectButton;

    private void Start()
    {
        Boder.SetActive(false);
    }

    public void SetIcon(Sprite tmpSprite, UIInventory tmpCanvas, EItemType tmpType, EPooling type)
    {
        if (tmpSprite != null)
        {
            mainImage.sprite = tmpSprite;
        }
        if (tmpCanvas != null)
        {
            CurrentCanvas = tmpCanvas;
        }
        ItemType = tmpType;
        TypeItem = type;
    }

    public void SetSelectButton(UnityAction tmpAction)
    {
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(tmpAction);
    }
}
