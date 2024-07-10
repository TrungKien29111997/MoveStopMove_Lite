using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowIndicator : GameUnit
{
    [field: SerializeField] public Image ArrowImage { get; private set; }
    [field: SerializeField] public TextMeshProUGUI KillText { get; private set; }

}
