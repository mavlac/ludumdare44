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

	public bool smooth;
	public float smoothTime = 5f;
	
	
	public static string mouseXInputName = "Mouse X";
	public static string mouseYInputName = "Mouse Y";
	

	private Quaternion playerTargetRot;
	private Quaternion cameraTargeRot;

	private float vertAxisClamp;



	private void Awake()
	{
		gameManager.appManager.LockCursor();
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
		
		
		//Debug.Log(Input.GetAxis(mouseXInputName) + ":" + Input.GetAxis(mouseYInputName));
		
		float xRot = Input.GetAxis(mouseXInputName);
		xRot *= mouseSensitivity * Time.deltaTime;
		float yRot = Input.GetAxis(mouseYInputName);
		yRot *= mouseSensitivity * Time.deltaTime;
		
		if (invertVertical) yRot*=-1f;
		
		if (Input.GetKey(KeyCode.Q)) xRot = 40f * Time.deltaTime;
		
		vertAxisClamp += yRot;

		if(vertAxisClamp > 90.0f)
		{
			vertAxisClamp = 90.0f;
			yRot = 0.0f;
			ClampVertAxisRotationToValue(270.0f);
		}
		else if (vertAxisClamp < -90.0f)
		{
			vertAxisClamp = -90.0f;
			yRot = 0.0f;
			ClampVertAxisRotationToValue(90.0f);
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
		Vector3 eulerRotation = transform.localEulerAngles;
		eulerRotation.x = value;
		transform.localEulerAngles = eulerRotation;
	}
	
}
