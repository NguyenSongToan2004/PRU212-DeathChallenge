using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FileHandler
{
    //public static void SaveToJSON<T>(List<T> toSave, string filename)
    //{
    //    if (string.IsNullOrEmpty(filename))
    //    {
    //        Debug.LogError("Filename is null or empty!");
    //        return;
    //    }

    //    try
    //    {
    //        string path = GetPath(filename);
    //        Debug.Log($"Saving to path: {path}");
    //        string content = JsonHelper.ToJson<T>(toSave.ToArray());
    //        WriteFile(path, content);
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogError($"Failed to save JSON file '{filename}': {e.Message}");
    //    }
    //}

    //public static void SaveToJSON<T>(T toSave, string filename)
    //{
    //    if (string.IsNullOrEmpty(filename))
    //    {
    //        Debug.LogError("Filename is null or empty!");
    //        return;
    //    }

    //    try
    //    {
    //        string content = JsonUtility.ToJson(toSave);
    //        WriteFile(GetPath(filename), content);
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogError($"Failed to save JSON file '{filename}': {e.Message}");
    //    }
    //}

    //public static List<T> ReadListFromJSON<T>(string filename)
    //{
    //    if (string.IsNullOrEmpty(filename))
    //    {
    //        Debug.LogError("Filename is null or empty!");
    //        return new List<T>();
    //    }

    //    try
    //    {
    //        string content = ReadFile(GetPath(filename));

    //        if (string.IsNullOrEmpty(content) || content == "{}")
    //        {
    //            return new List<T>();
    //        }

    //        List<T> res = JsonHelper.FromJson<T>(content).ToList();
    //        return res;
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogError($"Failed to read JSON file '{filename}': {e.Message}");
    //        return new List<T>();
    //    }
    //}


    //public static T ReadFromJSON<T>(string filename)
    //{
    //    if (string.IsNullOrEmpty(filename))
    //    {
    //        Debug.LogError("Filename is null or empty!");
    //        return default(T);
    //    }

    //    try
    //    {
    //        string content = ReadFile(GetPath(filename));

    //        if (string.IsNullOrEmpty(content) || content == "{}")
    //        {
    //            return default(T);
    //        }

    //        T res = JsonUtility.FromJson<T>(content);
    //        return res;
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogError($"Failed to read JSON file '{filename}': {e.Message}");
    //        return default(T);
    //    }
    //}

    //private static string GetPath(string filename)
    //{
    //    return @$"D:\\{filename}";
    //}

    //private static void WriteFile(string path, string content)
    //{
    //    if (string.IsNullOrEmpty(path))
    //    {
    //        Debug.LogError("Cannot write file: path is null or empty!");
    //        return;
    //    }

    //    try
    //    {
    //        FileStream fileStream = new FileStream(path, FileMode.Create);

    //        using (StreamWriter writer = new StreamWriter(fileStream))
    //        {
    //            writer.Write(content);
    //        }
    //        Debug.Log($"Successfully wrote file: {path}");
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogError($"Failed to write file '{path}': {e.Message}");
    //    }
    //}

    //private static string ReadFile(string path)
    //{
    //    if (string.IsNullOrEmpty(path))
    //    {
    //        Debug.LogError("Cannot read file: path is null or empty!");
    //        return "";
    //    }

    //    try
    //    {
    //        if (File.Exists(path))
    //        {
    //            using (StreamReader reader = new StreamReader(path))
    //            {
    //                string content = reader.ReadToEnd();
    //                Debug.Log($"Successfully read file: {path}");
    //                return content;
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"File does not exist: {path}");
    //        }
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogError($"Failed to read file '{path}': {e.Message}");
    //    }

    //    return "";
    //}

    private static string GetPath(string filename)
    {
        string dir = Application.persistentDataPath;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        return Path.Combine(dir, filename);
    }

    public static void SaveToJSON<T>(List<T> toSave, string filename)
    {
        try
        {
            string json = JsonHelper.ToJson(toSave.ToArray(), true);
            File.WriteAllText(GetPath(filename), json);
            Debug.Log($"Saved highscores to {filename}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving file {filename}: {e.Message}");
        }
    }

    public static List<T> ReadListFromJSON<T>(string filename)
    {
        try
        {
            string path = GetPath(filename);
            if (!File.Exists(path)) return new();
            string json = File.ReadAllText(path);
            return new List<T>(JsonHelper.FromJson<T>(json));
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading file {filename}: {e.Message}");
            return new();
        }
    }
}

public static class JsonHelper
{
    //public static T[] FromJson<T>(string json)
    //{
    //    Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
    //    return wrapper.Items;
    //}

    //public static string ToJson<T>(T[] array)
    //{
    //    Wrapper<T> wrapper = new Wrapper<T>();
    //    wrapper.Items = array;
    //    return JsonUtility.ToJson(wrapper);
    //}

    //public static string ToJson<T>(T[] array, bool prettyPrint)
    //{
    //    Wrapper<T> wrapper = new Wrapper<T>();
    //    wrapper.Items = array;
    //    return JsonUtility.ToJson(wrapper, prettyPrint);
    //}

    //[Serializable]
    //private class Wrapper<T>
    //{
    //    public T[] Items;
    //}

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }

    public static T[] FromJson<T>(string json)
    {
        var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        var wrapper = new Wrapper<T> { Items = array };
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
}