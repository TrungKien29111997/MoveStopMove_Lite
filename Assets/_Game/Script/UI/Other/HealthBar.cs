using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBar : LookAtCam
{
    [SerializeField] TextMeshProUGUI nameOfObj;

    public void SetName(string tmpName, Color tmpColor)
    {
        nameOfObj.text = tmpName;
        nameOfObj.faceColor = tmpColor;
    }
}
