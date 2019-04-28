using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScopeManager : MonoBehaviour
{
	public GameManager gameManager;
	public Camera playerCamera;
	
	[Space]
	public Image scopeMask;
	
	[Space]
	public float scopingFog;
	float defaultFog;
	public float scopingFOV;
	float defaultFOV;
	
	[HideInInspector]
	public bool scoping = false;
	
	
	void Awake()
	{
		defaultFog = RenderSettings.fogDensity;
		defaultFOV = playerCamera.fieldOfView;
	}
	
	
	
	public void Activate()
	{
		RenderSettings.fogDensity = scopingFog;
		gameManager.crosshair.HideEverything();
		scoping = true;
		
		gameManager.appManager.soundManager.Play(SoundManager.soundId.scopeIn);
		
		StopAllCoroutines();
		StartCoroutine(CameraFOVZoomCoroutine());
	}
	IEnumerator CameraFOVZoomCoroutine()
	{
		scopeMask.color = new Color(0,0,0,0);
		scopeMask.enabled = true;
		
		const float FOVchangeSpeed = 3f;
		for(float t = 0; t <= 1; t+=Time.deltaTime * FOVchangeSpeed)
		{
			playerCamera.fieldOfView =
				Mathf.Lerp(playerCamera.fieldOfView, scopingFOV, t);
			
			scopeMask.color = Color.Lerp(scopeMask.color, Color.black, t);
			
			yield return null;
		}
	}
	
	
	public void Deactivate()
	{
		RenderSettings.fogDensity = defaultFog;
		scoping = false;
		
		gameManager.appManager.soundManager.Play(SoundManager.soundId.scopeOut);
		
		StopAllCoroutines();
		StartCoroutine(CameraFOVRestoreCoroutine());
	}
	IEnumerator CameraFOVRestoreCoroutine()
	{
		const float FOVchangeSpeed = 3f;
		for(float t = 0; t <= 1; t+=Time.deltaTime * FOVchangeSpeed)
		{
			playerCamera.fieldOfView =
				Mathf.Lerp(playerCamera.fieldOfView, defaultFOV, t);
			
			scopeMask.color = Color.Lerp(scopeMask.color, new Color(0,0,0,0), t);
			
			yield return null;
		}
		
		playerCamera.fieldOfView = defaultFOV;
		scopeMask.enabled = false;
	}
}
