using UnityEngine;
using System.Collections;

public class FlashSpawner : MonoBehaviour {

	public float timerLength = 100f;
	public float randomPositiveXOffset = 10f;
	public float randomNegativeXOffset = 10f;
	public float randomPositiveYOffset = 3f;
	public float randomNegativeYOffset = 3f;
	public float baseXOffset = 0f;
	public float baseYOffset = 4f;
	float timer = 0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (timer >= timerLength) {
			Instantiate(Resources.Load("PreFabs/Lights/TitleFlash"), new Vector3(baseXOffset + transform.position.x + Random.Range(-randomNegativeXOffset, randomPositiveXOffset), baseYOffset + transform.position.y + Random.Range(-randomNegativeYOffset, randomPositiveYOffset), -5f), Quaternion.identity);
			timer -= timerLength;
		}
		timer++;
	}
}
