using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(Rigidbody))]
public class ShapeObject : MonoBehaviour
{
    [Header("Required Components")]
    private Rigidbody rb;
    private MeshFilter mesh;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    [Header("Serialize Data")]
    private ShapeObjectDataInfo dataInfo;

    private void Awake()
    {
        InitializeComponentsObject();
    }

    private void Start()
    {
        dataInfo = new ShapeObjectDataInfo();
        SetMesh();
    }

    public void UpdateDataInfo(MyVector3 position, MyColor color)
    {
        dataInfo.GetPosition = position;
        dataInfo.GetColor = color;
    }

    private void InitializeComponentsObject()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    private void SetMesh()
    {
        GameObject go = GameObject.CreatePrimitive((PrimitiveType)GameManagerScript.Instance.GetMeshType);
        mesh.mesh = go.GetComponent<MeshFilter>().mesh;
        meshCollider.sharedMesh = mesh.mesh;
        Destroy(go);
    }

    public ShapeObjectDataInfo GetDataInfo { get => dataInfo; }
}