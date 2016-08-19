using UnityEngine;

public class InputManager : MonoBehaviour
{
    private int previousHorizontalDirection;
    private int previousVerticalDirection;

    private bool isHorizontalDown = false;
    private bool isVerticalDown = false;

    // Use this for initialization
    void Start()
    {
    }

    // StateUpdate is called once per frame
    void Update()
    {
        isHorizontalDown = false;
        isVerticalDown = false;

        if (Input.GetButton("Escape"))
        {
            Application.Quit();
        }

//        GameManager.instance.GameState.HandleInput();
    }

    public bool Accept()
    {
        return Input.GetButtonDown("Accept");
    }

    public bool Cancel()
    {
        return Input.GetButtonDown("Cancel");
    }

    public bool Right()
    {
        if (GetHorizontalDown())
        {
            var axisValue = Input.GetAxis("Horizontal");
            return axisValue > 0;
        }
        return false;
    }

    public bool Left()
    {
        if (GetHorizontalDown())
        {
            var axisValue = Input.GetAxis("Horizontal");
            return axisValue < 0;
        }
        return false;
    }

    public bool Up()
    {

        if (GetVerticalDown())
        {
            var axisValue = Input.GetAxis("Vertical");
            return axisValue > 0;
        }
        return false;
    }

    public bool Down()
    {
        if (GetVerticalDown())
        {
            
            var axisValue = Input.GetAxis("Vertical");
            return axisValue < 0;
        }
        return false;
    }


    /**
     * Only returns true the first frame the horizontal axis is down.
     */
    public bool GetHorizontalDown()
    {
        if (isHorizontalDown)
        {
            return true;
        }

        if (Input.GetButton("Horizontal"))
        {
            var direction = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            if (previousHorizontalDirection == 0 || previousHorizontalDirection != direction)
            {
                previousHorizontalDirection = direction;

                // Remember that horizontal was down for the rest of this update frame.
                isHorizontalDown = true;
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
        if (isVerticalDown)
        {
            return true;
        }

        if (Input.GetButton("Vertical"))
        {
            var direction = Input.GetAxis("Vertical") > 0 ? 1 : -1;
            if (previousVerticalDirection == 0 || previousVerticalDirection != direction)
            {
                previousVerticalDirection = direction;
                // Remember that vertical was down for the rest of this update frame.
                isVerticalDown = true;
                return true;
            }
            return false;
        }

        previousVerticalDirection = 0;

        return false;
    }
}