using UnityEngine;
using System.Collections;

public class HueShift : MonoBehaviour {

	public Color myStartColor;
	public float myHue = 0;
	public float myHueShiftSpeed = 0.06f;
	public float baseHaloWhiteness = 0f;
	public float baseParticleWhiteness = 0.6f;
	public bool randomColor = false;
	// Use this for initialization
	void Start () {
		if (randomColor)
			myHue = Random.Range (0f, Mathf.PI);
//		myHueShiftSpeed = Random.Range (0.04f, 0.06f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		baseParticleWhiteness = 0.008f * rigidbody2D.velocity.magnitude;
		light.color = new Color (baseHaloWhiteness + (Mathf.Sin (myHue) + 1f) / 2f, baseHaloWhiteness + (Mathf.Sin ((2f/3f) * Mathf.PI + myHue) + 1f) / 2f, baseHaloWhiteness + (Mathf.Sin ((4f/3f) * Mathf.PI +myHue) + 1f) / 2f, 1f);
		//if (rigidbody2D.velocity.magnitude > 0.001f) {
		particleSystem.startColor = new Color (baseParticleWhiteness + myStartColor.r * (Mathf.Sin (myHue) + 1f) / 2f, baseParticleWhiteness + myStartColor.g * (Mathf.Sin ((2f/3f) * Mathf.PI + myHue) + 1f) / 2f, baseParticleWhiteness + myStartColor.b * (Mathf.Sin ((4f/3f) * Mathf.PI +myHue) + 1f) / 2f, 1f);
		myHue += myHueShiftSpeed * rigidbody2D.velocity.magnitude;
		//}
	}
}
