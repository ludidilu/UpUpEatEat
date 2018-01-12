public class ConfigDictionary : Config
{
    private static ConfigDictionary _Instnce;

    public static ConfigDictionary Instance
    {
        get
        {
            if (_Instnce == null)
            {
                _Instnce = new ConfigDictionary();
            }

            return _Instnce;
        }
    }

    public string csvPath;

    public int lineNum;

    public float humanPosY;

    public float humanRadius;

    public float time;
}
