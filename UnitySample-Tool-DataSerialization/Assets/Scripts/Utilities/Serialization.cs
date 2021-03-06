using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System;

public static class Serialization
{
    [Header("String path")]
    private static readonly string BINARY_PATH = "Assets/Resources/Database/saveBinaryFile.txt";
    private static readonly string XML_PATH = "Assets/Resources/Database/saveXMLFile.xml";
    private static readonly string JSON_PATH = "Assets/Resources/Database/saveJSONFile.txt";

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
        FileStream fs = File.Open(XML_PATH, FileMode.Append);
        try
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ShapeObjectDataInfo>));
            xmlSerializer.Serialize(fs, GameManagerScript.Instance.GetContainer.GetDataInfos);
        }
        catch (SerializationException e)
        {
            Debug.Log("Failed to Serialize : " + e.Message);
        }
        finally
        {
            fs.Close();
        }
    }

    private static void LoadXMLFile()
    {
        FileStream fs = File.Open(XML_PATH, FileMode.Open);
        try
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ShapeObjectDataInfo>));
            GameManagerScript.Instance.GetContainer.GetDataInfos = xmlSerializer.Deserialize(fs) as List<ShapeObjectDataInfo>;
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

    #region BINARY
    private static void SaveBinaryFile()
    {
        FileStream fs = File.Open(BINARY_PATH, FileMode.Append);
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
        FileStream fs = File.OpenRead(BINARY_PATH);
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
