using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Serialization
{
    [Header("String path")]
    private static readonly string PATH = "Assets/Resources/Database/saveFile.txt";

    public static void SaveBinaryFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //FileStream fs = File.Open(Application.dataPath + "dataInfo.dat", FileMode.Append);
        FileStream fs = File.Open(PATH, FileMode.Append);
        ShapeObjectDataInfo shapeObjectDataInfo = new ShapeObjectDataInfo();
        bf.Serialize(fs, shapeObjectDataInfo);
        fs.Close();
    }

    public static void LoadBinaryFile()
    {

    }

    public static void SaveJSONFile()
    {

    }

    public static void LoadJSONFile()
    {

    }

    public static void SaveXMLFile()
    {

    }

    public static void LoadXMLFile()
    {

    }
}
