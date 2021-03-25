using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    ////// Gameobject prefab to display the selected building
    public GameObject prefab;
    ////// Transform to assign a position for the anchor of the building when MouseButtonDown
    public Transform anchor;

    public void UpdateAnchorRotation(Vector3 position, int cellSize, int width, int height, int direction)
    {
        anchor.eulerAngles += new Vector3(0, 90, 0) * direction;
        ///// need to adapt the position of the building after rotation
        //anchor.transform.position = UpdateAnchorPosition(position, cellSize, width, height);
    }

    public Vector3 UpdateAnchorPosition(Vector3 position, int cellSize, int width, int height)
    {
        float x = 0, z = 0;
        Utilities.GetXY(position, ref x, ref z, cellSize, width, height);
        return new Vector3();
    }

    public void ResetAnchor()
    {
        anchor.rotation = Quaternion.Euler(0, 0, 0);
    }

    public GridObject UpdateGridObjectValues(Vector3 worldPosition)
    {
        anchor.position = worldPosition;
        return this;
    }
}
