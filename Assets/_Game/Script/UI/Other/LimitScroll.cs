using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitScroll : MonoBehaviour
{
    [SerializeField] RectTransform rtf;
    [SerializeField] Vector2 limitScroll;
    Vector3 limitUp => new Vector3(0, limitScroll.y, 0);
    Vector3 limitDown => new Vector3(0, limitScroll.x, 0);

    private void Update()
    {
        if (rtf.localPosition.y > limitScroll.y)
        {
            rtf.localPosition = limitUp;
        }
        else if (rtf.localPosition.y < limitScroll.x)
        {
            rtf.localPosition = limitDown;
        }
    }

    public void SetLimit(float min, float max)
    {
        limitScroll.x = min;
        limitScroll.y = max;
    }
}
