using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleRing : MonoBehaviour
{
    [SerializeField] Transform[] starTrans;
    Transform tf;
    public Transform TF
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }
    [field: SerializeField] public Renderer[] RingRender { get; private set; }
    int starCount;

    public void OnInit()
    {
        for (int i = 0; i < starTrans.Length; i++)
        {
            starTrans[i].gameObject.SetActive(false);
        }
        starCount = 0;
    }

    public void SetColor(Material mat)
    {
        for (int i = 0; i < RingRender.Length; i++)
        {
            RingRender[i].material = mat;
        }
    }

    public void IncreaseStar()
    {
        if (starCount < starTrans.Length)
        {
            starTrans[starCount].gameObject.SetActive(true);
            starCount++;
        }
        else
        {
            starCount = 0;
        }
    }
}
