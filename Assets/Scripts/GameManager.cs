using Assets.Scripts.GameState;
using UnityEngine;
using Rpg.GameState;
using Rpg.Unit;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject heroReference;

    [HideInInspector]
    public LevelManager levelManager;
    [HideInInspector]
    public InputManager inputManager;
    [HideInInspector]
    public IFriendlyUnit hero;

    public IGameState gameState;

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

        //gameState = new TestGameState();


        var heroGameObject = Instantiate(heroReference, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

        hero = heroGameObject.GetComponent<FriendlyUnit>();

        gameState = new ActiveUnitMenuState(hero);

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