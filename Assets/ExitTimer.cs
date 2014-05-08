using UnityEngine;
using System.Collections;

public class ExitTimer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine ("changeScreens"); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator changeScreens(){
		yield return new WaitForSeconds(3f);
		Application.LoadLevel (3);
	}
}
