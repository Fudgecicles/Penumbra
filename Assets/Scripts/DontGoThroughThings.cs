using UnityEngine;
using System.Collections;

public class DontGoThroughThings : MonoBehaviour 
{
	public LayerMask layerMask; //make sure we aren't in this layer 
	public float skinWidth = 0.1f; //probably doesn't need to be changed 
	
	private float minimumExtent; 
	private float partialExtent; 
	private float sqrMinimumExtent; 
	private Vector3 previousPosition; 
	private Rigidbody2D myRigidbody; 
	
	
	//initialize values 
	void Awake() 
	{ 
		myRigidbody = rigidbody2D; 
		previousPosition = transform.position; 
		BoxCollider2D col = GetComponent<BoxCollider2D>();
		minimumExtent =Mathf.Min(col.size.x/2, col.size.y/2); 
		partialExtent = minimumExtent * (1.0f - skinWidth); 
		sqrMinimumExtent = minimumExtent * minimumExtent; 
	} 
	
	void FixedUpdate() 
	{ 
		//have we moved more than our minimum extent? 
		Vector3 movementThisStep = transform.position - previousPosition; 
		float movementSqrMagnitude = movementThisStep.sqrMagnitude;
		
		if (movementSqrMagnitude > sqrMinimumExtent) 
		{ 
			float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
			RaycastHit hitInfo; 
			
			//check for obstructions we might have missed 
			if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value)) 
				transform.position = hitInfo.point - (movementThisStep/movementMagnitude)*partialExtent; 
		} 
		
		previousPosition = transform.position; 
	}
}