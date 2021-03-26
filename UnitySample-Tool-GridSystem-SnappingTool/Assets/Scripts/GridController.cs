using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public GridObject gridObject;
    GridObject lastGridObject;
    public GridSystem gridSystem;
    public Transform plane;
    public int height;
    public int width;
    readonly int dir = 1;
    readonly int activeBuilding = 1;
    readonly int modelScale = 125;
    private HashSet<GridObject> instances;

    void Start()
    {
        gridSystem = new GridSystem(width, height, 10, new Vector3(-width * 0.5f, 0, -height * 0.5f));
        gridObject.ResetAnchor();
        lastGridObject = gridObject;
        instances = new HashSet<GridObject>();
    }

    private void Update()
    {
        ////// Reset the anchor point of the current gridObject selected
        if (lastGridObject != gridObject)
            ResetObjectSelected();
        ////// Rotate the building anchor position
        ////// -
        if (Input.GetKeyDown(KeyCode.LeftBracket))
            gridObject.UpdateAnchorRotation(-dir);
        ////// +
        if (Input.GetKeyDown(KeyCode.RightBracket))
            gridObject.UpdateAnchorRotation(dir);

        ////// Set Grid Object Scale
        ////// +
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            gridObject.UpdateAnchorScale(dir);
        ////// -
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            gridObject.UpdateAnchorScale(-dir);

        ////// Set a Numeric value at mouseposition clicked (TESTING PURPOSE)
        if (Input.GetMouseButtonDown(0))
            SetGridNumericValueOnClick(Camera.main, gridSystem, 56);
        ////// Set a Building at mouseposition clicked
        if (Input.GetMouseButtonDown(1))
            SetGridObjectOnClick(Camera.main, gridObject, gridSystem, activeBuilding);
    }

    private void SetGridNumericValueOnClick(Camera camera, GridSystem gridSystem, int value)
    {
        if (camera == null || gridSystem == null || value < 0)
            return;

        var mouse = Utilities.GetMouse3D(camera, Input.mousePosition);
        gridSystem.SetTextValueOnMouseButtonDown(Utilities.ScreenToWorldRayCast3D(camera, mouse), value);
    }

    private void SetGridObjectOnClick(Camera camera, GridObject gridObject, GridSystem gridSystem, int value)
    {
        if (camera == null || gridSystem == null || value < 0)
            return;
        var mouse = Utilities.GetMouse3D(camera, Input.mousePosition);

        GridObject returnedGridObject = gridSystem.SetGridObjectOnMouseButtonDown(Utilities.ScreenToWorldRayCast3D(camera, mouse), gridObject, value);
        if (returnedGridObject != null)
            AddBuilding(returnedGridObject);
    }

    private void ResetObjectSelected()
    {
        gridObject.ResetAnchor();
        lastGridObject = gridObject;
    }

    private void AddBuilding(GridObject gridObject)
    {
        instances.Add(gridObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Mesh myMesh = gridObject.GetComponentInChildren<MeshFilter>().sharedMesh;
        myMesh.RecalculateNormals();
        Gizmos.DrawMesh(myMesh, Utilities.ScreenToWorldRayCast3D(Camera.main, Input.mousePosition), gridObject.anchor.rotation, gridObject.anchor.localScale * modelScale);
    }
}
