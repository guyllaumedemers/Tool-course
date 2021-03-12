using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CompactSingleRow
{
    public static Rect[] GetSubdivision(Rect myRect)
    {
        Rect left_Rect = new Rect(myRect.x, myRect.y, myRect.width / 2, myRect.height);
        Rect right_Rect = new Rect(myRect.x + myRect.width / 2, myRect.y, myRect.width / 2, myRect.height);
        return new Rect[] { left_Rect, right_Rect };
    }

    public static Rect[] GetSubdivisionWithPrefixe(Rect myRect)
    {
        Rect left_Rect = new Rect(myRect.x, myRect.y, myRect.width / 3, myRect.height);
        Rect middle_Rect = new Rect(myRect.x + myRect.width / 3, myRect.y, myRect.width / 3, myRect.height);
        Rect right_Rect = new Rect(myRect.x + ((myRect.width / 3) * 2), myRect.y, myRect.width / 3, myRect.height);
        return new Rect[] { left_Rect, middle_Rect, right_Rect };
    }

    public static Rect[,] GetGrid(int row, int col, Vector2 pos, float width, float height)
    {
        Rect[,] rects = new Rect[row, col];
        float minX = pos.x;

        //// We loop over the number of row first
        for (int i = 0; i < row; i++)
        {
            //// We loop over the number of col
            for (int j = 0; j < col; j++)
            {
                if (pos.x >= width)
                    pos.x = minX;
                //// We generate a new Rect for each index ij of the 2d Array
                //// We calculate the cell size for each columns forming a row (width / col)
                //// We calculate the cell size for each row (height / row)
                rects[i, j] = new Rect(pos.x, pos.y, width / col, height / row);
                pos.x += width / col;
            }
            pos.y += height / row;
        }
        return rects;
    }

    public static float[] UnpackVector2(Vector2 values)
    {
        return new float[] { Mathf.Round(values.x), Mathf.Round(values.y) };
    }
}
