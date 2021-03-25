using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridSystem
{
    [Header("Informations")]
    private int width;
    private int height;
    private int cellsize;
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
        Vector3 myCurrentCell;
        float textoffset = cellsize * 0.5f;

        for (int i = 0; i < gridArr.GetLength(0); i++)
        {
            for (int j = 0; j < gridArr.GetLength(1); j++)
            {
                ////// Retrieve a vector3 position of specified size at index xz in the gridArray
                myCurrentCell = Utilities.CellIndexToWorldPosition(i, j, origin, cellsize);
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
        /////// we set the value of the gridArr when adding an GridObject at index so we can later retrieve if the index = 1 | 0 
        gridArr[i_index, j_index] = value;
        textMeshPros[i_index, j_index].text = gridArr[i_index, j_index].ToString();
        /////// we first have to instanciate the gameobject holding the GridObject
        GridObject myGridObjectInstance = GameObject.Instantiate<GridObject>(selection, null);
        /////// the gridObject have to be updated so it can retrive the position of the index clicked by the mouse
        if (myGridObjectInstance != null)
            gridObjects[i_index, j_index] = myGridObjectInstance.UpdateGridObjectValues(Utilities.CellIndexToWorldPosition(i_index, j_index, origin, cellsize));
        return gridObjects[i_index, j_index];
    }

    public GridObject SetGridObjectOnMouseButtonDown(Vector3 worldPosition, GridObject selection, int value)
    {
        float x = 0, z = 0;
        Utilities.GetXY(worldPosition, ref x, ref z, cellsize, width, height);
        return SetGridObjectValue((int)x, (int)z, selection, value);
    }

    public Vector3 GetOrigin { get => origin; }
}
