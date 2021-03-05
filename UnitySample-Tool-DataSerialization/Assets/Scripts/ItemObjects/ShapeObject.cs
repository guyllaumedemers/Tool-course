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

    private int index = 0;

    private void Awake()
    {
        InitializeComponentsObject();
        index = UnityEngine.Random.Range(0, GameManagerScript.Instance.GetMaterials.Length - 1);
        dataInfo = new ShapeObjectDataInfo();
        SetMesh();
    }

    private void Update()
    {
        if (dataInfo.GetPosition.GetY > 0)
            UpdateDataInfo(ToCustomVector(this.transform.position), ToCustomColor(meshRenderer.material.color));
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
        meshRenderer.material = GameManagerScript.Instance.GetMaterials[index];
        meshCollider.sharedMesh = mesh.mesh;
        Destroy(go);
    }

    private MyVector3 ToCustomVector(Vector3 pos)
    {
        MyVector3 myVector3 = new MyVector3();
        myVector3.GetX = pos.x;
        myVector3.GetY = pos.y;
        myVector3.GetZ = pos.z;

        return myVector3;
    }

    private MyColor ToCustomColor(Color color)
    {
        MyColor myColor = new MyColor();
        myColor.GetX = color.r;
        myColor.GetY = color.g;
        myColor.GetZ = color.b;
        myColor.GetW = color.a;

        return myColor;
    }

    public ShapeObjectDataInfo GetDataInfo { get => dataInfo; set { dataInfo = value; } }
}