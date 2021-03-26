using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridSystem
{
    [Header("Informations")]
    private float width;
    private float height;
    private float cellsize;
    private int[,] gridArr;
    private TextMeshPro[,] textMeshPros;
    private GridObject[,] gridObjects;
    private Vector3 origin;

    public GridSystem(int myWidth, int myHeight, int sizeCell, Vector3 myOrigin)
    {
        height = myHeight;
        width = myWidth;
        cellsize = sizeCell;
        gridArr = new int[myWidth, myHeight];
        gridObjects = new GridObject[myWidth, myHeight];
        textMeshPros = new TextMeshPro[myWidth, myHeight];
        origin = myOrigin * cellsize;
        float textoffset = cellsize * 0.5f;

        for (int i = 0; i < gridArr.GetLength(0); i++)
        {
            for (int j = 0; j < gridArr.GetLength(1); j++)
            {
                ////// Retrieve a vector3 position of specified size at index xz in the gridArray
                Vector3 myCurrentCell = Utilities.CellIndexToWorldPosition(origin, i, j, cellsize);
                ////// Create a TextCell at cell position and return the MeshPro with value at gridArray index
                textMeshPros[i, j] = Utilities.CreateTextCell(myCurrentCell, gridArr[i, j], 24, textoffset);

                Debug.DrawLine(myCurrentCell, myCurrentCell + new Vector3(1, 0, 0) * cellsize, Color.red, 100f);
                Debug.DrawLine(myCurrentCell, myCurrentCell + new Vector3(0, 0, 1) * cellsize, Color.red, 100f);
            }
        }
        ///// Take into account the offset of the grid since we do not start at origin(0,0) but instead at the bottom left corner
        Debug.DrawLine(new Vector3(-width / 2, 0, height / 2) * cellsize, new Vector3(width / 2, 0, height / 2) * cellsize, Color.red, 100f);
        Debug.DrawLine(new Vector3(width / 2, 0, -height / 2) * cellsize, new Vector3(width / 2, 0, height / 2) * cellsize, Color.red, 100f);
    }

    #region GRID TESTING WITH NUMERIC VALUES
    private void SetTextValue(int i_index, int j_index, int value)
    {
        if (i_index < 0 || i_index > width || j_index < 0 || j_index > height)
            return;
        gridArr[i_index, j_index] = value;
        textMeshPros[i_index, j_index].text = gridArr[i_index, j_index].ToString();
    }

    public void SetTextValueOnMouseButtonDown(Vector3 worldPosition, int value)
    {
        ////// convert world position to single cell, so world position divided by the cell size give us the index
        ////// keep in mind that we have to offset the index values BECAUSE in order to align the grid with the plane we had to offset the position of the TextMesh
        float x = 0, z = 0;
        Utilities.GetXY(worldPosition, ref x, ref z, cellsize, width, height);
        SetTextValue((int)x, (int)z, value);
    }
    #endregion

    private GridObject SetGridObjectValue(int i_index, int j_index, GridObject selection, int value)
    {
        if (i_index < 0 || i_index > width || j_index < 0 || j_index > height || gridArr[i_index, j_index] != 0)
            return null;
        int result = 0;
        /////// we set the value of the gridArr when adding an GridObject at index so we can later retrieve if the index = 1 | 0 
        if (selection.anchor.localScale != Vector3.one)
            result = SetMultipleIndexValues(i_index, j_index, selection.anchor.localScale, value);
        else
            SetSingleIndexValue(i_index, j_index, value);

        /////// check condition when verifying all neighbors from when the scale of the grid object is bigger than 1
        if (result != 0)
            return null;
        /////// we first have to instanciate the gameobject holding the GridObject
        GridObject myGridObjectInstance = GameObject.Instantiate<GridObject>(selection);

        /////// the gridObject have to be updated so it can retrive the position of the index clicked by the mouse
        Vector3 cellIndexToWorldPos = Utilities.CellIndexToWorldPosition(origin, i_index, j_index, cellsize);

        if (myGridObjectInstance != null)
            gridObjects[i_index, j_index] = myGridObjectInstance.UpdateGridObjectValues(cellIndexToWorldPos, origin, cellsize, width, height);
        return gridObjects[i_index, j_index];
    }

    public GridObject SetGridObjectOnMouseButtonDown(Vector3 worldPosition, GridObject selection, int value)
    {
        float x = 0, z = 0;
        Utilities.GetXY(worldPosition, ref x, ref z, cellsize, width, height);
        return SetGridObjectValue((int)x, (int)z, selection, value);
    }

    private void SetSingleIndexValue(int i_index, int j_index, int value)
    {
        gridArr[i_index, j_index] = value;
        textMeshPros[i_index, j_index].text = gridArr[i_index, j_index].ToString();
    }

    private int SetMultipleIndexValues(int i_index, int j_index, Vector3 nextPos, int value)
    {
        int result = CheckIndexNeighbors(i_index, j_index, nextPos);
        if (result != 0)
            return -1;
        else
            AssignValuesToNeighbors(i_index, j_index, nextPos, value);
        return 0;
    }

    private int CheckIndexNeighbors(int i_index, int j_index, Vector3 nextPos)
    {
        for (int i = 0; i < nextPos.x; i++)
        {
            for (int j = 0; j < nextPos.z; j++)
            {
                if (gridArr[i_index + i, j_index + j] != 0)
                    return -1;
            }
        }
        return 0;
    }

    private void AssignValuesToNeighbors(int i_index, int j_index, Vector3 nextPos, int value)
    {
        for (int i = 0; i < nextPos.x; i++)
        {
            for (int j = 0; j < nextPos.z; j++)
            {
                gridArr[i_index + i, j_index + j] = value;
                textMeshPros[i_index + i, j_index + j].text = gridArr[i_index + i, j_index + j].ToString();
            }
        }
    }

    public float GetCellSize { get => cellsize; }
}
