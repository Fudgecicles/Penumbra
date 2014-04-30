using UnityEngine;
using System.Collections;

public class Wall{

	bool topOpen;
	bool botOpen;
	bool leftOpen; 
	bool rightOpen;

	public Wall(){
		topOpen = true;
		botOpen = true;
		leftOpen = true;
		rightOpen = true;
	}

	public Wall(bool top, bool bot, bool left, bool right){
		topOpen = top;
		botOpen = bot;
		leftOpen = left;
		rightOpen = right;
	}
	
	public bool isTopOpen(){
		return topOpen;
	}

	public bool isLeftOpen(){
		return leftOpen;
	}

	public bool isRightOpen(){
		return rightOpen;
	}

	public bool isBotOpen(){
		return botOpen;
	}
}
