using UnityEngine;
using System.Collections;

public class MapCenterer : MonoBehaviour {

	int xSize;
	int ySize;
	// Use this for initialization
	void Awake () {
		xSize = (int)transform.lossyScale.x;
		ySize = (int)transform.lossyScale.y;
		transform.position = new Vector2(xSize/2,ySize/2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
