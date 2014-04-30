using UnityEngine;
using System.Collections;

public class TutorialIncrementer : MonoBehaviour {

	tutorialText text;

	// Use this for initialization
	void Start () {
		text = GameObject.Find("SceneSetup").GetComponent<tutorialText>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.tag == "Player"){
			text.spot ++;
			col.collider2D.enabled = false;
		}
	}
}
