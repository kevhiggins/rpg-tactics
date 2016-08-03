using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject heroReference;

    [HideInInspector]
    public LevelManager levelManager;
    [HideInInspector]
    public InputManager inputManager;

    private GameObject hero;

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

        hero = Instantiate(heroReference, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

        InitGame();
    }

    void InitGame()
    {
        // Load the level here
        levelManager.LoadMap();
        levelManager.PlaceHero(hero, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
    }
}