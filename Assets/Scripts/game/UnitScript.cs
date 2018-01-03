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

        float scale = unit.size / (unit.sp.texture.width / unit.sp.pixelsPerUnit);

        transform.localScale = new Vector3(scale, scale, 1);
    }

    public static UnitScript Create(Unit _unit)
    {
        GameObject go = new GameObject();

        UnitScript script = go.AddComponent<UnitScript>();

        script.SetUnit(_unit);

        return script;
    }
}
