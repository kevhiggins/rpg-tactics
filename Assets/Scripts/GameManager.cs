using Assets.Scripts.GameState;
using UnityEngine;
using Rpg.GameState;
using Rpg.Unit;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject heroReference;
    public GameObject activeUnitMenu;

    [HideInInspector] public LevelManager levelManager;
    [HideInInspector] public InputManager inputManager;
    [HideInInspector] public IFriendlyUnit hero;

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

        //GameState = new TestGameState();


        var heroGameObject = Instantiate(heroReference, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

        hero = heroGameObject.GetComponent<FriendlyUnit>();

        GameState = new ActiveUnitMenuState(hero);

        InitGame();
    }

    void InitGame()
    {
        // Load the level here
        levelManager.LoadMap();
        levelManager.GetMap().PlaceUnit(hero, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
    }
}