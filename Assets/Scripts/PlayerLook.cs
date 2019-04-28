using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
	public GameManager gameManager;
	public Transform playerBody;
	public Transform playerCamera;
	
	public float mouseSensitivity;
	public bool invertVertical = true;
	
	public float clampY = 90f;
	
	public bool smooth;
	public float smoothTime = 5f;
	
	
	public static string mouseXInputName = "Mouse X";
	public static string mouseYInputName = "Mouse Y";
	

	private Quaternion playerTargetRot;
	private Quaternion cameraTargeRot;

	private float vertAxisClamp;
	private float mouseSensitivityMultiplier;



	void Awake()
	{
		GetInvert();
	}

	void Start() {
		playerTargetRot = playerBody.localRotation;
		cameraTargeRot = playerCamera.localRotation;
		vertAxisClamp = 0.0f;
	}
	
	private void Update()
	{
		CameraRotation();
	}
	
	private void CameraRotation()
	{
		// no mouselook on unlocked mouse
		if (Cursor.lockState != CursorLockMode.Locked) return;
		if (!AppManager.mouselookReady) return;
		
		// mouse half sensitive on scoping
		if (gameManager.scopeManager.scoping)
			mouseSensitivityMultiplier = 0.25f;
		else
			mouseSensitivityMultiplier = 1f;
		
		
		//Debug.Log(Input.GetAxis(mouseXInputName) + ":" + Input.GetAxis(mouseYInputName));
		
		float xRot = Input.GetAxis(mouseXInputName);
		xRot *= mouseSensitivity * mouseSensitivityMultiplier * Time.deltaTime;
		float yRot = Input.GetAxis(mouseYInputName);
		yRot *= mouseSensitivity * mouseSensitivityMultiplier * Time.deltaTime;
		
		if (invertVertical) yRot*=-1f;
		
		if (Input.GetKey(KeyCode.Q)) xRot = 40f * Time.deltaTime;
		
		vertAxisClamp += yRot;

		if(vertAxisClamp > clampY)
		{
			vertAxisClamp = clampY;
			yRot = 0.0f;
			ClampVertAxisRotationToValue(360f - clampY);
		}
		else if (vertAxisClamp < -clampY)
		{
			vertAxisClamp = -clampY;
			yRot = 0.0f;
			ClampVertAxisRotationToValue(clampY);
		}

		
		playerTargetRot *= Quaternion.Euler (0f, xRot, 0f);
		cameraTargeRot *= Quaternion.Euler (-yRot, 0f, 0f);
		
		if (smooth)
		{
			playerBody.localRotation =
				Quaternion.Slerp(playerBody.localRotation, playerTargetRot, smoothTime * Time.deltaTime);
			playerCamera.localRotation =
				Quaternion.Slerp(playerCamera.localRotation, cameraTargeRot, smoothTime * Time.deltaTime);
		}
		else
		{
			playerBody.localRotation = playerTargetRot;
			playerCamera.localRotation = cameraTargeRot;
		}
	}

	private void ClampVertAxisRotationToValue(float value)
	{
		Vector3 eulerRotation = playerCamera.localEulerAngles;
		eulerRotation.x = value;
		
		playerCamera.localEulerAngles = eulerRotation;
	}
	
	
	
	
	public void GetInvert()
	{
		invertVertical = ((PlayerPrefs.GetInt("pref_mouseinvert", 1) == 1) ? true : false);
	}
	public void SetInvert(bool newValue)
	{
		PlayerPrefs.SetInt("pref_mouseinvert", newValue ? 1 : 0);
		invertVertical = newValue;
		
		Debug.Log("mouse invert set to: " + invertVertical);
	}
}
