using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
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
    public event OnTimerDel onTimerEvent;

    private bool isInit = false;

    Stopwatch stopwatch;


    private void Awake()
    {
        onTimerEvent = null;
        stopwatch = new Stopwatch();
        //Timer.Instance.AddTimerListener(() => { ResetTimer(); });
        AddTimerListener(() => { StartTime(); });
        AddTimerListener(() => { TimeElapse(3000); });
        AddTimerListener(() => { EndTime(5000); });
    }

    private void Update()
    {
        try
        {
            //// Invoking inside the update to get pass the delay between the start method call and the update method call
            if (!isInit)
                onTimerEvent?.Invoke(); isInit = true;
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.Log($"Exception : {e.Message}");
        }
    }

    public void AddTimerListener(OnTimerDel myFunc)
    {
        onTimerEvent += myFunc;
    }

    private void ResetTimer()
    {
        //// not async function since we want it to delay the callback process so we are sure the timer is reset before the other functions
        ///  gets processed on the other threads
        stopwatch.Reset();
    }

    private async void StartTime()
    {
        stopwatch.Start();
        await Task.Run(() => { UnityEngine.Debug.Log($"START TIME : {stopwatch.ElapsedMilliseconds / 1000}"); });
    }

    private async void TimeElapse(int timeMs)
    {
        await Task.Run(() => { Thread.Sleep(timeMs); UnityEngine.Debug.Log($"TIME NOW : {stopwatch.ElapsedMilliseconds / 1000}"); });
    }

    private async void EndTime(int timeMs)
    {
        await Task.Run(() => { Thread.Sleep(timeMs); UnityEngine.Debug.Log($"END TIME : {stopwatch.ElapsedMilliseconds / 1000}"); });
        stopwatch.Stop();
    }
}
