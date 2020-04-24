using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
	public enum ClickAction {
		Scope,
		Nav,
		NavigateLeft,
		NavigateRight,
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
	bool navSelection = false;
	float navSelectionPlayerYOrigin;
	
	
	bool playerAlive = true;
	
	
	void Update () {
		
		// only game mechanics
		if (appManager.appState != AppManager.AppState.Game) return;
		
		
		
		
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.R)) appManager.RestartCurrentScene();
		if (Input.GetKeyDown(KeyCode.K)) ship.Collision();
		
		if (Input.GetKeyDown(KeyCode.A)) ship.shipNavigation.InitiateTurn(-1);
		if (Input.GetKeyDown(KeyCode.D)) ship.shipNavigation.InitiateTurn(1);
#endif
		
		// escape
		if (Input.GetButtonDown("Cancel"))
		{
			
			if (playerAlive)
			{
				crosshair.HideEverything();
				
				appManager.UnlockCursor();
				appManager.FreezeTimescale();
				appManager.MenuEntered();
				
				appManager.soundManager.Play(SoundManager.soundId.uiClick);
				return;
			}
			else
			{
				// ESC when game over: quit to title
				
				//appManager.UnlockCursor();
				//appManager.RestartCurrentScene();
				
				// now as a callback of fade
				appManager.screenFader.FadeScreenOut(ScreenFader.FadeOutCallback.RestartScene);
			}
		}
		
		// mouse sensitivity
		if (Input.GetKey(KeyCode.Alpha1)) playerLook.MouseSensitivity = 33f;
		if (Input.GetKey(KeyCode.Alpha2)) playerLook.MouseSensitivity = 66f;
		if (Input.GetKey(KeyCode.Alpha3)) playerLook.MouseSensitivity = 100f;
		if (Input.GetKey(KeyCode.Alpha4)) playerLook.MouseSensitivity = 133f;
		if (Input.GetKey(KeyCode.Alpha5)) playerLook.MouseSensitivity = 166f;
		
		
		
		
		// following mechanics when game in progress only
		if (!playerAlive) return;
		
		
		// enable possible actions dependent on camera angle
			// (not while scoping)
		if (scopeManager.scoping)
			availableAction = ClickAction.No;
		else
			PickMouseActionByCamAngle();
		
		
		// mouse click
		if (Input.GetMouseButtonDown(0)) {
			
			if (Cursor.lockState != CursorLockMode.Locked)
			{
				appManager.LockCursor();
				appManager.RestoreTimescale();
			}
			else {
				switch(availableAction)
				{
					case ClickAction.Scope:
						//playerUsedScopeAction = true; // to hide hint every next time - abandoned
						scopeManager.Activate();
						break;
					
					case ClickAction.Nav:
						//playerUsedNavAction = true;
						BeginNavSelection();
						break;
						
					case ClickAction.NavigateLeft:
						ship.shipNavigation.InitiateTurn(-1);
						EndNavSelection();
						break;
					
					case ClickAction.NavigateRight:
						ship.shipNavigation.InitiateTurn(1);
						EndNavSelection();
						break;
					
					case ClickAction.No:
						if (navSelection) EndNavSelection();
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
	
	
	public void PickMouseActionByCamAngle()
	{
		// check camera angle
		
		float horizLook = playerLook.transform.localEulerAngles.y;
		float vertLook = playerLook.playerCamera.localEulerAngles.x;
		
		// vertical bounds
		if (vertLook > 360f-vertAngle/2 || vertLook < vertAngle/2)
		{
			// front
			if (horizLook > (360f-frontAngle/2) || horizLook < (frontAngle/2))
			{
				availableAction = ClickAction.Scope;
				crosshair.SetHintToScope();
			}
			// rear
			else if (horizLook > (180f-rearAngle/2) && horizLook < (180+rearAngle/2))
			{
				// nav selection in progress
				// decide direction to turn
				if (navSelection)
				{
					const float selDeadZone = 3f;
					// option of selection
					if (playerLook.transform.localEulerAngles.y < navSelectionPlayerYOrigin - selDeadZone)
					{
						if (availableAction == ClickAction.No)
							appManager.soundManager.Play(SoundManager.soundId.uiClick);
						
						crosshair.SetHintToNavOptions(-1);
						availableAction = ClickAction.NavigateLeft;
					}
					else if (playerLook.transform.localEulerAngles.y > navSelectionPlayerYOrigin + selDeadZone)
					{
						if (availableAction == ClickAction.No)
							appManager.soundManager.Play(SoundManager.soundId.uiClick);
						
						crosshair.SetHintToNavOptions(1);
						availableAction = ClickAction.NavigateRight;
					}
					else
					{
						crosshair.SetHintToNavOptions(0);
						availableAction = ClickAction.No;
					}
				}
				// turning already in progress
				else if (ship.shipNavigation.turning)
				{
					availableAction = ClickAction.No;
					crosshair.SetHintToWait();
				}
				// turn available
				else
				{
					availableAction = ClickAction.Nav;
					crosshair.SetHintToNav();
				}
			}
			else
			{
				availableAction = ClickAction.No;
				crosshair.NoHint();
				
				// cancel nav selection on looking outside of rear area
				if (navSelection)
				{
					navSelection = false;
					
					appManager.soundManager.Play(SoundManager.soundId.noAction);
				}
			}
		}
		else
			crosshair.NoHint();
	}
	
	
	
	
	void BeginNavSelection()
	{
		if (navSelection) return;
		
		appManager.soundManager.Play(SoundManager.soundId.navSelect);
		
		navSelectionPlayerYOrigin = playerLook.transform.localEulerAngles.y;
		navSelection = true;
	}
	void EndNavSelection()
	{
		if (navSelection) navSelection = false;
	}
	
	
	
	
	
	public void ShipCollided()
	{
		playerAlive = false;
		
		cameraRig.Shake();
		ship.shipNavigation.penguin.BothWingsUp();
		
		Debug.Log("ship collision!");
		appManager.soundManager.Play(SoundManager.soundId.crash);
		
		crosshair.SetHintToGameOver();
		if (scopeManager.scoping) scopeManager.Deactivate();
		
		// TODO end of the game
	}
}
