using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    public GameObject tileSelection;


	// Use this for initialization
	void Start () {
        // Create the cursor at 16px and 16px
        GameObject selection = Instantiate(tileSelection, new Vector3(16, -16, 0), Quaternion.identity) as GameObject;

        // Potentially do something like this to pass map data to selection scripts
		//selection.SendMessage("InitMap", currentMap);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
