using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private static Timer instance;

    public static Timer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Timer>();
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    instance = go.AddComponent<Timer>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    public delegate void OnTimerDel();
    public event OnTimerDel OnTimerEvent;

    private bool isInit = false;

    private void Update()
    {
        try
        {
            //// Invoking inside the update to get pass the delay between the start method call and the update method call
            if (!isInit)
                OnTimerEvent?.Invoke(); isInit = true;
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.Log($"Exception : {e.Message}");
        }
    }

    public void AddTimerListener(OnTimerDel myFunc)
    {
        OnTimerEvent += myFunc;
    }
}
