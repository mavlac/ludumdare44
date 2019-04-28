using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
	public enum ClickAction {
		Scope,
		Nav,
		No
	}
	
	
	
	public AppManager appManager;
	public ScopeManager scopeManager;
	public Crosshair crosshair;
	
	[Space]
	public Ship ship;
	public PlayerLook playerLook;
	public CameraRig cameraRig;
	
	[Space]
	public float frontAngle = 90f;
	public float rearAngle = 30f;
	public float vertAngle = 30f;
	
	 
	[HideInInspector]
	public bool playerUsedScopeAction = false;
	[HideInInspector]
	public bool playerUsedNavAction = false;
	
	ClickAction availableAction = ClickAction.No;
	
	
	bool playerAlive = true;
	
	
	void Update () {
		
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.R)) appManager.RestartCurrentScene();
		if (Input.GetKeyDown(KeyCode.K)) ship.Collision();
		
		if (Input.GetKeyDown(KeyCode.A)) ship.shipNavigation.InitiateTurn(-1);
		if (Input.GetKeyDown(KeyCode.D)) ship.shipNavigation.InitiateTurn(1);
#endif
		
		// escape
		if (Input.GetButtonDown("Cancel")) {
			appManager.UnlockCursor();
			
			// TODO show menu
		}
		
		
		
		// following mechanics when game in progress only
		if (!playerAlive) return;
		
		// enable possible actions dependent on camera angle
		// (not while scoping)
		if (!scopeManager.scoping)
			CheckCameraAngle();
		else
			availableAction = ClickAction.No;
		
		// mouse click
		if (Input.GetMouseButtonDown(0)) {
			
			if (Cursor.lockState != CursorLockMode.Locked)
			{
				appManager.LockCursor();
			}
			else {
				switch(availableAction)
				{
					case ClickAction.Scope:
						//playerUsedScopeAction = true;
						scopeManager.Activate();
						break;
					
					case ClickAction.Nav:
						//playerUsedNavAction = true;
						break;
					
					default:
						break;
				}
			}
		}
		// mouse released
		if (!Input.GetMouseButton(0) && scopeManager.scoping) {
			scopeManager.Deactivate();
		}
	}
	
	
	public void CheckCameraAngle()
	{
		// check camera angle
		
		float horizLook = playerLook.transform.localEulerAngles.y;
		float vertLook = playerLook.playerCamera.localEulerAngles.x;
		
		if (vertLook > 360f-vertAngle/2 || vertLook < vertAngle/2)
		{
			if (horizLook > (360f-frontAngle/2) || horizLook < (frontAngle/2))
			{
				availableAction = ClickAction.Scope;
				crosshair.SetHintToScope();
			}
			else if (horizLook > (180f-rearAngle/2) && horizLook < (180+rearAngle/2))
			{
				if (!ship.shipNavigation.turning)
				{
					availableAction = ClickAction.Nav;
					crosshair.SetHintToNav();
				}
				else
				{
					// already turning in progress
					availableAction = ClickAction.No;
					crosshair.SetHintToWait();
				}
			}
			else
			{
				availableAction = ClickAction.No;
				crosshair.NoHint();
			}
		}
		else
			crosshair.NoHint();
	}
	
	
	
	
	
	
	public void ShipCollided()
	{
		playerAlive = false;
		
		cameraRig.Shake();
		ship.shipNavigation.penguin.BothWingsUp();
		
		Debug.Log("ship collision!");
		
		crosshair.HideEverything();
		if (scopeManager.scoping) scopeManager.Deactivate();
		
		// TODO end of the game
	}
}
