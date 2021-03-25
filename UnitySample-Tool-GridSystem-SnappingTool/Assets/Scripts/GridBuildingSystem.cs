using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    public GridObject gridObject;
    public GridSystem gridSystem;
    public Transform plane;
    public int height;
    public int width;
    private HashSet<GridObject> instances;

    void Start()
    {
        gridSystem = new GridSystem(width, height, 10, new Vector3(-width * 0.5f, 0, -height * 0.5f));
        instances = new HashSet<GridObject>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SetGridValueOnClick(Camera.main, gridSystem, 56);
        if (Input.GetMouseButtonDown(1))
            SetGridObjectOnClick(Camera.main, gridObject, gridSystem, 1);
    }

    private void SetGridValueOnClick(Camera camera, GridSystem gridSystem, int value)
    {
        if (camera == null || gridSystem == null || value < 0)
            return;
        var mouse = Input.mousePosition;
        mouse.z = camera.transform.position.y;
        gridSystem.SetTextValueOnMouseButtonDown(Utilities.ScreenToWorldRayCast3D(camera, mouse), value);
    }

    private void SetGridObjectOnClick(Camera camera, GridObject gridObject, GridSystem gridSystem, int value)
    {
        if (camera == null || gridSystem == null || value < 0)
            return;
        var mouse = Input.mousePosition;
        mouse.z = camera.transform.position.y;
        GridObject returnedGridObject = gridSystem.SetGridObjectOnMouseButtonDown(Utilities.ScreenToWorldRayCast3D(camera, mouse), gridObject, value);
        if (returnedGridObject != null)
            instances.Add(returnedGridObject);
    }
}
