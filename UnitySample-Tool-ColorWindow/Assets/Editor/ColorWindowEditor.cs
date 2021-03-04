using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  //Required for MenuItem, means that this is an Editor script, must be placed in an Editor folder, and cannot be compiled!
using System.Linq;  //Used for Select
using System;

public class ColorWindow : EditorWindow
{ //Now is of type EditorWindow

    [MenuItem("Custom Tools/ Color Window")] //This the function below it as a menu item, which appears in the tool bar
    public static void CreateShowcase() //Menu items can call STATIC functions, does not work for non-static since Editor scripts cannot be attached to objects
    {
        EditorWindow window = GetWindow<ColorWindow>("Color Window");
    }

    private Color[,] colors;
    private int width = 8;
    private int height = 8;
    Texture colorTexture;
    Renderer textureTarget;

    [Header("Color Picker")]
    private Color selectedColor = Color.white;
    private Color eraseColor = Color.white;
    private Vector4 myRandoFactor;

    public delegate void MyDelegate(Color color);
    [Header("Delegates")]
    public MyDelegate myDelegate;

    [Header("Editor Fields Constants")]
    private readonly float MAX_RGBA_VALUE = 1.0f;
    private readonly float MIN_RGBA_VALUE = 0.0f;
    private readonly float MIN_RANDOM_VALUE = -0.01f;
    private readonly float MAX_RANDOM_VALUE = 0.01f;
    private readonly int NUMBER_DECIMAL = 2;
    private readonly float CLAMP_OFFSET = 0.01f;

    [Header("NEIGHBOR VARIABLES")]
    private readonly int UNIT_NEIGHBOR = 1;
    private readonly int ZERO_POSITION = 0;

    public void OnEnable()
    {
        colors = new Color[width, height];
        for (int i = 0; i < colors.GetLength(0); i++)
            for (int j = 0; j < colors.GetLength(1); j++)
                colors[i, j] = GetRandomColor();
        colorTexture = EditorGUIUtility.whiteTexture;
        myDelegate += OnSelectionChanged;
    }

    public Color GetColor
    {
        get
        {
            myDelegate.Invoke(selectedColor);
            return selectedColor;
        }
    }

