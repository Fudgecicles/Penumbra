using UnityEngine;
using System.Collections;

public class GridLoc : MonoBehaviour {

	private int xGridLoc;
	private int yGridLoc;
	private Vector2 gridPosition;
	private GridHandler grid;
	GameObject gridParent;
	public bool topOpen;
	public bool leftOpen;
	public bool rightOpen;
	public bool botOpen;

	// Use this for initialization
	void Start () {
		gridParent = GameObject.Find ("SceneSetup");
		grid = (GridHandler) gridParent.GetComponentInChildren<GridHandler>();
		int xGridLoc = (int) transform.position.x;
		int yGridLoc = (int) transform.position.y;
		gridPosition = new Vector2(xGridLoc,yGridLoc);
		transform.position = new Vector2(xGridLoc+.5f,yGridLoc+.5f);
		grid.setTaken(xGridLoc,yGridLoc,new Wall(topOpen, botOpen,leftOpen,rightOpen));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
