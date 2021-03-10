using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ExampleClass : MonoBehaviour
{
    Stopwatch stopwatch = new Stopwatch();

    private void Awake()
    {
        Timer.Instance.AddTimerListener(() => { stopwatch.Start(); UnityEngine.Debug.Log($"START TIME : {stopwatch.ElapsedMilliseconds / 1000}"); });
        Timer.Instance.AddTimerListener(() => { TimeElapse(3000); });
        Timer.Instance.AddTimerListener(() => { stopwatch.Stop(); EndTime(5000); });
    }

    private async void TimeElapse(int timeMs)
    {
        await Task.Run(() => { Thread.Sleep(timeMs); UnityEngine.Debug.Log($"TIME NOW : {stopwatch.ElapsedMilliseconds / 1000}"); });
    }

    private async void EndTime(int timeMs)
    {
        await Task.Run(() => { Thread.Sleep(timeMs); UnityEngine.Debug.Log($"END TIME : {stopwatch.ElapsedMilliseconds / 1000}"); });
    }
}
