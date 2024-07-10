using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushShopData : MonoBehaviour
{
    [SerializeField] SaveShopData saveShopData;
    [SerializeField] bool push;
    [SerializeField] List<EPooling> item;

    void Start()
    {
        push = false;
    }

    void Update()
    {
        if (push)
        {
            saveShopData.PushItem(item);
            push = false;
        }
    }
}
