using Assets.Scripts.GameState;
using UnityEngine;
using Rpg.GameState;
using Rpg.Unit;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject heroReference;
    public GameObject hero2Reference;
    public GameObject activeUnitMenu;

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

        //GameState = new TestGameState();






        InitGame();
    }

    void InitGame()
    {
        // Load units.
        var heroGameObject = Instantiate(heroReference, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        var hero2GameObject = Instantiate(hero2Reference, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

        var hero = heroGameObject.GetComponent<FriendlyUnit>();
        var hero2 = hero2GameObject.GetComponent<FriendlyUnit>();


        actionQueue.UnitList.Add(hero);
        actionQueue.UnitList.Add(hero2);

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
        GameState = new ActiveUnitMenuState(unit);
    }
}