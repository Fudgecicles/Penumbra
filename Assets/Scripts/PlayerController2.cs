using UnityEngine;
using System.Collections;

public class PlayerController2 : MonoBehaviour {
	
	bool moving;
	float speed;
	private GridHandler grid;
	GameObject gridParent;
	Vector2 gridPosition;
	Vector2 targetPos;
	float timer;
	Direction dir;
	float fireTime;
	public GameObject leftShot;
	public GameObject rightShot;
	public GameObject topShot;
	public GameObject bottomShot;
	public GameObject bullet;
	Wall lastSpot;
	Animator[] anim;
	bool xMoving;
	bool yMoving;
	public GameObject fireCone;
	bool reloading;
	float reloadTimer;
	enum Direction {up, down, left, right, upRight, upLeft, downLeft, downRight};
	GameObject light;
	Transform gun;
	Vector2 prevPos;
	float distMoved;
	GameObject deathParticle;

	// Use this for initialization
	void Start () {
		deathParticle = (GameObject)Resources.Load("Prefabs/deathParticle");
		prevPos = transform.position;
		gun = transform.FindChild("Gun");
		light = (GameObject)Resources.Load("Prefabs/Lights/GunFlash");
		gridParent = GameObject.Find ("SceneSetup");
		reloading = false;
		anim = GetComponentsInChildren<Animator>();
		fireTime = 0;
		timer = 0;
		speed = 5;
		grid = (GridHandler) gridParent.GetComponentInChildren<GridHandler>();
		int xGridLoc = (int) transform.position.x;
		int yGridLoc = (int) transform.position.y;
		gridPosition = new Vector2(xGridLoc,yGridLoc);
		transform.position = new Vector2(xGridLoc+.5f,yGridLoc+.5f);
		lastSpot = new Wall(true, true, true, true);
		grid.setTaken(xGridLoc,yGridLoc, lastSpot);
		moving = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(grid.isTaken((int)gridPosition.x,(int)gridPosition.y).isBotOpen() + " " +grid.isTaken((int)gridPosition.x,(int)gridPosition.y).isRightOpen() + " " +grid.isTaken((int)gridPosition.x,(int)gridPosition.y).isLeftOpen() + " " +grid.isTaken((int)gridPosition.x,(int)gridPosition.y).isTopOpen());
		if(Time.time-reloadTimer >2){
			reloading = false;
		}
		//inputs
		
		if(Input.GetKey (KeyCode.W)){
			dir = Direction.up;
			anim[0].SetInteger("dir",(int)dir);
			anim[1].SetInteger("dir",(int)dir);
			if(!moving){
				if(checkIfCanMove(gridPosition,Direction.up)){
					setMovement (Direction.up,new Vector2(gridPosition.x,gridPosition.y+1));
					yMoving = true;
				}
			}
		}
		else if (Input.GetKey(KeyCode.A)){
			dir = Direction.left;
			anim[0].SetInteger("dir",(int)dir);
			anim[1].SetInteger("dir",(int)dir);
			if(!moving){
				if(checkIfCanMove(gridPosition,Direction.left)){
					setMovement (Direction.left,new Vector2(gridPosition.x-1,gridPosition.y));
					xMoving = true;
				}				
			}
		}
		else if(Input.GetKey(KeyCode.D)){
			dir = Direction.right;
			anim[0].SetInteger("dir",(int)dir);
			anim[1].SetInteger("dir",(int)dir);
			if(!moving){
				if(checkIfCanMove(gridPosition,Direction.right)){
					setMovement (Direction.right,new Vector2(gridPosition.x+1,gridPosition.y));
					xMoving = true;
				}
			}
		}
		else if(Input.GetKey(KeyCode.S)){
			dir = Direction.down;
			anim[0].SetInteger("dir",(int)dir);
			anim[1].SetInteger("dir",(int)dir);
			if(!moving){
				if(checkIfCanMove(gridPosition,Direction.down)){
					setMovement (Direction.down,new Vector2(gridPosition.x,gridPosition.y-1));
					yMoving = true;
				}
			}
		}
		
		//movement
		if(moving){
			if(!reloading){
				timer += Time.deltaTime;
				transform.Translate((new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.y).normalized*speed*Time.deltaTime));
			}
			else{
				timer += Time.deltaTime*.25f;
				transform.Translate((new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.y).normalized*speed*Time.deltaTime*.25f));
			}
			if(timer > 1/speed){
				moving  = false;
				transform.position = targetPos;
				timer = 0;
				anim[2].SetBool("Moving",false);
			}
		}
		
		// fire gun
		if(Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.C)){
			fireGun();
		}
	}
	
	void setMovement(Direction curDir,Vector2 destination){
		anim[2].SetInteger("dir",(int)dir);
		anim[2].SetBool("Moving",true);
		grid.setTaken((int)gridPosition.x,(int)gridPosition.y, lastSpot);
		gridPosition = destination;
		lastSpot = grid.isTaken((int)gridPosition.x, (int)gridPosition.y);
		grid.setTaken((int)gridPosition.x,(int)gridPosition.y, new Wall(false, false, false, false));
		targetPos = gridToWorld ();
		moving = true;
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.tag == "Bullet"){
			Instantiate(deathParticle,transform.position,Quaternion.identity);
			Destroy(this.gameObject);
		}
	}

	void fireGun(){
		if(Time.time - fireTime >2){
			audio.Play();
			reloading = true;
			reloadTimer = Time.time;
			fireTime = Time.time;
			if(dir == Direction.up){
				Instantiate(light,new Vector3(topShot.transform.position.x,topShot.transform.position.y,topShot.transform.position.z+Random.Range(0,5)),Quaternion.identity);
				Instantiate(bullet,topShot.transform.position,Quaternion.Euler(0,0,15));
				Instantiate(bullet,topShot.transform.position,Quaternion.Euler(0,0,7.5f));
				Instantiate(bullet,topShot.transform.position,Quaternion.Euler(0,0,-7.5f));
				Instantiate(bullet,topShot.transform.position,Quaternion.Euler(0,0,-15));
				Instantiate(bullet,topShot.transform.position,Quaternion.identity);
				Instantiate(fireCone,topShot.transform.position,Quaternion.identity);
			}
			if(dir == Direction.left){
				Instantiate(light,new Vector3(leftShot.transform.position.x,leftShot.transform.position.y,leftShot.transform.position.z+Random.Range(0,5)),Quaternion.identity);
				GameObject middle = (GameObject) Instantiate(bullet,leftShot.transform.position,Quaternion.Euler(0,0,105));
				GameObject farRight = (GameObject) Instantiate(bullet,leftShot.transform.position,Quaternion.Euler(0,0,97.5f));
				GameObject farLeft = (GameObject) Instantiate(bullet,leftShot.transform.position,Quaternion.Euler(0,0,82.5f));
				GameObject slightLeft = (GameObject) Instantiate(bullet,leftShot.transform.position,Quaternion.Euler(0,0,75));
				GameObject slightRight = (GameObject) Instantiate(bullet,leftShot.transform.position,Quaternion.Euler(0,0,90));
				Instantiate(fireCone,leftShot.transform.position,Quaternion.Euler(0,0,90));
				
			}
			if(dir == Direction.right){
				Instantiate(light,new Vector3(rightShot.transform.position.x,rightShot.transform.position.y,rightShot.transform.position.z+Random.Range(0,5)),Quaternion.identity);
				GameObject middle = (GameObject) Instantiate(bullet,rightShot.transform.position,Quaternion.Euler(0,0,-105));
				GameObject farRight = (GameObject) Instantiate(bullet,rightShot.transform.position,Quaternion.Euler(0,0,-97.5f));
				GameObject farLeft = (GameObject) Instantiate(bullet,rightShot.transform.position,Quaternion.Euler(0,0,-82.5f));
				GameObject slightLeft = (GameObject) Instantiate(bullet,rightShot.transform.position,Quaternion.Euler(0,0,-75));
				GameObject slightRight = (GameObject) Instantiate(bullet,rightShot.transform.position,Quaternion.Euler(0,0,-90));
				Instantiate(fireCone,rightShot.transform.position,Quaternion.Euler(0,0,-90));
				
			}
			if(dir == Direction.down){
				Instantiate(light,new Vector3(bottomShot.transform.position.x,bottomShot.transform.position.y,bottomShot.transform.position.z+Random.Range(0,5)),Quaternion.identity);
				GameObject middle = (GameObject) Instantiate(bullet,bottomShot.transform.position,Quaternion.Euler(0,0,195));
				GameObject farRight = (GameObject) Instantiate(bullet,bottomShot.transform.position,Quaternion.Euler(0,0,187.5f));
				GameObject farLeft = (GameObject) Instantiate(bullet,bottomShot.transform.position,Quaternion.Euler(0,0,172.5f));
				GameObject slightLeft = (GameObject) Instantiate(bullet,bottomShot.transform.position,Quaternion.Euler(0,0,165));
				GameObject slightRight = (GameObject) Instantiate(bullet,bottomShot.transform.position,Quaternion.Euler(0,0,180));
				Instantiate(fireCone,bottomShot.transform.position,Quaternion.Euler(0,0,-180));
				
			}
		}
	}
	
	bool checkIfCanMove(Vector2 start,Direction dir){
		if(dir == Direction.up){
			if(gridPosition.y+1 < grid.getYSize ())
				if(grid.isTaken((int)start.x, (int)start.y +1).isBotOpen()&& lastSpot.isTopOpen())
					return true;
		}
		else if(dir == Direction.left){
			if(gridPosition.x-1 >= 0)
				if(grid.isTaken((int)start.x-1, (int)start.y).isRightOpen()&& lastSpot.isLeftOpen())
					return true;
		}
		else if (dir == Direction.right){
			if(gridPosition.x+1< grid.getXSize())
				if(grid.isTaken((int)start.x+1, (int)start.y).isLeftOpen()&& lastSpot.isRightOpen())
					return true;
		}
		else if(dir == Direction.down){
			if( gridPosition.y - 1 >=0)
				if(grid.isTaken((int)start.x, (int)start.y-1).isTopOpen()&& lastSpot.isBotOpen())
					return true;
		}
		
		
		return false;			
	}
	
	Vector2 gridToWorld(){
		return new Vector2(gridPosition.x+.5f,gridPosition.y+.5f);
	}
	
}
