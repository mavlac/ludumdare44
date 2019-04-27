using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
	public AppManager appManager;
	
	[Space]
	public Ship ship;
	public PlayerLook playerLook;
	public CameraRig cameraRig;
	
	bool playerAlive = true;
	
	
	void Start () {
	}
	
	void Update () {
		
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.R)) appManager.RestartCurrentScene();
		if (Input.GetKeyDown(KeyCode.K)) ship.Collision();
#endif
		
		if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked) {
			appManager.LockCursor();
		}
		
		// escape
		if (Input.GetButtonDown("Cancel")) {
			appManager.UnlockCursor();
		}
		
	}
	
	
	
	
	public void ShipCollided()
	{
		playerAlive = false;
		
		cameraRig.Shake();
		
		Debug.Log("ship collision!");
		// TODO end of the game
	}
}
