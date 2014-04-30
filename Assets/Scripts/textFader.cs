using UnityEngine;
using System.Collections;

public class textFader : MonoBehaviour {

	Material mat;
	bool fadedIn;
	bool fadedOut;
	// Use this for initialization
	void Start () {
		fadedIn = false;
		fadedOut =false;
		mat = renderer.material;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void fadeIn(){
		if(!fadedIn){
			Color col = mat.color;
			mat.color = new Color(col.r,col.g,col.b, col.a+.01f);
			if(mat.color.a >=1){
				fadedIn = true;
			}
		}
	}

	public void fadeOut(){
		if(!fadedOut){
			Color col = mat.color;
			mat.color = new Color(col.r,col.g,col.b, col.a-.01f);
			if(mat.color.a<=0){
				fadedOut = true;
			}
		}
	}
}
