using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowSystem : MonoBehaviour
{
	
	
	// How dark a color has to be for it to count as a shadow
	[SerializeField]
	private float minLuminance = 0.01f;
	[SerializeField]
	private float shadowBias = 0.001f;
	[SerializeField]
	public float haloRange = 0.02f;
	[SerializeField]
	public float intensity = 1f;
	[SerializeField]
	public Color tintColor = new Color (1, 1, 1, 1);
	[SerializeField]
	private Camera shadowCamera;
	private Vector2 defaultOffset;
	[SerializeField]
	private bool highQuality = false;
	[SerializeField]
	private Shader lightDistanceShader;
	[SerializeField]
	private int shadowMapSize = 512;
	//
	private GameObject myMap;
	private RenderTexture _texShadowTexture;
	private RenderTexture _texTarget;
	private Dictionary<Shader, Material> _shaderMap = new Dictionary<Shader, Material> ();
	private List<RenderTexture> _tempRenderTextures = new List<RenderTexture> ();

	private Material GetMaterial (Shader shader)
	{
		Material material;
		if (_shaderMap.TryGetValue (shader, out material)) {
			return material;
		} else {
			material = new Material (shader);
			_shaderMap.Add (shader, material);
			return material;
		}
	}
	void Start() {
		myMap = GameObject.Find ("map");
		renderer.material.SetColor ("_TintColor", tintColor);
		transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);

		renderer.material.SetTextureScale ("_Alpha", new Vector2 ((Camera.main.orthographicSize / shadowCamera.orthographicSize), (Camera.main.orthographicSize / shadowCamera.orthographicSize)));
		renderer.material.SetTextureOffset ("_Alpha", new Vector2 (0.5f - 0.025f * shadowCamera.transform.position.x, 0.5f - 0.025f * shadowCamera.transform.position.y));
		transform.localScale = new Vector3 (myMap.transform.localScale.x / 10, 1, myMap.transform.localScale.y / 10);
	}
	private void OnEnable ()
	{
		_texShadowTexture = new RenderTexture (shadowMapSize, shadowMapSize, 16, RenderTextureFormat.Default);
		_texTarget = new RenderTexture (shadowMapSize, shadowMapSize, 0, RenderTextureFormat.Default);

		shadowCamera.targetTexture = _texShadowTexture;
		// hack to fix broken aspect
		shadowCamera.rect = new Rect (0, 0, 1, 1);

		// Match plane to orthographic size
		//transform.localScale = Vector3.one * shadowCamera.orthographicSize / 5;
		//transform.localScale = Vector3.one;
		//renderer.material.mainTexture = _texTarget;
		//renderer.material.SetTextureOffset("_Alpha", new Vector2((transform.position.x - shadowCamera.transform.position.x) / 2, (transform.position.z - shadowCamera.transform.position.z) / 2));
		renderer.material.SetTexture ("_Alpha", _texTarget);
		renderer.material.SetTexture ("_MainTex", Camera.main.targetTexture);
		//renderer.material.SetTexture ("_MainTex", shadowCamera.targetTexture);
		//renderer.material.SetTextureOffset ("_MainTex", new Vector2 (-shadowCamera.transform.position.x/2, -shadowCamera.transform.position.y/2));

		shadowMapSize = Mathf.NextPowerOfTwo (shadowMapSize);
		shadowMapSize = Mathf.Clamp (shadowMapSize, 8, 2048);
	}

	private void Update ()
	{
		//defaultOffset = new Vector2 ((10 - (shadowCamera.orthographicSize % 10)) / 20, (10 - (shadowCamera.orthographicSize % 20)) / 20);
		//renderer.material.SetTextureScale ("_MainTex", new Vector2(1, 1));
		//renderer.material.SetTextureOffset ("_MainTex", defaultOffset + new Vector2 ((shadowCamera.transform.position.x-Camera.main.transform.position.x)/10, (shadowCamera.transform.position.y-Camera.main.transform.position.y)/10));
		renderer.material.SetColor ("_TintColor", tintColor);

		transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);

		//renderer.material.SetTextureScale ("_MainTex", new Vector2 (transform.localScale.x, transform.localScale.z));
		renderer.material.SetTextureScale ("_Alpha", new Vector2 ((Camera.main.orthographicSize / shadowCamera.orthographicSize), (Camera.main.orthographicSize / shadowCamera.orthographicSize)));
		renderer.material.SetTextureOffset ("_Alpha", new Vector2 (0.5f - 0.025f * shadowCamera.transform.position.x, 0.5f - 0.025f * shadowCamera.transform.position.y));
		/*
		transform.position = new Vector3(0,0,transform.position.z);
		defaultOffset = new Vector2 ((10 - (shadowCamera.orthographicSize % 20)) / 20, (10 - (shadowCamera.orthographicSize % 20)) / 20);
		renderer.material.SetTextureScale ("_MainTex", new Vector2(shadowCamera.orthographicSize/10, shadowCamera.orthographicSize/10));
		renderer.material.SetTextureOffset ("_MainTex", defaultOffset);
		renderer.material.SetTextureOffset ("_Alpha", -new Vector2 ((shadowCamera.transform.position.x-Camera.main.transform.position.x)/shadowCamera.orthographicSize/2, (shadowCamera.transform.position.y-Camera.main.transform.position.y)/shadowCamera.orthographicSize/2));
		*/
	}

	private void OnDestroy ()
	{
		foreach (KeyValuePair<Shader, Material> item in _shaderMap) {
			Destroy (item.Value);
		}
		_shaderMap.Clear ();

		Destroy (_texTarget);
		Destroy (_texShadowTexture);

		ReleaseAllRenderTextures ();
	}

	private void OnWillRenderObject ()
	{
		shadowCamera.Render ();
		Camera.main.Render ();

		GetMaterial (lightDistanceShader).SetFloat ("_MinLuminance", minLuminance);
		GetMaterial (lightDistanceShader).SetFloat ("_ShadowOffset", shadowBias);

		GetMaterial (lightDistanceShader).SetFloat ("_Range", haloRange);
		GetMaterial (lightDistanceShader).SetFloat ("_Intensity", intensity);
		
		RenderTexture texLightDistance = PushRenderTexture (_texShadowTexture.width, _texShadowTexture.height);
		Graphics.Blit (_texShadowTexture, texLightDistance, GetMaterial (lightDistanceShader), 0);
		//Graphics.Blit (_texShadowTexture, texLightDistance);

//		Graphics.Blit(texLightDistance, _texTarget);
//		ReleaseAllRenderTextures();
//		return;

		RenderTexture texStretched = PushRenderTexture (_texShadowTexture.width, _texShadowTexture.height);
		Graphics.Blit (texLightDistance, texStretched, GetMaterial (lightDistanceShader), 1);
		//Graphics.Blit (texLightDistance, texStretched);

// 		Graphics.Blit(texStretched, _texTarget);
//		ReleaseAllRenderTextures();
//		return;

		int width = texStretched.width;
		int height = texStretched.height;

		RenderTexture texDownSampled = texStretched;
		while (width > 2) {

			width /= 2;

			RenderTexture texDownSampleTemp = PushRenderTexture (width, height);
			Graphics.Blit (texDownSampled, texDownSampleTemp, GetMaterial (lightDistanceShader), 2);
			//Graphics.Blit (texDownSampled, texDownSampleTemp);
			texDownSampled = texDownSampleTemp;
		}

//		Graphics.Blit(texDownSampled, _texTarget);
//		ReleaseAllRenderTextures();
//		return;
		Graphics.Blit (texDownSampled, _texTarget, GetMaterial (lightDistanceShader), highQuality ? 4 : 3);
		//Graphics.Blit (texDownSampled, _texTarget);
		ReleaseAllRenderTextures ();
	}

	private RenderTexture PushRenderTexture (int width, int height, int depth = 0, RenderTextureFormat format = RenderTextureFormat.Default)
	{
		RenderTexture tex = RenderTexture.GetTemporary (width, height, depth, format);
		tex.filterMode = FilterMode.Point;
		tex.wrapMode = TextureWrapMode.Clamp;
		_tempRenderTextures.Add (tex);
		return tex;
	}

	private void ReleaseAllRenderTextures ()
	{
		foreach (var item in _tempRenderTextures) {
			RenderTexture.ReleaseTemporary (item);
		}
		_tempRenderTextures.Clear ();
	}
}
