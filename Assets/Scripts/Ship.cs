using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
	public GameManager gameManager;
	
	ShipMovement shipMovement;
	RockCradle rockCradle;
	
	bool collided = false;
	
	void Awake()
	{
		shipMovement = GetComponent<ShipMovement>();
		rockCradle = GetComponentInChildren<RockCradle>();
	}
	
	void OnCollisionEnter(Collision col)
	{
		Collision();
	}
	
	public void Collision()
	{
		// first collision
		if (!collided)
		{
			gameManager.ShipCollided();
			
			shipMovement.EndJourneyByCollision();
			rockCradle.enabled = false;
			
			collided = true;
		}
	}
}
