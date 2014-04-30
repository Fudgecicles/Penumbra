using UnityEngine;
using System.Collections;

public class ParticleCleanup : MonoBehaviour {

	float deathTimer;
	// Use this for initialization
	void Start () {
		deathTimer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - deathTimer>3)
			Destroy(this.gameObject);
	}
}
