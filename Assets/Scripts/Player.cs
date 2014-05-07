using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int id;
	bool moving;
	float speed;
	private GridHandler grid;
	GameObject gridParent;
	public Vector2 gridPosition;
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
	public bool reloading;
	public float reloadTimer;
	enum Direction {up, down, left, right, upRight, upLeft, downLeft, downRight};
	GameObject light;
	Transform gun;
	Vector2 prevPos;
	float distMoved;
	GameObject deathParticle;
	Color[] colors;
	AudioSource[] sources;
	bool fire;
	
	// Use this for initialization
	void Start () {
		sources = GetComponents<AudioSource>();
		colors = new Color[4];
		colors[0] = new Color(0,0,0);
		colors[1] = new Color(.89f, .086f,.086f);
		colors[2] = new Color(1,0,.086f);
		colors[3] = new Color(1,.565f,.565f);
		deathParticle =  (GameObject)Resources.Load("Prefabs/deathParticle");
		prevPos = transform.position;
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
			if(fire){
				reloadTimer = Time.time;
				StartCoroutine("startFire");
				fire = false;
			}
		}

		//inputs
		

		
		//movement
		if(moving){
			if(!reloading){
				timer += Time.deltaTime;
				transform.Translate((new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.y).normalized*speed*Time.deltaTime));
			}
			else{
				timer += Time.deltaTime*.5f;
				transform.Translate((new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.y).normalized*speed*Time.deltaTime*.5f));
			}
			if(timer > 1/speed){
				moving  = false;
				transform.position = targetPos;
				timer = 0;
				anim[2].SetBool("Moving",false);
			}
		}
		
		// fire gun
	}

	public void shootGun(){
		if(Time.time-reloadTimer>2){
			fire = true;
			reloading = true;
		}
	}
	public void setID(int num){
		id = num;
		Debug.Log(id);
		gun = transform.Find("Dude/Gun");
		colors = new Color[4];
		colors[0] = new Color(1,1,1);
		colors[1] = new Color(.89f, .086f,.086f);
		colors[2] = new Color(.365f,0,1);
		colors[3] = new Color(1,.565f,.565f);
		gun.GetComponent<SpriteRenderer>().color = colors[id];
	}

	public void moveUp(){

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

	public void moveLeft(){
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

	public void moveRight(){
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

	public void moveDown(){
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

	void OnTriggerEnter2D(Collider2D col){
		if(col.tag == "Bullet"){
			Instantiate(deathParticle,transform.position,Quaternion.identity);
			grid.setTaken((int)gridPosition.x,(int)gridPosition.y,lastSpot);
			grid.respawnPlayer(id);
			Destroy(this.gameObject);
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

	IEnumerator startFire(){
		sources[0].Play();
		yield return new WaitForSeconds(.5f);
		fireGun();
		yield return new WaitForSeconds(.2f);
		sources[2].Play ();
	}

	void fireGun(){
		if(Time.time - fireTime >2){
			sources[1].Play();
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
