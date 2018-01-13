using System;
using UnityEngine;
using System.IO;

public static class ResourceLoader
{
    public static void Start(Action _callBack)
    {
        LoadConfig();

        StaticData.path = Path.Combine(Application.streamingAssetsPath, "csv");

        LoadCsv();
    }

    public static void LoadConfig()
    {
        ConfigDictionary.Instance.LoadLocalConfig(Path.Combine(Application.streamingAssetsPath, "config.xml"));
    }

    public static void LoadCsv()
    {
        StaticData.Dispose();

        StaticData.Load<TimeSDS>("time");

        StaticData.Load<FoodSDS>("food");

        StaticData.Load<ObstacleSDS>("obstacle");
    }
}
