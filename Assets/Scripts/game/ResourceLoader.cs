using System;
using UnityEngine;
using System.IO;

public static class ResourceLoader
{
    public static void Start(Action _callBack)
    {
        LoadCsv();

        StaticData.path = ConfigDictionary.Instance.csvPath;

        LoadCsv();
    }

    public static void LoadConfig()
    {
        ConfigDictionary.Instance.LoadLocalConfig(Path.Combine(Application.streamingAssetsPath, "config.xml"));
    }

    public static void LoadCsv()
    {
        StaticData.Load<TimeSDS>("time");

        StaticData.Load<FoodSDS>("food");

        StaticData.Load<ObstacleSDS>("obstacle");
    }
}
