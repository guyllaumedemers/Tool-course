using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class Serialization
{
    [Header("String path")]
    private static readonly string PATH = "Assets/Resources/Database/saveFile.txt";

    public static void SaveBinaryFile()
    {
        FileStream fs = File.Open(PATH, FileMode.Append);
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, GameManagerScript.Instance.GetShapeObjectDataInfos);
        }
        catch (SerializationException e)
        {
            Debug.Log("Failed to Serialize : " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }
    }

    public static void LoadBinaryFile()
    {
        FileStream fs = File.OpenRead(PATH);
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            GameManagerScript.Instance.GetShapeObjectDataInfos = (List<ShapeObjectDataInfo>)bf.Deserialize(fs);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("Directory not found : " + e.Message);
        }
        finally
        {
            fs.Close();
        }
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
