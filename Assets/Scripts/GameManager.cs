using System;
using Rpg;
using UnityEngine;
using Rpg.Unit;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject activeUnitMenu;
    public GameObject unitInfoBox;
    public GameObject targetActionBox;
    public Animator GameStateMachine { get; private set; }
    

    public int pixelsToUnits = 100;

    [HideInInspector] public LevelManager levelManager;
    [HideInInspector] public InputManager inputManager;
    [HideInInspector] public IActionQueue actionQueue;
    [HideInInspector] public BattleManager battleManager;
    [HideInInspector] public AudioManager audioManager;
    [HideInInspector] public PopManager popManager;
    [HideInInspector] public PathManager PathManager { get; private set; }
    [HideInInspector] public UnitTurn UnitTurn { get; set; }
    [HideInInspector] public CameraManager CameraManager { get; private set; }


    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        levelManager = GetComponent<LevelManager>();
        inputManager = GetComponent<InputManager>();
        actionQueue = GetComponent<ActionQueue>();
        battleManager = GetComponent<BattleManager>();
        audioManager = GetComponent<AudioManager>();
        popManager = GetComponent<PopManager>();
        GameStateMachine = GetComponent<Animator>();
        PathManager = GetComponent<PathManager>();
        CameraManager = GetComponent<CameraManager>();

        InitGame();
    }

    void InitGame()
    {
        // Load the level here
        levelManager.LoadMap();
        levelManager.GetMap().OnCursorMove += CameraManager.CheckCamera;

        foreach (var unit in levelManager.GetMap().TileMap.Units)
        {
            actionQueue.UnitList.Add(unit);
            popManager.RegisterUnit(unit);
        }
    }
}