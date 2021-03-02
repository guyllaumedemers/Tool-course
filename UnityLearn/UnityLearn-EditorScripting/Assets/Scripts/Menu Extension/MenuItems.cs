using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuItems
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    private static void NewMenuOption()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Window/New Option #w")]
    private static void NewOption()
    {
        Debug.Log("Window/New Option");
    }

    [MenuItem("Tool/SubMenu/Option")]
    private static void NewNestedOption()
    {
        Debug.Log("Tool/SubMenu/Option");
    }

    [MenuItem("Assets/Load Additive Scene")]
    private static void LoadAdditiveScene()
    {
        var selected = Selection.activeObject;
        EditorApplication.OpenSceneAdditive(AssetDatabase.GetAssetPath(selected));
    }

    [MenuItem("Assets/Create/Add Configuration")]
    private static void AddConfig()
    {
        Debug.Log("Add COnfiguration");
    }

    [MenuItem("CONTEXT/Rigidbody/New Option")]
    private static void NewOptionForRigigbody()
    {
        Debug.Log("New Option For Rigigbody");
    }

    [MenuItem("Assets/ProcessMaterial")]
    private static void DoSomethingWithTexture()
    {
        Debug.Log("Process Material");
    }

    [MenuItem("Assets/ProcessMaterial", true)]
    private static bool NewMenuOptionValidation()
    {
        return Selection.activeObject.GetType() == typeof(Material);
    }
}
