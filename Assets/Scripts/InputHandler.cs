using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	// Use this for initialization
	int[] inputs;
	GridHandler handler;
	bool inGame;
	public Player[] players;
	int numPlayers=0;

	void Start () {
		DontDestroyOnLoad(this);
		players = new Player[4];
		inputs = new int[6];
		for(int k=0;k<6;k++){
			inputs[k] = -1;
		}
		handler = GameObject.Find("SceneSetup").GetComponent<GridHandler>();
		handler.setInput(this);
	}
	
	// Update is called once per frame
	void Update () {

		if(!handler.spawnPlayer){//let players join
			if(Input.GetKeyDown(KeyCode.F)&&inputs[0]==-1&&handler.numPlayers<4){
				inputs[0] = handler.numPlayers;
				players[handler.numPlayers] = handler.predeterminedSpawn(handler.numPlayers);
				handler.numPlayers++;
				numPlayers++;
			}
			if(Input.GetKeyDown(KeyCode.Slash)&&inputs[1]==-1&&handler.numPlayers<4){
				inputs[1] = handler.numPlayers;
				players[handler.numPlayers] = handler.predeterminedSpawn(handler.numPlayers);
				handler.numPlayers++;
				numPlayers++;

			}
			if(Input.GetAxis("p0_Fire")>.5f&&inputs[2]==-1&&handler.numPlayers<4){
				inputs[2] = handler.numPlayers;
				players[handler.numPlayers] = handler.predeterminedSpawn(handler.numPlayers);
				handler.numPlayers++;
				numPlayers++;

			}
			if(Input.GetAxis("p1_Fire")>.5f&&inputs[3]==-1&&handler.numPlayers<4){
				inputs[3] = handler.numPlayers;
				players[handler.numPlayers] = handler.predeterminedSpawn(handler.numPlayers);
				handler.numPlayers++;
				numPlayers++;

			}
			if(Input.GetAxis("p2_Fire")>.5f&&inputs[4]==-1&&handler.numPlayers<4){
				inputs[4] = handler.numPlayers;
				players[handler.numPlayers] = handler.predeterminedSpawn(handler.numPlayers);
				handler.numPlayers++;
				numPlayers++;

			}
			if(Input.GetAxis("p3_Fire")>.5f&&inputs[5]==-1&&handler.numPlayers<4){
				inputs[5] = handler.numPlayers;
				players[handler.numPlayers] = handler.predeterminedSpawn(handler.numPlayers);
				handler.numPlayers++;
				numPlayers++;

			}

		}
		//keyboard wasd
		if(Input.GetKey(KeyCode.W)&&inputs[0]!=-1){
			players[inputs[0]].moveUp();
			players[inputs[0]].lookUp();
		}
		else if(Input.GetKey(KeyCode.A)&&inputs[0]!=-1){
			players[inputs[0]].moveLeft();
			players[inputs[0]].lookLeft();
		}
		else if(Input.GetKey(KeyCode.D)&&inputs[0]!=-1){
			players[inputs[0]].moveRight();
			players[inputs[0]].lookRight();
		}
		else if(Input.GetKey(KeyCode.S)&&inputs[0]!=-1){
			players[inputs[0]].moveDown();
			players[inputs[0]].lookDown();
		}
		if(Input.GetKey(KeyCode.F)&&inputs[0]!=-1){
			players[inputs[0]].shootGun();
		}
		//arrow keys
		if(Input.GetKey(KeyCode.UpArrow)&&inputs[1]!=-1){
			players[inputs[1]].moveUp();
			players[inputs[1]].lookUp();
		}
		else if(Input.GetKey(KeyCode.LeftArrow)&&inputs[1]!=-1){
			players[inputs[1]].moveLeft();
			players[inputs[1]].lookLeft();
		}
		else if(Input.GetKey(KeyCode.RightArrow)&&inputs[1]!=-1){
			players[inputs[1]].moveRight();
			players[inputs[1]].lookRight();
		}
		else if(Input.GetKey(KeyCode.DownArrow)&&inputs[1]!=-1){
			players[inputs[1]].moveDown();
			players[inputs[1]].lookDown();
		}
		if(Input.GetKey(KeyCode.Slash)&&inputs[1]!=-1){
			players[inputs[1]].shootGun();
		}
		//p1controller
		bool storeOpen = false;
		for(int k=0;k<4;k++){
			if (Input.GetAxis ("p"+k+"_MoveY") >= 0.5f&&inputs[k+2]!=-1) {
				players[inputs[k+2]].moveUp();
				players[inputs[k+2]].lookUp();
			}
			else if (Input.GetAxis ("p"+k+"_MoveX") <= -0.5f&&inputs[k+2]!=-1) {
				players[inputs[k+2]].moveLeft();
				players[inputs[k+2]].lookLeft();
			}
			else if (Input.GetAxis ("p"+k+"_MoveX") >= 0.5f&&inputs[k+2]!=-1) {
				players[inputs[k+2]].moveRight();
				players[inputs[k+2]].lookRight();
			}
			else if (Input.GetAxis ("p"+k+"_MoveY") <= -0.5f&&inputs[k+2]!=-1) {
				players[inputs[k+2]].moveDown();
				players[inputs[k+2]].lookDown();
			}

			if (Input.GetAxis ("p"+k+"_AimY") >= 0.5f&&inputs[k+2]!=-1) {
				players[inputs[k+2]].lookUp();
			}
			else if (Input.GetAxis ("p"+k+"_AimX") <= -0.5f&&inputs[k+2]!=-1) {
				players[inputs[k+2]].lookLeft();
			}
			else if (Input.GetAxis ("p"+k+"_AimX") >= 0.5f&&inputs[k+2]!=-1) {
				players[inputs[k+2]].lookRight();
			}
			else if (Input.GetAxis ("p"+k+"_AimY") <= -0.5f&&inputs[k+2]!=-1) {
				players[inputs[k+2]].lookDown();
			}

			if(Input.GetAxis("p"+k+"_Fire")>=.5f&&inputs[k+2]!=-1){
				players[inputs[k+2]].shootGun();
			}

			if(Input.GetAxis("p"+k+"_Start")>=.5f){
				storeOpen = true;
			}

			if (storeOpen) {
				if (GameObject.Find ("Store(Clone)") == null) {
					Instantiate (Resources.Load("Prefabs/Store"),Camera.main.transform.position,Quaternion.identity);
				}
			}
			else {
				if (GameObject.Find ("Store(Clone)") != null) {
					Destroy(GameObject.Find ("Store(Clone)"));
				}
			}
		}


	}

	void OnLevelWasLoaded(int level) {
		handler = GameObject.Find("SceneSetup").GetComponent<GridHandler>();
		handler.numPlayers = numPlayers;
		handler.setInput(this);
	}
}
