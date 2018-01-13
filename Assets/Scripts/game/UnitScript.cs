using UnityEngine;
using textureFactory;

public class UnitScript : MonoBehaviour
{
    private const string spritePath = "Assets/Resource/texture/{0}.png";

    public ObstacleSDS unit { private set; get; }

    private SpriteRenderer sr;

    private float radius;

    public static float lineWidth;

    public float size
    {
        get
        {
            return radius * lineWidth;
        }
    }

    void Awake()
    {
        sr = gameObject.AddComponent<SpriteRenderer>();
    }

    public void SetUnit(ObstacleSDS _unit)
    {
        unit = _unit;

        SetUnit(unit.icon, unit.radius);
    }

    public void SetUnit(string _icon, float _radius)
    {
        Sprite sp = GetSp(_icon);

        radius = _radius;

        sr.sprite = sp;

        float scale = size / (sp.rect.width * 0.5f / sp.pixelsPerUnit);

        transform.localScale = new Vector3(scale, scale, 1);
    }

    public Sprite GetSp(string _path)
    {
        return Resources.Load<Sprite>(_path);

        //return TextureFactory.Instance.GetTexture<Sprite>(string.Format(spritePath, _path), null, true);
    }

    public static UnitScript Create(Transform _parent)
    {
        GameObject go = new GameObject();

        if (_parent != null)
        {
            go.transform.SetParent(_parent, false);
        }

        UnitScript script = go.AddComponent<UnitScript>();

        return script;
    }
}
