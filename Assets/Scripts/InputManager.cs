using UnityEditor;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private int previousHorizontalDirection;
    private int previousVerticalDirection;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var levelManager = GameManager.instance.levelManager;

        // Move the tile cursor depending on axis position
        if (GetHorizontalDown())
        {
            var axisValue = Input.GetAxis("Horizontal");
            levelManager.MoveTileCursor(axisValue > 0 ? 1 : -1, 0);
        }
        else if (GetVerticalDown())
        {
            var axisValue = Input.GetAxis("Vertical");
            levelManager.MoveTileCursor(0, axisValue > 0 ? 1 : -1);
        }

        if (Input.GetButton("Cancel"))
        {
            Application.Quit();
        }
    }

    /**
     * Only returns true the first frame the horizontal axis is down.
     */
    public bool GetHorizontalDown()
    {
        if (Input.GetButton("Horizontal"))
        {
            var direction = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            if (previousHorizontalDirection == 0 || previousHorizontalDirection != direction)
            {
                previousHorizontalDirection = direction;
                return true;
            }
            return false;
        }

        previousHorizontalDirection = 0;

        return false;
    }

    /**
     * Only returns true the first frame the vertical axis is down.
     */
    public bool GetVerticalDown()
    {
        if (Input.GetButton("Vertical"))
        {
            var direction = Input.GetAxis("Vertical") > 0 ? 1 : -1;
            if (previousVerticalDirection == 0 || previousVerticalDirection != direction)
            {
                previousVerticalDirection = direction;
                return true;
            }
            return false;
        }

        previousVerticalDirection = 0;

        return false;
    }
}