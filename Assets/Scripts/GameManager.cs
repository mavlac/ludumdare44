using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
	public AppManager appManager;
	
	[Space]
	public PlayerLook playerLook;
	
	
	
	void Start () {
	}
	
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.R)) appManager.RestartCurrentScene();
		
		if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked) {
			appManager.LockCursor();
		}
		
		// escape
		if (Input.GetButtonDown("Cancel")) {
			appManager.UnlockCursor();
		}
		
	}
}
