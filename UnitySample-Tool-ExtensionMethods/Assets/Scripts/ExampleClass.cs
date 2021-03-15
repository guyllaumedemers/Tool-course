using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleClass : MonoBehaviour
{
    string[] strings = { "Hello", "Butter", "Peanut", "Foodoodle" };
    int[] intArr = { 1, 7, 6, 4, 10 };
    int myInt = 5;
    Vector2 init = new Vector2(1, 0);
    readonly int angle = 90;

    private void Awake()
    {
        //Debug.Log(strings.GetRandomElements());
        //Debug.Log(intArr.Concat());
        //Debug.Log(myInt.Factorial());
        Debug.Log(init.Rotate(angle));
        //List<int> myList = intArr.Predicate((a) => a != 4);
        //foreach (int i in myList)
        //{
        //    Debug.Log(i);
        //}
    }
}
