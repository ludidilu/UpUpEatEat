using System;
using UnityEngine;

[Serializable]
public enum UnitType
{
    HUMAN,
    OBSTACLE,
    FOOD
}

[Serializable]
public class Unit
{
    public static float lineWidth;

    public Sprite sp;

    public float radius;

    public float size
    {
        get
        {
            return radius * lineWidth;
        }
    }

    public UnitType unitType;
}
