using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	[HideInInspector] public int[] inputAssignment = new int[4] {-1, -1, -1, -1};
	int[] inputAssignmentOpposite = new int[8] {-1, -1, -1, -1, -1, -1, -1, -1};
	public string[] inputAssignmentName = new string[8] {"Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", };
	bool[] playerAssigned = new bool[8] {false, false, false, false, false, false, false, false};
	[HideInInspector] public int playerCounter = 0;
	public bool inputSet = false;
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		//Screen.lockCursor = true;
		reset ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!inputSet) {
			for (int i=0; i<7; i++) {
				int myNumber = i + 1;
				if (Input.GetAxis ("p" + myNumber + "_Fire") == 1.0f && !playerAssigned [i]) {
					inputAssignment [playerCounter] = i;
					inputAssignmentOpposite [i] = playerCounter;
					if (myNumber <= 4)
						inputAssignmentName[playerCounter] = "Controller " + myNumber;
					else if (myNumber == 5)
						inputAssignmentName[playerCounter] = "Mouse";
					else if (myNumber == 6)
						inputAssignmentName[playerCounter] = "Keyboard (WASD + F)";
					else if (myNumber == 7)
						inputAssignmentName[playerCounter] = "Keyboard (Arrow Keys + /)";
					else
						inputAssignmentName[playerCounter] = "Unknown Input";
					playerCounter++;
					playerAssigned [i] = true;
					print ("Player " + (playerCounter + 1) + " assigned to input " + (i + 1));
				}
			}
		}
	}
	
	public void reset() {
		inputAssignment = new int[8] {-1, -1, -1, -1, -1, -1, -1, -1};
		inputAssignmentOpposite = new int[8] {-1, -1, -1, -1, -1, -1, -1, -1};
		inputAssignmentName = new string[8] {"Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", };
		playerAssigned = new bool[] {false, false, false, false, false, false, false, false};
		playerCounter = 0;
		inputSet = false;
		print ("Reset");
	}
}
