using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MyVector3
{
    public MyVector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public float X { get; }
    public float Y { get; }
    public float Z { get; }
}
[System.Serializable]
public struct MyColor
{
    public MyColor(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public float X { get; }
    public float Y { get; }
    public float Z { get; }
    public float W { get; }
}

[System.Serializable]
public class ShapeObjectDataInfo
{
    [Header("Object Information")]
    [SerializeField] private MyVector3 position;
    [SerializeField] private MyColor color;

    public ShapeObjectDataInfo()
    {
        position = new MyVector3(0, 0, 0);
        color = new MyColor(1, 1, 1, 1);
    }

    public MyVector3 GetPosition { get => position; set { position = value; } }

    public MyColor GetColor { get => color; set { color = value; } }
}
