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
        SetMesh();
    }

    private void UpdateDataInfo(Vector3 position, Color color)
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
        dataInfo = new ShapeObjectDataInfo();
    }

    private void SetMesh()
    {
        GameObject go = GameObject.CreatePrimitive((PrimitiveType)GameManagerScript.Instance.GetMeshType);
        mesh.mesh = go.GetComponent<MeshFilter>().mesh;
        meshCollider.sharedMesh = mesh.mesh;
        Destroy(go);
    }
}