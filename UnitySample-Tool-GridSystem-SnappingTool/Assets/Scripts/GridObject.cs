using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    ////// Gameobject prefab to display the selected building
    public GameObject prefab;
    ////// Transform to assign a position for the anchor of the building when MouseButtonDown
    public Transform anchor;

    public void UpdateAnchorRotation(int direction, int width, int height)
    {
        anchor.eulerAngles += new Vector3(0, 90, 0) * direction;
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
