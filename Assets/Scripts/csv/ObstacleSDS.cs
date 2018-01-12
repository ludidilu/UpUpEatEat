public enum UnitType
{
    HUMAN,
    OBSTACLE,
    FOOD
}

public class ObstacleSDS : CsvBase
{
    public string icon;
    public float radius;
    public float timeChange;
    public int score;

    public virtual UnitType unitType
    {
        get
        {
            return UnitType.OBSTACLE;
        }
    }
}
