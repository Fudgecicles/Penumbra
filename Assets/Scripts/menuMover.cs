using UnityEngine;
using System.Collections;

public class menuMover : MonoBehaviour {

	public GameObject moverOne;
	public GameObject moverTwo;
	public GameObject moverThree;
	public GameObject moverFour;
	public GameObject moverFive;
	public GameObject moverSix;
	public GameObject moverZero;
	public GameObject moverNeg;



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		moverOne.transform.Translate(Vector3.right*Time.deltaTime);
		moverTwo.transform.Translate(Vector3.right*Time.deltaTime);
		moverThree.transform.Translate(Vector3.right*Time.deltaTime);
		moverFour.transform.Translate(Vector3.right*Time.deltaTime);
		moverFive.transform.Translate(Vector3.right*Time.deltaTime);
		moverSix.transform.Translate(Vector3.right*Time.deltaTime);
		moverZero.transform.Translate(Vector3.right*Time.deltaTime);
		moverNeg.transform.Translate(Vector3.right*Time.deltaTime);

		if(moverOne.transform.position.x>25){
			moverOne.transform.position = new Vector2(-15,moverOne.transform.position.y);
		}
		if(moverTwo.transform.position.x>25){
			moverTwo.transform.position = new Vector2(-15,moverTwo.transform.position.y);
		}
		if(moverThree.transform.position.x>25){
			moverThree.transform.position = new Vector2(-15,moverThree.transform.position.y);
		}
		if(moverFour.transform.position.x>25){
			moverFour.transform.position = new Vector2(-15,moverFour.transform.position.y);
		}
		if(moverFive.transform.position.x>25){
			moverFive.transform.position = new Vector2(-15,moverFive.transform.position.y);
		}
		if(moverSix.transform.position.x>25){
			moverSix.transform.position = new Vector2(-15,moverSix.transform.position.y);
		}
		if(moverZero.transform.position.x>25){
			moverZero.transform.position = new Vector2(-15,moverZero.transform.position.y);
		}
		if(moverNeg.transform.position.x>25){
			moverNeg.transform.position = new Vector2(-15,moverNeg.transform.position.y);
		}
	}
}
