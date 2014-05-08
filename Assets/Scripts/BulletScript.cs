using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {


	// Use this for initialization
	public float velocity = 5;
	bool destroy;
	float timer;
	ParticleSystem system;
	public float distTraveled;
	Vector3 prevLoc;
	Vector3 curLoc;
	bool richochet;
	GameObject deathParticle;

	void Start () {
		deathParticle = (GameObject)Resources.Load("Prefabs/deathParticle");
		distTraveled = 0;
		prevLoc = transform.position;
		curLoc = transform.position;
		rigidbody2D.velocity = transform.up*velocity;
		destroy = false;
		system = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		if(checkIfGoingToHit()){
//			particleSystem.Stop ();
//			collider2D.enabled = false;
//			destroy = true;
//			timer = Time.time;
//		}
//		if(checkIfGoingToHit()){
//			destroy = true;
//			rigidbody2D.velocity = Vector3.zero;
//			collider2D.enabled = false;
//			timer = Time.time;
//		}


		//calculate how far the bullet has gone, and if it has gone far enough, disable and destroy
		curLoc = transform.position;
		distTraveled += (curLoc - prevLoc).magnitude; 
		if(distTraveled>500){
			//system.Stop ();
//			collider2D.enabled = false;
//			destroy = true;
//			rigidbody2D.velocity = Vector3.zero;
//			timer = Time.time;
			Destroy (this.gameObject);
		}


		//if something was hit, stop everything and wait for particles to fizzle, then die after 5 seconds
		if(destroy){
			if(Time.time-timer>5){
				Destroy(this.gameObject);
			}
		}

	}

	void OnTriggerEnter2D(Collider2D col){
		GameObject hit = col.gameObject;
		//system.Stop ();
		collider2D.enabled = false;
		timer = Time.time;
		rigidbody2D.velocity = Vector3.zero;
		destroy = true;
//		if(hit.tag == "Player" && destroy == false){
//			Instantiate(deathParticle,hit.transform.position,Quaternion.identity);
//			Destroy (hit);
//		}
//		else {
//			destroy = true;
//		}
	}

	bool checkIfGoingToHit(){
		RaycastHit2D hit;
		hit = Physics2D.Raycast(transform.position,rigidbody2D.velocity);
		if(hit!=null){
			Debug.Log ("is this happens");
			transform.position = hit.point;
			return true;
		}
		else
			return false;
	}
}
