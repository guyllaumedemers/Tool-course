using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    ////// Gameobject prefab to display the selected building
    public GameObject prefab;
    ////// Transform to assign a position for the anchor of the building when MouseButtonDown
    public Transform anchor;

    public void UpdateAnchorRotation(float direction)
    {
        anchor.eulerAngles += new Vector3(0, 90, 0) * direction;
    }

    public Vector3 UpdateAnchorPosition(Vector3 worldPosition, Vector3 origin, float cellSize, float width, float height)
    {
        float x = 0, z = 0;
        Utilities.GetXY(worldPosition, ref x, ref z, cellSize, width, height);
        ////// have to take into acccount the different rotation angle and the position that we have to set accordingly
        return GetAnchorRotationResult(origin, x, z, anchor.eulerAngles.y, cellSize);
    }

    public void UpdateAnchorScale(float direction)
    {
        Vector3 result = anchor.localScale + new Vector3(1, 1, 1) * direction;
        if (result != Vector3.zero)
            anchor.localScale = result;
    }

    /// <summary>
    /// Theres is some inconsistency after a number of instance on the grid
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="angle"></param>
    /// <param name="cellsize"></param>
    /// <returns></returns>
    public Vector3 GetAnchorRotationResult(Vector3 origin, float x, float z, float angle, float cellsize)
    {
        if (angle == 90 || angle == -270)
            return Utilities.CellIndexToWorldPosition(origin, x, z + 1, cellsize);
        else if (angle == 270 || angle == -90)
            return Utilities.CellIndexToWorldPosition(origin, x + 1, z, cellsize);
        else if (angle == 180 || angle == -180)
            return Utilities.CellIndexToWorldPosition(origin, x + 1, z + 1, cellsize);
        else
            return Utilities.CellIndexToWorldPosition(origin, x, z, cellsize);
    }

    public void ResetAnchor()
    {
        anchor.rotation = Quaternion.Euler(0, 0, 0);
        anchor.position = Vector3.zero;
        anchor.localScale = Vector3.one;
    }

    public GridObject UpdateGridObjectValues(Vector3 worldPosition, Vector3 origin, float cellSize, float width, float height)
    {
        anchor.position = UpdateAnchorPosition(worldPosition, origin, cellSize, width, height);
        return this;
    }
}
