using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [HideInInspector]
    public LevelManager levelManager;
    [HideInInspector]
    public InputManager inputManager;

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
        InitGame();
    }

    void InitGame()
    {
        // Load the level here
        levelManager.LoadMap();
    }

    // Update is called once per frame
    void Update()
    {
    }
}