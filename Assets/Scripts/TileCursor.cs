using UnityEngine;
using System.Collections;

public class TileCursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // TODO Define a global variable for tile width
        const int TILE_WIDTH = 32;
        const int TILE_HEIGHT = 32;

        var mapTileWidth = 10;
        var mapTileHeight = 10;
                
        var newPosition = gameObject.transform.position;
        if (Input.GetKeyDown("right"))
        {
            newPosition.x += TILE_WIDTH;
        }
        else if(Input.GetKeyDown("left"))
        {
            newPosition.x -= TILE_WIDTH;
        }
        else if (Input.GetKeyDown("up"))
        {
            newPosition.y += TILE_HEIGHT;
        }
        else if (Input.GetKeyDown("down"))
        {
            newPosition.y -= TILE_HEIGHT;
        }

        if((newPosition.x > mapTileWidth * TILE_WIDTH) || newPosition.x < 0)
        {
            newPosition.x = gameObject.transform.position.x;
        }

        if((newPosition.y < -(mapTileHeight * TILE_HEIGHT)) || newPosition.y > 0)
        {
            newPosition.y = gameObject.transform.position.y;
        }

        gameObject.transform.position = newPosition;
    }
}
