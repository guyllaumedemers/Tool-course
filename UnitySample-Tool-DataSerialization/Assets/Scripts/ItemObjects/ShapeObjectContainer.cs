using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
[XmlRoot("DataCollection")]
public class ShapeObjectContainer
{
    [SerializeField]
    [XmlArray("DataEntries"), XmlArrayItem("Data")]
    private List<ShapeObjectDataInfo> dataInfos;

    public ShapeObjectContainer()
    {
        dataInfos = new List<ShapeObjectDataInfo>();
    }

    [XmlIgnore] public List<ShapeObjectDataInfo> GetDataInfos { get => dataInfos; set { dataInfos = value; } }
}
