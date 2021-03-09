using System.Collections;
using System.Collections.Generic;
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
                instance = FindObjectOfType<Timer>();
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.AddComponent<Timer>();
                    instance = go.GetComponent<Timer>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    public delegate void OnTimerDelegate(Timer timer);

    private OnTimerDelegate onTimerDelegate;

    private float time = 2;

    private void Awake()
    {
        onTimerDelegate += (Timer) => { Debug.Log($"Calling a function from the Timer Class + Time : {Timer.GetTime}"); };
        StartCoroutine(LaunchDelegate());
    }

    private void Start()
    {
        //try
        //{
        //    onTimerDelegate?.Invoke(this);
        //}
        //catch (System.Exception e)
        //{
        //    Debug.Log($"Exception : {e.Message}");
        //}
    }

    public void AddListener(OnTimerDelegate listener)
    {
        onTimerDelegate += listener;
    }

    private IEnumerator LaunchDelegate()
    {
        yield return new WaitForSeconds(time);
        try
        {
            onTimerDelegate?.Invoke(this);
        }
        catch (System.Exception e)
        {
            Debug.Log($"Exception : {e.Message}");
        }
    }

    public float GetTime { get => time; }
}
