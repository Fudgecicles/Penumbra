using UnityEngine;
using System.Collections;

public class LightFlash : MonoBehaviour {
	
	ShadowSystem lightSystem;
	public float haloReduction = 0.1f;
	public float intensityReduction = 0.1f;
	public float randomColorRange = 0.5f;
	public float baseBrightness = 0.5f;
	// Use this for initialization
	void Start () {
		lightSystem = GetComponent<ShadowSystem>();
		lightSystem.tintColor = new Color (baseBrightness + Random.Range (0f, randomColorRange), baseBrightness + Random.Range (0f, randomColorRange), baseBrightness + Random.Range (0f, randomColorRange), 1f);
	}
	
	// Update is called once per frame
	void Update () {
		lightSystem.haloRange /= 1 + haloReduction;
		lightSystem.intensity -= intensityReduction;

		if (lightSystem.intensity <= 0) {
			Destroy (this.transform.parent.gameObject);
		}
	}
}
