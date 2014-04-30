using UnityEngine;
using System.Collections;

public class GridMover : MonoBehaviour {
	
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
		int xGridLoc = (int) transform.position.x;
		int yGridLoc = (int) transform.position.y;
		gridPosition = new Vector2(xGridLoc,yGridLoc);
		transform.position = new Vector3(xGridLoc+.5f,yGridLoc+.5f, -5.0f -(float)Random.Range(0f,5f));
	}
	
	// Update is called once per frame
	void Update () {

	}
	
}
