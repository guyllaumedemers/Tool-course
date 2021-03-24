using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class Utilities
{
    public static Vector3 ScreenToWorld2D(Camera camera, Vector3 screenPos)
    {
        return camera.ScreenToWorldPoint(screenPos);
    }

    public static Vector3 ScreenToWorldRayCast3D(Camera camera, Vector3 screenPos)
    {
        if (Physics.Raycast(camera.ScreenPointToRay(screenPos), out RaycastHit rc))
        {
            return rc.point;
        }
        return new Vector3();
    }

    public static Vector3 CellIndexToWorldPosition(int i_index, int j_index, Vector3 origin, int scale)
    {
        return new Vector3(i_index, 0, j_index) * scale + origin;
    }

    public static Vector3 WorldPositionToCellIndex(Vector3 worldPos, int scale)
    {
        return worldPos / scale;
    }

    /// <summary>
    /// Create a Text Cell that is flat on the surface
    /// We return the TextMeshPro and store it so we have a reference to it and can update its value when selected 
    /// </summary>
    /// <param name="worldPos"></param>
    /// <param name="textsize"></param>
    /// <returns></returns>
    public static TextMeshPro CreateTextCell(Vector3 worldPos, int textvalue, int textsize, float textOffset)
    {
        GameObject gameObject = new GameObject();
        gameObject.transform.position = worldPos + new Vector3(0, 0.1f, 0);
        gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
        TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
        textMeshPro.transform.position += new Vector3(textOffset, 0, textOffset);
        textMeshPro.color = Color.white;
        textMeshPro.fontSize = textsize;
        textMeshPro.alignment = TextAlignmentOptions.Midline;
        textMeshPro.text = textvalue.ToString();
        return textMeshPro;
    }
}
