using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private int lineNum;

    [SerializeField]
    private Unit[] obstacle;

    [SerializeField]
    private Unit[] food;

    [SerializeField]
    private Unit human;

    [SerializeField]
    private float startSpeed;

    [SerializeField]
    private float speedAddPerSecond;

    [SerializeField]
    private float humanPosYPercent;

    [SerializeField]
    private int minFoodNum;

    [SerializeField]
    private int maxFoodNum;

    [SerializeField]
    private int minObstacleNum;

    [SerializeField]
    private int maxObstacleNum;

    [SerializeField]
    private Transform container;

    private Queue<UnitScript> pool = new Queue<UnitScript>();

    private List<UnitScript> list = new List<UnitScript>();

    private UnitScript humanGo;

    private Vector2 stepV;

    private float createGap;

    private float[] xPosArr;

    private bool isUpdate;

    private float deltaTime;

    private int createNum;

    private bool isDown;

    private Vector2 mousePos;

    void Awake()
    {
        stepV = new Vector2(mainCamera.aspect * mainCamera.orthographicSize, mainCamera.orthographicSize);

        Unit.lineWidth = stepV.x * 2 / lineNum;

        createGap = stepV.x * 2 / lineNum;

        Application.targetFrameRate = 60;

        xPosArr = new float[lineNum];

        float lineWidth = stepV.x * 2 / lineNum;

        for (int i = 0; i < lineNum; i++)
        {
            float x = -stepV.x + lineWidth * i + lineWidth * 0.5f;

            xPosArr[i] = x;
        }

        humanGo = Create(human, null);



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

        humanGo.transform.localPosition = new Vector3(0, -stepV.y + stepV.y * 2 * humanPosYPercent, -1);

        isUpdate = true;
    }

    private void Update()
    {
        if (isUpdate)
        {
            deltaTime += Time.deltaTime;

            float posY = -(startSpeed + startSpeed + speedAddPerSecond * deltaTime) * deltaTime * stepV.y;

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

                    if (x < -stepV.x + human.size)
                    {
                        x = -stepV.x + human.size;
                    }
                    else if (x > stepV.x - human.size)
                    {
                        x = stepV.x - human.size;
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

                if (Vector2.Distance(unit.transform.position, humanGo.transform.position) < unit.unit.size + humanGo.unit.size)
                {
                    if (unit.unit.unitType == UnitType.FOOD)
                    {
                        list.RemoveAt(i);

                        unit.gameObject.SetActive(false);

                        pool.Enqueue(unit);

                        continue;
                    }
                    else
                    {
                        Over();

                        return;
                    }
                }

                if (unit.transform.position.y + unit.unit.size < -stepV.y)
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

        createNum = 0;

        deltaTime = 0;
    }

    private void CreateObstacleAndFood(float _posY)
    {
        int num = lineNum;

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

    private UnitScript Create(Unit _unit, Transform _parent)
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
}
