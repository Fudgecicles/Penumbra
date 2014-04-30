using UnityEngine;
using System.Collections;

public class GridHandler : MonoBehaviour {

	private Wall[,] grid;
	public GameObject mapBackground;
	private int xSize;
	private int ySize;
	// Use this for initialization
	void Awake () {
		xSize = (int) mapBackground.renderer.bounds.size.x;
		ySize = (int) mapBackground.renderer.bounds.size.y;
		grid = new Wall[xSize,ySize];
		for(int k=0;k<xSize;k++){
			for(int c = 0;c<ySize;c++){
				grid[k,c] = new Wall();
			}
		}
		transform.position = new Vector3(xSize/2,ySize/2,-10);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Wall[,] getGrid(){
		return grid;
	}

	public int getXSize(){
		return xSize;
	}

	public int getYSize(){
		return ySize;
	}

	public void setTaken(int x, int y, Wall spot){
		grid[x,y] = spot;
	}

	public Wall isTaken(int x, int y){
		return grid[x,y];
	}
}
