using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipNavigation : MonoBehaviour
{
	public GameManager gameManager;
	public HelmWheel helmWheel;
	public Penguin penguin;
	
	[Space]
	public float turnSpeed;
	public float turnDuration;
	
	[HideInInspector]
	public bool turning;
	
	
	public void InitiateTurn(int dir)
	{
		if (turning) return;
		
		
		helmWheel.Turn(dir);
		penguin.WaveWing(dir);
		
		StopAllCoroutines();
		StartCoroutine(TurnCoroutine(dir));	
		turning = true;
	}
	IEnumerator TurnCoroutine(int dir) 
	{
		float startTime = Time.time;
		float midTime = startTime + (turnDuration / 2);
		float endTime = startTime + turnDuration;
		
		float currentTurnSpeed = 0f;
		
		do {
			
			if (Time.time < midTime)
			{
				// turn attack phase
				float t = Mathf.InverseLerp(startTime, midTime, Time.time);
				currentTurnSpeed = Mathf.Lerp(0, turnSpeed, t);
			}
			else
			{
				// turn release phase
				float t = Mathf.InverseLerp(midTime, endTime, Time.time);
				currentTurnSpeed = Mathf.Lerp(turnSpeed, 0f, t);
			}
			
			Quaternion newShipRot = gameManager.ship.shipMovement.rb.rotation;
			newShipRot *= Quaternion.Euler(Vector3.down * currentTurnSpeed * dir);
			gameManager.ship.shipMovement.rb.MoveRotation(newShipRot);
			
			yield return null;
			
		} while (Time.time < endTime);
		
		
		turning = false;
	}
}
