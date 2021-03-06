using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;

public static class Serialization
{
    [Header("String path")]
    private static readonly string PATH = "Assets/Resources/Database/saveFile.txt";

    public static void Save(EnumSerializationType serializationType)
    {
        switch (serializationType)
        {
            case EnumSerializationType.XML:
                SaveXMLFile();
                break;
            case EnumSerializationType.BINARY:
                SaveBinaryFile();
                break;
            case EnumSerializationType.JSON:
                SaveJSONFile();
                break;
        }
    }

    public static void Load(EnumSerializationType serializationType)
    {
        switch (serializationType)
        {
            case EnumSerializationType.XML:
                LoadXMLFile();
                break;
            case EnumSerializationType.BINARY:
                LoadBinaryFile();
                break;
            case EnumSerializationType.JSON:
                LoadJSONFile();
                break;
        }
    }

    #region XML
    private static void SaveXMLFile()
    {
        //var serializer = new XmlSerializer(typeof(MonsterContainer));
        //var stream = new FileStream(path, FileMode.Create));
        //serializer.Serialize(stream, this);
        //stream.Close();
    }

    private static void LoadXMLFile()
    {
        //var serializer = new XmlSerializer(typeof(MonsterContainer));
        //var stream = new FileStream(path, FileMode.Open);
        //var container = serializer.Deserialize(stream) as MonsterContainer;
        //stream.Close();
    }
    #endregion

    #region BINARY
    private static void SaveBinaryFile()
    {
        FileStream fs = File.Open(PATH, FileMode.Append);
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, GameManagerScript.Instance.GetContainer.GetDataInfos);
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

    private static void LoadBinaryFile()
    {
        FileStream fs = File.OpenRead(PATH);
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            GameManagerScript.Instance.GetContainer.GetDataInfos = (List<ShapeObjectDataInfo>)bf.Deserialize(fs);
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
    #endregion

    #region JSON
    private static void SaveJSONFile()
    {

    }

    private static void LoadJSONFile()
    {

    }
    #endregion
}
