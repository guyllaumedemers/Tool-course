using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class PoolPreviewWindow : EditorWindow
{
    readonly string COLLECTION_PATH = "Assets/Resources/ScriptableObjects";
    readonly string ICON_PATH = "Assets/Resources/Sprites/ScriptableAsset/Icons/icon.png";
    readonly string PREFIX = "Assets/Resources/";
    Object buttonPreview;
    Texture texture;
    Vector2 scrollview;
    bool isActive;
    string selection_path;
    string activeFolder;

    Dictionary<string, System.Type> keyValuePairs = new Dictionary<string, System.Type>()
    {
        { "Classes", typeof(ClassInfo) },
        { "Minions", typeof(MinionInfo) },
        { "Spells", typeof(MagicCardInfo) }
    };


    [MenuItem("Custom Windows/ScriptablePool")]
    public static void OpenWindow()
    {
        EditorWindow.GetWindow<PoolPreviewWindow>();
    }

    private void OnEnable()
    {
        scrollview = new Vector2();
        selection_path = null;
        isActive = false;
        activeFolder = null;
    }

    private void OnGUI()
    {
        ///// First retrieve the folder name from the database so we can assign each to a specific button
        string[] sub_folder = AssetDatabase.GetSubFolders(COLLECTION_PATH);
        for (int i = 0; i < sub_folder.Length; i++)
        {
            ////// Remove folder name path and extension
            sub_folder[i] = Path.GetFileNameWithoutExtension(sub_folder[i]);
        }
        EditorGUILayout.BeginVertical();

        buttonPreview = AssetDatabase.LoadAssetAtPath(ICON_PATH, typeof(Object));
        texture = AssetPreview.GetAssetPreview(buttonPreview);
        ///// Create the same amount of buttons as folder their is
        EditorGUILayout.BeginHorizontal();
        foreach (string folder_name in sub_folder)
        {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button(texture, GUILayout.MaxWidth(75), GUILayout.MaxHeight(75)))
            {
                ///// if button is clicked, display the content of the button selection
                selection_path = $"ScriptableObjects/{folder_name}/";
                isActive = true;
                activeFolder = folder_name;
            }
            GUILayout.Label(folder_name);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();

        if (isActive)
        {
            scrollview = EditorGUILayout.BeginScrollView(scrollview);

            Object[] folder_content = Resources.LoadAll(selection_path);
            EditorGUILayout.BeginHorizontal();
            foreach (Object myObject in folder_content)
            {
                if (myObject != null)
                    Display(myObject);
            }

            EditorGUILayout.EndHorizontal();
            System.Type myType = keyValuePairs[activeFolder];

            if (GUILayout.Button(new GUIContent("+")))
                CreateNewAsset(myType, selection_path);
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }

    private void Display(Object myObject)
    {
        EditorGUILayout.BeginVertical();

        Editor myEditor = Editor.CreateEditor(myObject);
        myEditor.OnInspectorGUI();

        EditorGUILayout.EndVertical();
    }

    private void CreateNewAsset(System.Type type, string selectionPath)
    {
        ScriptableObject scriptableObject = ScriptableObject.CreateInstance(type.ToString());
        AssetDatabase.CreateAsset(scriptableObject, PREFIX + selectionPath + "newAsset.asset");
        AssetDatabase.SaveAssets();

        Selection.activeObject = scriptableObject;
    }
}
