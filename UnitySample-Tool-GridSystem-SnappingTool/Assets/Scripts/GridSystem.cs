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

    public GridSystem(int myWidth, int myHeight, int sizeCell)
    {
        width = myWidth;
        height = myHeight;
        cellsize = sizeCell;
        gridArr = new int[myWidth, myHeight];
        textMeshPros = new TextMeshPro[myWidth, myHeight];

        Vector3 origin = new Vector3(-width * 0.5f, 0, -height * 0.5f) * cellsize;
        Vector3 myCurrentCell;
        float textoffset = cellsize * 0.5f;

        for (int i = 0; i < gridArr.GetLength(0); i++)
        {
            for (int j = 0; j < gridArr.GetLength(1); j++)
            {
                ////// Retrieve a vector3 position of specified size at index xz in the gridArray
                myCurrentCell = Utilities.SetCellPosition(i, j, origin, cellsize);
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

    private void SetTextValue(int i_index, int j_index, int value)
    {
        if (i_index < 0 || i_index > width || j_index < 0 || j_index > height)
            return;
        gridArr[i_index, j_index] = value;
        textMeshPros[i_index, j_index].text = gridArr[i_index, j_index].ToString();
    }

    private void GetXY(Vector3 worldPosition, ref float x, ref float z)
    {
        ////// do not forget to take into account that the world position (0,0) is offset to set the grid, so pressing at 0,0 actually update its -width/2, -height/2 value
        x = Utilities.WorldToCellIndex(worldPosition, cellsize).x + width / 2;
        z = Utilities.WorldToCellIndex(worldPosition, cellsize).z + height / 2;
    }

    public void SetTextValueOnMouseButtonDown(Vector3 worldPosition, int value)
    {
        ////// convert world position to single cell, so world position divided by the cell size give us the index
        ////// keep in mind that we have to offset the index values BECAUSE in order to align the grid with the plane we had to offset the position of the TextMesh
        float x = 0, z = 0;
        GetXY(worldPosition, ref x, ref z);
        SetTextValue((int)x, (int)z, value);
    }


}
