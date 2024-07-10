using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] string triggerName;
    [SerializeField] GameObject skillPrefab;
    [SerializeField] bool start;

    private void Start()
    {
        start = false;
    }

    private void Update()
    {
        if (start)
        {
            anim.SetTrigger(triggerName);
            skillPrefab.SetActive(true);
            start = false;
        }
    }
}
