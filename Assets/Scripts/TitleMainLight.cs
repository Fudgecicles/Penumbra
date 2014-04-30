using UnityEngine;
using System.Collections;

public class TitleMainLight : MonoBehaviour {
	
	ShadowSystem lightSystem;
	public float baseBrightness = 0.9f;
	public float pulseRate = 0.01f;
	private float pulseTimer = 0f;
	public float pulseBrightness = 1.0f;
	// Use this for initialization
	void Start () {
		lightSystem = GetComponent<ShadowSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		pulseTimer += pulseRate;
		lightSystem.intensity = baseBrightness + pulseBrightness * Mathf.Sin (pulseTimer);
	}
}