    private Color GetRandomColor()  //Built a get random color tool
    {
        return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1f);
    }

    /// <summary>
    /// Could have use Mathf.Clamp but I like mine better
    /// </summary>
    private void GetEditorFieldsForSelectedColor()
    {
        GetEditorFieldsClampedColor();
        GetEditorFieldsForRandomFactor();
    }

    private void GetEditorFieldsClampedColor()
    {
        UpdateClampValues();
    }

    private void GetEditorFieldsForRandomFactor()
    {
        GUILayout.Space(20.0f);
        EditorGUILayout.FloatField("Rnd_R", (float)Math.Round(myRandoFactor.x, NUMBER_DECIMAL));
        EditorGUILayout.FloatField("Rnd_G", (float)Math.Round(myRandoFactor.y, NUMBER_DECIMAL));
        EditorGUILayout.FloatField("Rnd_B", (float)Math.Round(myRandoFactor.z, NUMBER_DECIMAL));
        EditorGUILayout.FloatField("Rnd_A", (float)Math.Round(myRandoFactor.w, NUMBER_DECIMAL));
    }

    private void OnSelectionChanged(Color color)
    {
        //myRandoFactor = color; // => use only if we want to display the value before the random factor is applied
        color.r += UnityEngine.Random.Range(MIN_RANDOM_VALUE, MAX_RANDOM_VALUE);
        color.g += UnityEngine.Random.Range(MIN_RANDOM_VALUE, MAX_RANDOM_VALUE);
        color.b += UnityEngine.Random.Range(MIN_RANDOM_VALUE, MAX_RANDOM_VALUE);
        myRandoFactor = color - selectedColor; // is that wonky?
        selectedColor = color;
    }

    private void UpdateClampValues()
    {
        float clampR = selectedColor.r < MIN_RGBA_VALUE ? MIN_RGBA_VALUE : selectedColor.r % MAX_RGBA_VALUE;
        float clampG = selectedColor.g < MIN_RGBA_VALUE ? MIN_RGBA_VALUE : selectedColor.g % MAX_RGBA_VALUE;
        float clampB = selectedColor.b < MIN_RGBA_VALUE ? MIN_RGBA_VALUE : selectedColor.b % MAX_RGBA_VALUE;
        float clampA = selectedColor.a < MIN_RGBA_VALUE ? MIN_RGBA_VALUE : selectedColor.a % MAX_RGBA_VALUE;
        selectedColor.r = EditorGUILayout.FloatField(new GUIContent("R"), selectedColor.r > MAX_RGBA_VALUE - CLAMP_OFFSET ? MAX_RGBA_VALUE : (float)Math.Round(clampR, NUMBER_DECIMAL));
        selectedColor.g = EditorGUILayout.FloatField(new GUIContent("G"), selectedColor.g > MAX_RGBA_VALUE - CLAMP_OFFSET ? MAX_RGBA_VALUE : (float)Math.Round(clampG, NUMBER_DECIMAL));
        selectedColor.b = EditorGUILayout.FloatField(new GUIContent("B"), selectedColor.b > MAX_RGBA_VALUE - CLAMP_OFFSET ? MAX_RGBA_VALUE : (float)Math.Round(clampB, NUMBER_DECIMAL));
        selectedColor.a = EditorGUILayout.FloatField(new GUIContent("A"), selectedColor.a > MAX_RGBA_VALUE - CLAMP_OFFSET ? MAX_RGBA_VALUE : (float)Math.Round(clampA, NUMBER_DECIMAL));
    }

    void OnGUI() //Called every frame in Editor window
    {
        GUILayout.BeginHorizontal();        //Have each element below be side by side
        DoControls();
        DoCanvas();
        GUILayout.EndHorizontal();
    }

    void DoControls()
    {
        GUILayout.BeginVertical();                                                      //Start vertical section, all GUI draw code after this will belong to same vertical
        GUILayout.Label("ToolBar", EditorStyles.largeLabel);                            //A label that says "Toolbar"
        selectedColor = EditorGUILayout.ColorField("Paint Color", selectedColor);       //Make a color field with the text "Paint Color" and have it fill the selectedColor var
        eraseColor = EditorGUILayout.ColorField("Erase Color", eraseColor);             //Make a color field with the text "Erase Color"
        if (GUILayout.Button("Fill All"))                                               //A button, if pressed, returns true
            FillMatrix();                                                                  //colors = colors.Select(c => c = selectedColor).ToArray();                   //Linq expresion, for every color in the color array, sets it to the selected color

        GetEditorFieldsForSelectedColor();

        GUILayout.FlexibleSpace();                                                      //Flexible space uses any left over space in the loadout
        textureTarget = EditorGUILayout.ObjectField("Output Renderer", textureTarget, typeof(Renderer), true) as Renderer;  //Build an object field that accepts a renderer

        if (GUILayout.Button("Save to Object"))
        {
            Texture2D t2d = new Texture2D(width, height);                               //Create a new texture
            t2d.filterMode = FilterMode.Point;                                          //Simplest non-blend texture mode
            textureTarget.material = new Material(Shader.Find("Diffuse"));              //Materials require Shaders as an arguement, Diffuse is the most basic type
            textureTarget.sharedMaterial.mainTexture = t2d;                             //sharedMaterial is the MAIN RESOURCE MATERIAL. Changing this will change ALL objects using it, .material will give you the local instance

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    t2d.SetPixel(i, height - 1 - j, colors[i, j]);                     //Color every pixel using our color table, the texture is 8x8 pixels large, but strecthes to fit
                }
            }
            t2d.Apply();                                                                //Apply all changes to texture
        }
        GUILayout.EndVertical();                                                        //end vertical section
    }

    void DoCanvas()
    {
        Event evt = Event.current;                     //Grab the current event

        Color oldColor = GUI.color;                    //GUI color uses a static var, need to save the original to reset it
        GUILayout.BeginHorizontal();                   //All following gui will be on one horizontal line until EndHorizontal is called
        for (int i = 0; i < width; i++)
        {
            GUILayout.BeginVertical();                //All following gui will be in a vertical line
            for (int j = 0; j < height; j++)
            {
                Rect colorRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)); //Reserve a square, which will autofit to the size given
                HandleEvents(evt, colorRect, i, j);

                GUI.color = colors[i, j];            //Same as a 2D array
                GUI.DrawTexture(colorRect, colorTexture); //This is colored by GUI.Color!!!
            }
            GUILayout.EndVertical();                  //End Vertical Zone
        }
        GUILayout.EndHorizontal();                    //End horizontal zone
        GUI.color = oldColor;                         //Restore the old color
    }

    private void HandleEvents(Event evt, Rect colorRect, int i, int j)
    {
        if ((evt.type == EventType.MouseDown || evt.type == EventType.MouseDrag) && colorRect.Contains(evt.mousePosition)) //Can now paint while dragging update
        {
            if (evt.button == 0)                //If mouse button pressed is left
                PaintFill(evt, colorRect, i, j);
            //colors[i, j] = GetColor;          //Set the color of the index
            else
                colors[i, j] = eraseColor;      //Set the color of the index
            evt.Use();                          //The event was consumed, if you try to use event after this, it will be non-sensical
        }
    }


    private void FillMatrix()
    {
        for (int i = 0; i < colors.GetLength(0); i++)
        {
            for (int j = 0; j < colors.GetLength(1); j++)
            {
                colors[i, j] = selectedColor;
            }
        }
    }

    private void Selection(Queue<Vector2> list)
    {
        foreach (Vector2 v in list)
        {
            colors[(int)v.x, (int)v.y] = selectedColor;
        }
    }
    private void PaintFill(Event evt, Rect colorRect, int i, int j)
    {
        if (colors[i, j] != eraseColor)
        {
            Queue<Vector2> toFill = BFSAlgorithm(i, j);

            if (toFill != null)
                Selection(toFill);
        }
    }

    private Queue<Vector2> BFSAlgorithm(int positionXclicked, int positionYclicked)
    {
        Queue<Vector2> myNeighbors = new Queue<Vector2>();
        Queue<Vector2> flagged = new Queue<Vector2>();

        myNeighbors.Enqueue(new Vector2 { x = positionXclicked, y = positionYclicked });

        while (myNeighbors.Count > 0)
        {
            if (!Exist(flagged, myNeighbors.ToArray()[0]))
                flagged.Enqueue(myNeighbors.ToArray()[0]);

            if (myNeighbors != null && flagged != null)
                GetNeighbors(myNeighbors, flagged);

            if (myNeighbors != null)
                myNeighbors.Dequeue();
        }
        return flagged;
    }

    private void GetNeighbors(Queue<Vector2> myNeighbors, Queue<Vector2> flagged)
    {
        Vector2 mycurrentposition = myNeighbors.ToArray()[0];

        int minRow = GetMin((int)mycurrentposition.x);
        int maxRow = GetMax((int)mycurrentposition.x, height);

        int minCol = GetMin((int)mycurrentposition.y);
        int maxCol = GetMax((int)mycurrentposition.y, width);

        AddValidNeighbors(myNeighbors, flagged, minRow, maxRow, minCol, maxCol, mycurrentposition);
    }

    private void AddValidNeighbors(Queue<Vector2> myNeighbors, Queue<Vector2> flagged, int minRow, int maxRow, int minCol, int maxCol, Vector2 mycurrentposition)
    {
        for (int i = minRow; i <= maxRow; i++)
        {
            for (int j = minCol; j <= maxCol; j++)
            {
                Vector2 myVec2 = new Vector2 { x = i, y = j };

                if (Exist(myNeighbors, myVec2))
                    continue;
                else if (colors[i, j] != eraseColor && !Exist(flagged, myVec2))
                    myNeighbors.Enqueue(myVec2);
            }
        }
    }

    private int GetMin(int position)
    {
        return position - UNIT_NEIGHBOR < ZERO_POSITION ? position : position - UNIT_NEIGHBOR;
    }

    private int GetMax(int position, int size)
    {
        return position + UNIT_NEIGHBOR > size ? position : position + UNIT_NEIGHBOR;
    }

    private bool Exist(Queue<Vector2> myList, Vector2 vector2)
    {
        foreach (Vector2 v in myList)
        {
            if (v.Equals(vector2))
                return true;
        }
        return false;
    }
}
