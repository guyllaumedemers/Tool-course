using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class Utilities
{
    /// <summary>
    /// Get WorldPos from ScreenPos (ONLY Works in 2D Space)
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="screenPos"></param>
    /// <returns></returns>
    public static Vector3 ScreenToWorld2D(Camera camera, Vector3 screenPos)
    {
        return camera.ScreenToWorldPoint(screenPos);
    }

    /// <summary>
    /// Get WorldPos from ScreenPos via Raycast (Use this when working in a 3D Space)
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="screenPos"></param>
    /// <returns></returns>
    public static Vector3 ScreenToWorldRayCast3D(Camera camera, Vector3 screenPos)
    {
        if (Physics.Raycast(camera.ScreenPointToRay(screenPos), out RaycastHit rc))
        {
            return rc.point;
        }
        return new Vector3();
    }

    /// <summary>
    /// Get MousePos in 3D Space
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="inputMouse"></param>
    /// <returns></returns>
    public static Vector3 GetMouse3D(Camera camera, Vector3 inputMouse)
    {
        var mouse = inputMouse;
        ///// the z position have to be set otherwise the mouse position is always going to return the camera position and not the position clicked
        mouse.z = camera.transform.position.y;
        return mouse;
    }

    /// <summary>
    /// Get Index in Grid at WorldPos selected
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="cellsize"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void GetXY(Vector3 worldPosition, ref float x, ref float z, float cellsize, float width, float height)
    {
        ////// do not forget to take into account that the world position (0,0) is offset to set the grid, so pressing at 0,0 actually update its -width/2, -height/2 value
        x = Utilities.WorldPositionToCellIndex(worldPosition, cellsize).x + width / 2;
        z = Utilities.WorldPositionToCellIndex(worldPosition, cellsize).z + height / 2;
    }
    /// <summary>
    /// Works In parallel with the above function (KEEP PRIVATE)
    /// </summary>
    /// <param name="worldPos"></param>
    /// <param name="cellsize"></param>
    /// <returns></returns>
    private static Vector3 WorldPositionToCellIndex(Vector3 worldPos, float cellsize)
    {
        return worldPos / cellsize;
    }

    /// <summary>
    /// Get WorldPos from Index selected
    /// </summary>
    /// <param name="i_index"></param>
    /// <param name="j_index"></param>
    /// <param name="origin"></param>
    /// <param name="cellsize"></param>
    /// <returns></returns>
    public static Vector3 CellIndexToWorldPosition(Vector3 origin, float i_index, float j_index, float cellsize)
    {
        ////// dont forget to take into account the origin of the grid since its (0,0) value is moved at instanciation
        return new Vector3(i_index, 0, j_index) * cellsize + origin;
    }

    /// <summary>
    /// Create text cell on Plane
    /// We return the TextMeshPro and store it so we have a reference to it and can update its value when selected 
    /// </summary>
    /// <param name="worldPos"></param>
    /// <param name="textsize"></param>
    /// <returns></returns>
    public static TextMeshPro CreateTextCell(Vector3 worldPos, float textvalue, float textsize, float textOffset)
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
