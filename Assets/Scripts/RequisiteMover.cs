using UnityEngine;
using System.Collections;

public class RequisiteMover : MonoBehaviour {

	GameObject map;
	GameObject renderer;
	GameObject unlitCamera;
	GameObject litCamera;
	GameObject camera;
	int xSize;
	int ySize;
	// Use this for initialization
	void Awake () {
		map = GameObject.Find("map");
		renderer = GameObject.Find("Unlit Render");
		unlitCamera = GameObject.Find("Unlit Camera");
		litCamera = GameObject.Find("Lit Camera");
		camera = GameObject.Find("Main Camera");
		xSize = (int)map.transform.lossyScale.x;
		ySize = (int)map.transform.lossyScale.y;
		map.transform.localScale = new Vector2(xSize,ySize);
		renderer.transform.localScale = new Vector3 (xSize/10,0,ySize/10);
		unlitCamera.camera.orthographicSize = ySize/2;
		litCamera.camera.orthographicSize = ySize/2;
		camera.camera.orthographicSize = ySize/2;
		transform.position = new Vector2(xSize/2,ySize/2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
