using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UICanvas : MonoBehaviour
{
    [SerializeField] bool isDestroyOnClose = false;

    private void Awake()
    {
        // xu ly title
        RectTransform rect = GetComponent<RectTransform>();
        float ratio = (float)Screen.width / (float)Screen.height;
        if (ratio > 2.1f)
        {
            Vector2 leftBottom = rect.offsetMin;
            Vector2 rightTop = rect.offsetMax;

            leftBottom.y = 0f;
            rightTop.y = -100f;

            rect.offsetMin = leftBottom;
            rect.offsetMax = rightTop;
        }
    }

    // goi truoc khi canvas duoc active
    public virtual void SetUp()
    {

    }

    // goi sau khi canvas duoc active
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    // tat canvas sau bao nhieu giay
    public virtual void Close(float time)
    {
        Invoke(nameof(CloseDirecly), time);
    }

    // tat canvas truc tiep 
    public virtual void CloseDirecly()
    {
        if (isDestroyOnClose)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    protected void SetButton(Button tmpButton, UnityAction tmpAction)
    {
        tmpButton.onClick.RemoveAllListeners();
        tmpButton.onClick.AddListener(tmpAction);
    }
}
