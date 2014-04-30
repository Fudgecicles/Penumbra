using UnityEngine;
using System.Collections;

public class GunFlash : MonoBehaviour {

	ShadowSystem system;
	// Use this for initialization
	void Start () {
		system = GetComponent<ShadowSystem>();
	
	}
	
	// Update is called once per frame
	void Update () {
		system.intensity -=.01f;
		system.haloRange /= 1.1f;
		if(system.intensity<0){
			Destroy (this.transform.parent.gameObject);
		}
	}
}
