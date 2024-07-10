using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    Dictionary<System.Type, UICanvas> canvasActives = new Dictionary<System.Type, UICanvas>();
    Dictionary<System.Type, UICanvas> canvasPrefabs = new Dictionary<System.Type, UICanvas>();
    [SerializeField] Transform parent;

    private void Awake()
    {
        // load UIPrefab tu Resourse
        UICanvas[] prefabs = Resources.LoadAll<UICanvas>("UI/");
        for (int i = 0; i < prefabs.Length; i++)
        {
            canvasPrefabs.Add(prefabs[i].GetType(), prefabs[i]);
        }
    }
    // mo canvas
    public T OpenUI<T>() where T : UICanvas
    {
        T canvas = GetUI<T>();
        canvas.SetUp();
        canvas.Open();
        return canvas;
    }

    // dong canvas sau time
    public void CloseUI<T>(float time) where T : UICanvas
    {
        if (IsLoaded<T>())
        {
            canvasActives[typeof(T)].Close(time);
        }
    }

    // dong canvas truc tiep
    public void CloseDirecly<T>() where T : UICanvas
    {
        canvasActives[typeof(T)].CloseDirecly();
    }

    // kiem tra canvas duoc tai chua
    public bool IsLoaded<T>() where T : UICanvas
    {
        return canvasActives.ContainsKey(typeof(T)) && canvasActives[typeof(T)] != null;
    }

    // kiem tra canvas duoc active chua
    public bool IsOpened<T>() where T : UICanvas    
    {
        return IsLoaded<T>() && canvasActives[typeof(T)].gameObject.activeSelf;
    }

    // lay active canvas
    public T GetUI<T>() where T : UICanvas
    {
        if (!IsLoaded<T>())
        {
            T prefab = GetUIPrefab<T>();
            T canvas = Instantiate(prefab, parent);
            canvasActives[typeof(T)] = canvas;
        }
        return canvasActives[typeof(T)] as T;
    }

    // get Prefabs
    T GetUIPrefab<T>() where T : UICanvas
    {
        return canvasPrefabs[typeof(T)] as T;
    }

    // dong tat ca
    public void CloseAll()
    {
        foreach(var canvas in canvasActives)
        {
            if (canvas.Value != null && canvas.Value.gameObject.activeSelf)
            {
                canvas.Value.Close(0);
            }
        }
    }
}


