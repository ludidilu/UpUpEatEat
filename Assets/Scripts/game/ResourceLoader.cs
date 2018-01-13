using System;
using UnityEngine;
using System.IO;
using assetManager;
using textureFactory;
using System.Collections.Generic;

public static class ResourceLoader
{
    private static Action callBack;

    public static void Start(Action _callBack)
    {
        callBack = _callBack;

        LoadConfig();

        StaticData.path = Path.Combine(Application.streamingAssetsPath, "csv");

        LoadCsv();

        AssetManager.Instance.Init(InitOver);
    }

    private static void InitOver()
    {
        Dictionary<int, ObstacleSDS> obstacle = StaticData.GetDic<ObstacleSDS>();

        Dictionary<int, FoodSDS> food = StaticData.GetDic<FoodSDS>();

        int num = obstacle.Count + food.Count + 3;

        Action<Sprite> dele = delegate (Sprite _sp)
        {
            num--;

            if (num == 0)
            {
                callBack();
            }
        };

        TextureFactory.Instance.GetTexture(string.Format(UnitScript.spritePath, ConfigDictionary.Instance.humanSpriteName), dele, true);

        TextureFactory.Instance.GetTexture<Sprite>(string.Format(UnitScript.spritePath, "blank"), dele, true);

        IEnumerator<ObstacleSDS> enumerator = obstacle.Values.GetEnumerator();

        while (enumerator.MoveNext())
        {
            TextureFactory.Instance.GetTexture(string.Format(UnitScript.spritePath, enumerator.Current.icon), dele, true);
        }

        IEnumerator<FoodSDS> enumerator2 = food.Values.GetEnumerator();

        while (enumerator2.MoveNext())
        {
            TextureFactory.Instance.GetTexture(string.Format(UnitScript.spritePath, enumerator2.Current.icon), dele, true);
        }

        dele(null);
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
