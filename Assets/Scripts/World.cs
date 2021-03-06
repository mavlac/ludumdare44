﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	public GameManager gameManager;
	
	[Space]
	public GameObject[] obstacle;
	public float spawnDistance;
	public float spawnSpread;
	public float distanceDeltaToSpawn;
	
	
	Vector3 lastSpawnShipPos;
	
	
	void Start()
	{
		SpawnObstacle();
	}
	
	void Update()
	{
		if (Vector3.Distance(lastSpawnShipPos, gameManager.ship.transform.position) > distanceDeltaToSpawn)
			SpawnObstacle();
		
		Debug.DrawLine(
			gameManager.ship.transform.position, gameManager.ship.transform.position + gameManager.ship.transform.forward * spawnDistance,
			Color.green);
		Debug.DrawLine(
			gameManager.ship.transform.position + gameManager.ship.transform.forward * spawnDistance + gameManager.ship.transform.right * Random.Range(-spawnSpread, spawnSpread),
			gameManager.ship.transform.position + gameManager.ship.transform.forward * spawnDistance + gameManager.ship.transform.right * Random.Range(-spawnSpread, spawnSpread),
			Color.green);
	}
	
	
	
	public void SpawnObstacle()
	{
		lastSpawnShipPos = gameManager.ship.transform.position;
		
		Vector3 pos = gameManager.ship.transform.position + gameManager.ship.transform.forward * spawnDistance;
		pos += gameManager.ship.transform.right * Random.Range(-spawnSpread, spawnSpread);
		Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360f), 0);
		Instantiate(obstacle[Random.Range(0, obstacle.Length)], pos, rot);
	}
}
