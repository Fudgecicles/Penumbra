using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GridHandler : MonoBehaviour {

	public bool spawnPlayer;
	public int numPlayers;
	public GameObject player;
	private SortedList availableSpawns;
	private List<Player> players;
	private Wall[,] grid;
	public GameObject mapBackground;
	private int xSize;
	private int ySize;
	private int[] lives;

	// Use this for initialization
	void Awake () {
		lives = new int[numPlayers];
		for(int k=0;k<numPlayers;k++){
			lives[k] = 3;
		}
		availableSpawns = new SortedList();
		players = new List<Player>();
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

	void Start(){

		if(spawnPlayer){
			StartCoroutine(initialSpawn());
		}
	}

	// Update is called once per frame
	void Update () {
	}

	IEnumerator initialSpawn(){
		yield return new WaitForEndOfFrame();
		for(int k=0;k<numPlayers;k++){
			availableSpawns = new SortedList();
			getValidSpawns();
			Debug.Log(availableSpawns.Count);
			Vector2 spawn = (Vector2)availableSpawns.GetByIndex(Random.Range(0,availableSpawns.Count));
			GameObject temp = (GameObject)Instantiate(player,gridToWorld(spawn),Quaternion.identity);
			Player p = temp.GetComponent<Player>();
			p.id = k;
			p.gridPosition = spawn;
			players.Add(p);

		}
	}

	public void respawnPlayer(int id){
		print (id);
		lives[id] -= 1;
		if(lives[id]>0){
			availableSpawns = new SortedList();
			getValidSpawns();
			Vector2 spawn = (Vector2)availableSpawns.GetByIndex(Random.Range(0,availableSpawns.Count));
			GameObject temp = (GameObject)Instantiate(player,gridToWorld(spawn),Quaternion.identity);
			Player p = temp.GetComponent<Player>();
			p.id = id;
			p.inputNum = id;
			p.gridPosition = spawn;
			players[id] = p;
		}
	}


	public void getValidSpawns(){
		for(int k=0;k<xSize;k++){
			for(int c = 0; c<ySize;c++){
				if(farEnoughFromPlayers(k,c,xSize*ySize/20)&&canSpawnIn(k,c))
					availableSpawns.Add(vectorToCode(k,c),new Vector2(k,c));
			}
		}
		for(int k=0;k<players.Count;k++){
			if(players[k]!=null){
			removeDirectShots((int)players[k].gridPosition.x,(int)players[k].gridPosition.y);
			Debug.Log(players[k].gridPosition);
			}
		}
	}

	public void removeDirectShots(int x, int y){
		removeRightShots(x,y);
		removeTopShots(x,y);
		removeBotShot(x,y);
		removeLeftShots(x,y);
	}

	public void removeRightShots(int x, int y){
		x+=1;
		while(x<xSize&&canSpawnIn(x,y)){
			Debug.Log(x+" " + y + "right");

			availableSpawns.Remove(vectorToCode(x,y));
			x+=1;
		}
	}

	public void removeTopShots(int x, int y){
		y+=1;
		while(y<ySize&&canSpawnIn(x,y)){
			Debug.Log(x+" " + y + "top");

			availableSpawns.Remove(vectorToCode(x,y));
			y+=1;
		}
	}
	public void removeLeftShots(int x, int y){
		x-=1;
		while(x>=0&&canSpawnIn(x,y)){
			Debug.Log(x+" " + y + "left");
			availableSpawns.Remove(vectorToCode(x,y));
			x-=1;
		}
	}
	public void removeBotShot(int x, int y){
		y-=1;
		while(y>=0&&canSpawnIn(x,y)){
			Debug.Log(x+" " + y + "bot");

			availableSpawns.Remove(vectorToCode(x,y));
			y-=1;
		}
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
	int distanceBetween(int xStart, int yStart, int xEnd, int yEnd){
		int xDif;
		int yDif;
		if(xStart<xEnd){
			xDif = xEnd - xStart;
		}
		else{
			xDif = xStart - xEnd;
		}
		if(yStart<yEnd){
			yDif = yEnd - yStart;
		}
		else{
			yDif = yStart - yEnd;
		}
		return xDif + yDif;
	}

	bool farEnoughFromPlayers(int x, int y,int dist){
		for(int k=0;k<players.Count;k++){
			if(players[k]!=null){
			if(distanceBetween(x,y,(int)players[k].gridPosition.x,(int)players[k].gridPosition.y)<dist)
			   return false;
			}
		}
		return true;
	}

	string vectorToCode(int x, int y){
		string code = x+","+y;
		return code;
	}

	bool canSpawnIn(int x, int y){
		Wall temp = grid[x,y];
		if(temp.isBotOpen())
			return true;
		if (temp.isLeftOpen())
			return true;
		if(temp.isRightOpen())
			return true;
		if(temp.isTopOpen())
			return true;
		return false;
	}

	Vector2 gridToWorld(Vector2 gridPosition){
		return new Vector2(gridPosition.x+.5f,gridPosition.y+.5f);
	}
}
