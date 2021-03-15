using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static T GetRandomElements<T>(this T[] myArr)
    {
        return myArr[UnityEngine.Random.Range(0, myArr.Length)];
    }

    public static string Concat<T>(this T[] myArr)
    {
        string myString = null;
        for (int i = 0; i < myArr.Length; i++)
        {
            myString += i != myArr.Length - 1 ? $"{myArr[i]}," : $"{myArr[i]}";
        }
        return myString;
    }

    public static int Factorial(this int value)
    {
        int result = 1;
        for (int i = 1; i <= value; i++)
        {
            result *= i;
        }
        return result;
    }

    public static Vector2 Rotate(this Vector2 myVec2)
    {

        return new Vector2();
    }

    public delegate bool PredicateDel<T>(T value);

    public static List<T> Predicate<T>(this T[] myArr, PredicateDel<T> predicate)
    {
        List<T> myList = new List<T>();
        for (int i = 0; i < myArr.Length; i++)
        {
            if (predicate(myArr[i]))
                myList.Add(myArr[i]);
        }
        return myList;
    }
}
