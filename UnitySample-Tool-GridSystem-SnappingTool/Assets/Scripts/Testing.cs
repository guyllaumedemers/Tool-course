using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public Transform plane;
    GridSystem gridSystem;

    void Start()
    {
        gridSystem = new GridSystem((int)plane.localScale.x, (int)plane.localScale.z, 10);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SetGridValueOnClick(Camera.main, gridSystem, 56);
    }

    private void SetGridValueOnClick(Camera camera, GridSystem gridSystem, int value)
    {
        if (camera == null || gridSystem == null || value < 0)
            return;
        var mouse = Input.mousePosition;
        mouse.z = 90;
        gridSystem.SetTextValueOnMouseButtonDown(Utilities.ScreenToWorld(camera, mouse), value);
    }
}
