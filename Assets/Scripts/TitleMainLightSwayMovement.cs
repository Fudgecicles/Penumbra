using UnityEngine;
using System.Collections;

public class TitleMainLightSwayMovement : MonoBehaviour {
	
	ShadowSystem lightSystem;
	public float baseXOffset = 0.0f;
	public float baseYOffset = 0.0f;
	public float swayXRate = 0.005f;
	public float swayYRate = 0.008f;
	private float swayXTimer = 0f;
	private float swayYTimer = 0f;
	public float swayXOffset = 3.0f;
	public float swayYOffset = 3.0f;
	private Vector3 originalPosition;
	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		swayXTimer += swayXRate;
		swayYTimer += swayYRate;
		transform.position = originalPosition + new Vector3 (baseXOffset + swayXOffset * Mathf.Cos (swayXTimer), baseYOffset + swayYOffset * Mathf.Sin (swayYTimer), 0f);
	}
}
