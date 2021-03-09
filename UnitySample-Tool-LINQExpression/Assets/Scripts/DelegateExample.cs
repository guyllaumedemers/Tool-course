using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateExample : MonoBehaviour
{
    private void Awake()
    {
        Timer.Instance.AddListener((Timer) => { Debug.Log($"Calling a function from the EventSystem Example + Time : {Timer.GetTime}"); });
    }


}
