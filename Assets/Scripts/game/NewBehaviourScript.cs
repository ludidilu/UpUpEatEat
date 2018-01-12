using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private RuntimeAnimatorController ra;

    [SerializeField]
    private Transform container;

    [SerializeField]
    private SpriteRenderer timer;

    [SerializeField]
    private TextMesh tm;

    [SerializeField]
    private TextMeshOutline tmo;

    [SerializeField]
    private float timerHeight;

    private Queue<UnitScript> pool = new Queue<UnitScript>();

    private List<UnitScript> list = new List<UnitScript>();

    private UnitScript humanGo;

    private Animator humanAnimator;

    private Vector2 stepV;

    private float createGap;

    private float[] xPosArr;

    private bool isUpdate;

    private float deltaTime;

    private int createNum;

    private bool isDown;

    private Vector2 mousePos;

    private float timerScaleX;

    private float timerScaleY;

    private float m_time;

    private List<TimeSDS> timeList;

    private float[] levelUpTimeArr;

    private int timeIndex;

    private float time
    {
        set
        {
            m_time = value;

            timer.transform.localScale = new Vector3(timerScaleX * time, timerScaleY, 1);

            timer.transform.position = new Vector3(-stepV.x * (1 - time), stepV.y - timerHeight * stepV.y * 0.5f, -1);
        }

        get
        {
            return m_time;
        }
    }

    private int m_score;

    private int score
    {
        set
        {
            m_score = value;

            string str = m_score.ToString();

            tm.text = str;

            tmo.SetText(str);
        }

        get
        {
            return m_score;
        }
    }

    void Awake()
    {
        ResourceLoader.Start(null);

        timeList = StaticData.GetList<TimeSDS>();

        levelUpTimeArr = new float[timeList.Count];

        levelUpTimeArr[0] = timeList[0].time;

        for (int i = 1; i < timeList.Count; i++)
        {
            levelUpTimeArr[i] = levelUpTimeArr[i - 1] + timeList[i].time;
        }

        stepV = new Vector2(mainCamera.aspect * mainCamera.orthographicSize, mainCamera.orthographicSize);

        UnitScript.lineWidth = stepV.x * 2 / ConfigDictionary.Instance.lineNum;

        createGap = stepV.x * 2 / ConfigDictionary.Instance.lineNum;

        Application.targetFrameRate = 60;

        xPosArr = new float[ConfigDictionary.Instance.lineNum];

        float lineWidth = stepV.x * 2 / ConfigDictionary.Instance.lineNum;

        for (int i = 0; i < ConfigDictionary.Instance.lineNum; i++)
        {
            float x = -stepV.x + lineWidth * i + lineWidth * 0.5f;

            xPosArr[i] = x;
        }

        humanGo = Create("human", ConfigDictionary.Instance.humanRadius, null);

        humanGo.transform.localPosition = new Vector3(0, -stepV.y + stepV.y * 2 * ConfigDictionary.Instance.humanPosY, -1);

        humanAnimator = humanGo.gameObject.AddComponent<Animator>();

        humanAnimator.runtimeAnimatorController = ra;

        timerScaleX = stepV.x / (timer.sprite.rect.width * 0.5f / timer.sprite.pixelsPerUnit);

        timerScaleY = timerHeight * stepV.y / (timer.sprite.rect.height * 0.5f / timer.sprite.pixelsPerUnit);

        tm.transform.position = new Vector3(0, stepV.y - timerHeight * stepV.y * 2f, -2);

        time = ConfigDictionary.Instance.time;

        score = 0;

        StartGame();
    }

    private void StartGame()
    {
        for (int i = 0; i < list.Count; i++)
        {
            UnitScript unit = list[i];

            unit.gameObject.SetActive(false);

            pool.Enqueue(unit);
        }

        list.Clear();

        container.localPosition = Vector3.zero;

        humanGo.transform.localPosition = new Vector3(0, -stepV.y + stepV.y * 2 * ConfigDictionary.Instance.humanPosY, -1);

        time = ConfigDictionary.Instance.time;

        createNum = 0;

        deltaTime = 0;

        score = 0;

        timeIndex = 0;

        isUpdate = true;
    }

    private void Update()
    {
        if (isUpdate)
        {
            time -= Time.deltaTime;

            if (time < 0)
            {
                Over();

                return;
            }


            float posY = container.localPosition.y;

            if (timeIndex == levelUpTimeArr.Length - 1)
            {
                posY -= timeList[timeIndex].speed * deltaTime * stepV.y;
            }
            else if (deltaTime + Time.deltaTime > levelUpTimeArr[timeIndex])
            {
                float p;

                if (timeIndex == 0)
                {
                    p = deltaTime;
                }
                else
                {
                    p = deltaTime - levelUpTimeArr[timeIndex - 1];
                }

                float p0 = p / timeList[timeIndex].time;

                float v0 = timeList[timeIndex].speed * (1 - p0) + timeList[timeIndex + 1].speed * p0;

                posY -= (v0 + timeList[timeIndex + 1].speed) * (levelUpTimeArr[timeIndex] - deltaTime) * stepV.y * 0.5f;


                p = deltaTime + Time.deltaTime - levelUpTimeArr[timeIndex];

                timeIndex++;

                p0 = p / timeList[timeIndex].time;

                v0 = timeList[timeIndex].speed * (1 - p0) + timeList[timeIndex + 1].speed * p0;

                posY -= (timeList[timeIndex].speed + v0) * p * stepV.y * 0.5f;
            }
            else
            {
                float p;

                if (timeIndex == 0)
                {
                    p = deltaTime;
                }
                else
                {
                    p = deltaTime - levelUpTimeArr[timeIndex - 1];
                }

                float p0 = p / timeList[timeIndex].time;

                float p1 = (p + Time.deltaTime) / timeList[timeIndex].time;

                float v0 = timeList[timeIndex].speed * (1 - p0) + timeList[timeIndex + 1].speed * p0;

                float v1 = timeList[timeIndex].speed * (1 - p1) + timeList[timeIndex + 1].speed * p1;

                posY -= (v0 + v1) * deltaTime * stepV.y * 0.5f;
            }

            deltaTime += Time.deltaTime;

            container.localPosition = new Vector3(container.localPosition.x, posY, container.localPosition.z);

            int num = (int)(Mathf.Abs(posY) / createGap) + 1;

            if (num > createNum)
            {
                for (int i = createNum; i < num; i++)
                {
                    CreateObstacleAndFood(createGap * (i + 1) + stepV.y);
                }

                createNum = num;
            }

            if (Input.GetMouseButtonDown(0))
            {
                isDown = true;

                mousePos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                if (isDown)
                {
                    float x = humanGo.transform.localPosition.x + (Input.mousePosition.x - mousePos.x) / Screen.width * (stepV.x * 2);

                    if (x < -stepV.x + humanGo.size)
                    {
                        x = -stepV.x + humanGo.size;
                    }
                    else if (x > stepV.x - humanGo.size)
                    {
                        x = stepV.x - humanGo.size;
                    }

                    humanGo.transform.localPosition = new Vector3(x, humanGo.transform.localPosition.y, humanGo.transform.localPosition.z);

                    mousePos = Input.mousePosition;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDown = false;
            }

            for (int i = list.Count - 1; i > -1; i--)
            {
                UnitScript unit = list[i];

                if (Vector2.Distance(unit.transform.position, humanGo.transform.position) < unit.size + humanGo.size)
                {
                    //if (unit.unit.unitType == UnitType.FOOD)
                    //{
                    //    list.RemoveAt(i);

                    //    unit.gameObject.SetActive(false);

                    //    pool.Enqueue(unit);

                    //    continue;
                    //}
                    //else
                    //{
                    //    Over();

                    //    return;
                    //}

                    list.RemoveAt(i);

                    unit.gameObject.SetActive(false);

                    pool.Enqueue(unit);

                    time += unit.unit.timeChange;

                    score += unit.unit.score;

                    if (unit.unit.unitType == UnitType.OBSTACLE)
                    {
                        humanAnimator.SetTrigger("hit");
                    }
                    else
                    {
                        if (time > ConfigDictionary.Instance.time)
                        {
                            time = ConfigDictionary.Instance.time;
                        }
                    }
                }

                if (unit.transform.position.y + unit.size < -stepV.y)
                {
                    list.RemoveAt(i);

                    unit.gameObject.SetActive(false);

                    pool.Enqueue(unit);
                }
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                StartGame();
            }
        }
    }

    private void Over()
    {
        isUpdate = false;

        isDown = false;
    }

    private void CreateObstacleAndFood(float _posY)
    {
        int num = ConfigDictionary.Instance.lineNum;

        int foodNum = (int)(Random.value * (maxFoodNum + 1 - minFoodNum)) + minFoodNum;

        for (int i = 0; i < foodNum && num > 0; i++)
        {
            int foodIndex = (int)(Random.value * food.Length);

            int posIndex = (int)(Random.value * num);

            UnitScript unit = Create(food[foodIndex], container);

            list.Add(unit);

            float x = xPosArr[posIndex];

            unit.transform.localPosition = new Vector3(x, _posY, 0);

            xPosArr[posIndex] = xPosArr[num - 1];

            xPosArr[num - 1] = x;

            num--;
        }

        int obstacleNum = (int)(Random.value * (maxObstacleNum + 1 - minObstacleNum)) + minObstacleNum;

        for (int i = 0; i < obstacleNum && num > 0; i++)
        {
            int obstacleIndex = (int)(Random.value * obstacle.Length);

            int posIndex = (int)(Random.value * num);

            UnitScript unit = Create(obstacle[obstacleIndex], container);

            list.Add(unit);

            float x = xPosArr[posIndex];

            unit.transform.localPosition = new Vector3(x, _posY, 0);

            xPosArr[posIndex] = xPosArr[num - 1];

            xPosArr[num - 1] = x;

            num--;
        }
    }

    private UnitScript Create(ObstacleSDS _unit, Transform _parent)
    {
        UnitScript unit;

        if (pool.Count > 0)
        {
            unit = pool.Dequeue();
        }
        else
        {
            unit = UnitScript.Create(_parent);
        }

        unit.gameObject.SetActive(true);

        unit.SetUnit(_unit);

        return unit;
    }

    private UnitScript Create(string _icon, float _radius, Transform _parent)
    {
        UnitScript unit;

        if (pool.Count > 0)
        {
            unit = pool.Dequeue();
        }
        else
        {
            unit = UnitScript.Create(_parent);
        }

        unit.gameObject.SetActive(true);

        unit.SetUnit(_icon, _radius);

        return unit;
    }
}
