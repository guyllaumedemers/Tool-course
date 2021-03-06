using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MyVector3
{
    private float x, y, z;
    public MyVector3(float myX, float myY, float myZ)
    {
        x = myX;
        y = myY;
        z = myZ;
    }
    public float GetX { get => x; set { x = value; } }
    public float GetY { get => y; set { y = value; } }
    public float GetZ { get => z; set { z = value; } }
}
[System.Serializable]
public struct MyColor
{
    private float x, y, z, w;
    public MyColor(float myX, float myY, float myZ, float myW)
    {
        x = myX;
        y = myY;
        z = myZ;
        w = myW;
    }

    public float GetX { get => x; set { x = value; } }
    public float GetY { get => y; set { y = value; } }
    public float GetZ { get => z; set { z = value; } }
    public float GetW { get => w; set { w = value; } }
}

[System.Serializable]
public class ShapeObjectDataInfo
{
    [Header("Object Information")]
    [SerializeField] private MyVector3 position;
    [SerializeField] private MyColor color;
    [SerializeField] private EnumMeshType meshType;

    public ShapeObjectDataInfo()
    {
        position = new MyVector3(0, 0, 0);
        color = new MyColor(1, 1, 1, 1);
        meshType = 0;
    }

    public MyVector3 GetPosition { get => position; set { position = value; } }

    public MyColor GetColor { get => color; set { color = value; } }

    public EnumMeshType GetEnumType { get => meshType; set { meshType = value; } }
}
