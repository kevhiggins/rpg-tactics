using System;
using Assets.Scripts.GameState;
using Pathfinding.Util;
using UnityEngine;
using Rpg.GameState;
using Rpg.Unit;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject activeUnitMenu;
    public GameObject unitInfoBox;
    public int pixelsToUnits = 100;

    public GameObject[] units;

    [HideInInspector] public LevelManager levelManager;
    [HideInInspector] public InputManager inputManager;
    [HideInInspector] public IActionQueue actionQueue;


    private IGameState gameState;

    public IGameState GameState
    {
        get { return gameState; }
        set
        {
            // Disable old game state, and enable the new one.
            if (gameState != null)
            {
                gameState.Disable();
            }
            
            // Save the new game state, and enable it.
            gameState = value;
            gameState.Enable();
        }
    }

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


        InitGame();
    }

    void InitGame()
    {
        foreach (var unitObject in units)
        {
            var unitInstance = Instantiate(unitObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            var unit = unitInstance.GetComponent<AbstractUnit>();
            if (unit == null)
            {
                throw new Exception("Could not find AbstractUnit script");
            }
            

            actionQueue.UnitList.Add(unit);
        }

        // Load the level here
        levelManager.LoadMap();

        foreach(var unit in actionQueue.UnitList)
        {
            levelManager.GetMap().PlaceUnit(unit, unit.StartPosition);
        }

        WaitForNextAction();
    }

    /// <summary>
    /// Run additiona clock cycles until their is an action to perform.
    /// </summary>
    public void WaitForNextAction()
    {
        IUnit unit;
        do
        {
            actionQueue.ClockTick();
        }
        while ((unit = actionQueue.GetActiveUnit()) == null);

        // At this point we will have an active unit. Select the unit, and prepare for user input.
        unit.StartTurn();

        if (unit is IFriendlyUnit)
        {
            GameState = new ActiveUnitMenuState(unit);
        }
        else
        {
            GameState = new EnemyTurn(unit);
        }
        
    }
}