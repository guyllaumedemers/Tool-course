using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public GridObject gridObject;
    public Transform plane;
    GridSystem gridSystem;
    int height;
    int width;

    void Start()
    {
        height = (int)plane.localScale.z;
        width = (int)plane.localScale.x;
        gridSystem = new GridSystem(width, height, 10, new Vector3(-width * 0.5f, 0, -height * 0.5f));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SetGridValueOnClick(Camera.main, gridSystem, 56);
        if (Input.GetMouseButtonDown(1))
            SetGridObjectOnClick(Camera.main, gridSystem, 1);
    }

    private void SetGridValueOnClick(Camera camera, GridSystem gridSystem, int value)
    {
        if (camera == null || gridSystem == null || value < 0)
            return;
        var mouse = Input.mousePosition;
        mouse.z = 90;
        gridSystem.SetTextValueOnMouseButtonDown(Utilities.ScreenToWorld(camera, mouse), value);
    }

    private void SetGridObjectOnClick(Camera camera, GridSystem gridSystem, int value)
    {
        if (camera == null || gridSystem == null || value < 0)
            return;
        var mouse = Input.mousePosition;
        mouse.z = 90;
        gridSystem.SetGridObjectOnMouseButtonDown(Utilities.ScreenToWorld(camera, mouse), gridObject, value);
    }
}
