using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeObject : MonoBehaviour
{

    [Header("Serialize Data")]
    private ShapeObjectDataInfo dataInfo;

    private void Awake()
    {
        dataInfo = new ShapeObjectDataInfo();
    }

    public MyVector3 ToCustomVector(Vector3 pos)
    {
        MyVector3 myVector3 = new MyVector3();
        myVector3.GetX = pos.x;
        myVector3.GetY = pos.y;
        myVector3.GetZ = pos.z;

        return myVector3;
    }

    public MyColor ToCustomColor(Color color)
    {
        MyColor myColor = new MyColor();
        myColor.GetX = color.r;
        myColor.GetY = color.g;
        myColor.GetZ = color.b;
        myColor.GetW = color.a;

        return myColor;
    }

    public ShapeObjectDataInfo GetDataInfo { get => dataInfo; set { dataInfo = value; } }
}