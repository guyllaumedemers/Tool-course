using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ExampleClass : MonoBehaviour
{
    Stopwatch stopwatch;
    private void Start()
    {
        stopwatch = new Stopwatch();
        //Timer.Instance.AddTimerListener(() => { ResetTimer(); });
        Timer.Instance.AddTimerListener(() => { StartTime(); });
        Timer.Instance.AddTimerListener(() => { TimeElapse(3000); });
        Timer.Instance.AddTimerListener(() => { EndTime(5000); });
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