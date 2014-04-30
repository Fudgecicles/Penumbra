using UnityEngine;
using System.Collections;

public class tutorialText : MonoBehaviour {

	public int spot;
	public GameObject t1;
	public GameObject t2;
	public GameObject t3;
	public GameObject t4;
	public GameObject t5;
	public GameObject t6;
	textFader textOne;
	textFader textTwo;
	textFader textThree;
	textFader textFour;
	textFader textFive;
	textFader textSix;

	
	// Use this for initialization
	void Start () {
		spot = 0;
		textOne = t1.GetComponent<textFader>();
		textTwo = t2.GetComponent<textFader>();
		textThree = t3.GetComponent<textFader>();
		textFour = t4.GetComponent<textFader>();
		textFive = t5.GetComponent<textFader>();
		textSix = t6.GetComponent<textFader>();
	}
	
	// Update is called once per frame
	void Update () {
		if(spot ==1){
			textTwo.fadeIn();
			textOne.fadeOut();
		}
		else if(spot ==2){
			textThree.fadeIn();
			textTwo.fadeOut();
		}
		else if(spot ==3){
			textFour.fadeIn();
			textThree.fadeOut();
		}
		else if(spot ==4){
			textFive.fadeIn();
			textFour.fadeOut();
		}
		else if(spot ==5){
			textSix.fadeIn();
			textFive.fadeOut();
		}
	}
}
