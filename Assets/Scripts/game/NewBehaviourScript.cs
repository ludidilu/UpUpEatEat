using System.Collections.Generic;
using UnityEngine;

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
    private float speedFixPerSecond;

    [SerializeField]
    private float humanPosY;

    [SerializeField]
    private Transform container;

    private Queue<UnitScript> pool = new Queue<UnitScript>();

    private Queue<UnitScript> queue = new Queue<UnitScript>();

    private UnitScript humanGo;

    private Vector2 stepV;

    void Awake()
    {
        stepV = new Vector2(mainCamera.aspect * mainCamera.orthographicSize, mainCamera.orthographicSize);

        Application.targetFrameRate = 60;


    }
}
