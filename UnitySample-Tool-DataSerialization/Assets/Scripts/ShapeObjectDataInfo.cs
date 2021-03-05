using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeObjectDataInfo
{
    [Header("Object Information")]
    private Vector3 position;
    private Color color;

    public ShapeObjectDataInfo()
    {
        position = Vector3.zero;
        color = Color.white;
    }

    public Vector3 GetPosition { get => position; set { position = value; } }

    public Color GetColor { get => color; set { color = value; } }
}
