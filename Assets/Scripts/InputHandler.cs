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
		}
		else if(Input.GetKey(KeyCode.A)&&inputs[0]!=-1){
			players[inputs[0]].moveLeft();
		}
		else if(Input.GetKey(KeyCode.D)&&inputs[0]!=-1){
			players[inputs[0]].moveRight();
		}
		else if(Input.GetKey(KeyCode.S)&&inputs[0]!=-1){
			players[inputs[0]].moveDown();
		}
		if(Input.GetKey(KeyCode.F)&&inputs[0]!=-1){
			players[inputs[0]].shootGun();
		}
		//arrow keys
		if(Input.GetKey(KeyCode.UpArrow)&&inputs[1]!=-1){
			players[inputs[1]].moveUp();
		}
		else if(Input.GetKey(KeyCode.LeftArrow)&&inputs[1]!=-1){
			players[inputs[1]].moveLeft();
		}
		else if(Input.GetKey(KeyCode.RightArrow)&&inputs[1]!=-1){
			players[inputs[1]].moveRight();
		}
		else if(Input.GetKey(KeyCode.DownArrow)&&inputs[1]!=-1){
			players[inputs[1]].moveDown();
		}
		if(Input.GetKey(KeyCode.Slash)&&inputs[1]!=-1){
			players[inputs[1]].shootGun();
		}
		//p1controller
		for(int k=0;k<4;k++){
			if (Input.GetAxis ("p"+k+"_MoveY") >= 0.5f&&inputs[k+2]!=-1) {
				players[inputs[k+2]].moveUp();
			}
			//		else if (Input.GetKey(KeyCode.LeftArrow)){
			else if (Input.GetAxis ("p"+k+"_MoveX") <= -0.5f&&inputs[k+2]!=-1) {
				players[inputs[k+2]].moveLeft();
			}
			//		else if(Input.GetKey(KeyCode.RightArrow)){
			else if (Input.GetAxis ("p"+k+"_MoveX") >= 0.5f&&inputs[k+2]!=-1) {
				players[inputs[k+2]].moveRight();
			}
			//		else if(Input.GetKey(KeyCode.DownArrow)){
			else if (Input.GetAxis ("p"+k+"_MoveY") <= -0.5f&&inputs[k+2]!=-1) {
				players[inputs[k+2]].moveDown();
			}
			if(Input.GetAxis("p"+k+"_Fire")>=.5f&&inputs[k+2]!=-1){
				players[inputs[k+2]].shootGun();
			}
		}


	}

	void OnLevelWasLoaded(int level) {
		handler = GameObject.Find("SceneSetup").GetComponent<GridHandler>();
		handler.numPlayers = numPlayers;
		handler.setInput(this);
	}
}
