using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public Unit unit { private set; get; }

    private SpriteRenderer sr;

    void Awake()
    {
        sr = gameObject.AddComponent<SpriteRenderer>();
    }

    public void SetUnit(Unit _unit)
    {
        unit = _unit;

        sr.sprite = unit.sp;

        float scale = unit.size / (unit.sp.rect.width * 0.5f / unit.sp.pixelsPerUnit);

        transform.localScale = new Vector3(scale, scale, 1);
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
