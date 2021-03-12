using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    private static ToolManager instance;
    public static ToolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ToolManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    instance = go.AddComponent<ToolManager>();
                }
            }
            return instance;
        }
    }

    public AnimationCurveDrawer animationCurveInstance;

    private void Start()
    {
        Debug.Log("Evaluate : " + animationCurveInstance.GetAnimationCurve.Evaluate(5));
    }
}
