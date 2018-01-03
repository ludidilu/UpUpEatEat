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
    public Sprite sp;

    public float size;

    public UnitType isObstacle;
}
