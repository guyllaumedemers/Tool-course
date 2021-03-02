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

    private Color[] colors;
    private int width = 8;
    private int height = 8;
    Texture colorTexture;
    Renderer textureTarget;

    Color selectedColor = Color.white;
    Color eraseColor = Color.white;

    public delegate void MyDelegate(Color color);
    public MyDelegate myDelegate;

    private readonly float MAX_RGBA_VALUE = 1.0f;
    private readonly float MIN_RGBA_VALUE = 0.0f;
    private readonly int NUMBER_DECIMAL = 2;
    private readonly float CLAMP_OFFSET = 0.01f;

    public void OnEnable()
    {
        colors = new Color[width * height];
        for (int i = 0; i < colors.Length; i++)
            colors[i] = GetRandomColor();
        colorTexture = EditorGUIUtility.whiteTexture;
        myDelegate += AddRandomRGBA;
    }

    private Color GetColor
    {
        get => selectedColor;
        set
        {
            if (selectedColor == value)
                return;
            myDelegate.Invoke(selectedColor);
        }
    }

    private Color GetRandomColor()  //Built a get random color tool
    {
        return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1f);
    }

    /// <summary>
    /// Could have use Mathf.Clamp but I like mine better
    /// </summary>
    private void GetRGBAFields()
    {
        Color oldColor = selectedColor;
        UpdateClampValues();
        GetColor = oldColor; // => Property use to trigger delegate onValueChange for selectedColor
    }

    private void AddRandomRGBA(Color color)
    {
        color.r += UnityEngine.Random.Range(-0.2f, 0.2f);
        color.g += UnityEngine.Random.Range(-0.2f, 0.2f);
        color.b += UnityEngine.Random.Range(-0.2f, 0.2f);
        color.a += UnityEngine.Random.Range(-0.2f, 0.2f);
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
            colors = colors.Select(c => c = selectedColor).ToArray();                   //Linq expresion, for every color in the color array, sets it to the selected color

        GetRGBAFields();

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
                    int index = j + i * height;
                    t2d.SetPixel(i, height - 1 - j, colors[index]);                     //Color every pixel using our color table, the texture is 8x8 pixels large, but strecthes to fit
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
                int index = j + i * height;           //Rememeber, this is just like a 2D array, but in 1D
                Rect colorRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)); //Reserve a square, which will autofit to the size given
                if ((evt.type == EventType.MouseDown || evt.type == EventType.MouseDrag) && colorRect.Contains(evt.mousePosition)) //Can now paint while dragging update
                {
                    if (evt.button == 0)                //If mouse button pressed is left
                        colors[index] = selectedColor; //Set the color of the index
                    else
                        colors[index] = eraseColor;   //Set the color of the index
                    evt.Use();                        //The event was consumed, if you try to use event after this, it will be non-sensical
                }

                GUI.color = colors[index];            //Same as a 2D array
                GUI.DrawTexture(colorRect, colorTexture); //This is colored by GUI.Color!!!
            }
            GUILayout.EndVertical();                  //End Vertical Zone
        }
        GUILayout.EndHorizontal();                    //End horizontal zone
        GUI.color = oldColor;                         //Restore the old color
    }
}
