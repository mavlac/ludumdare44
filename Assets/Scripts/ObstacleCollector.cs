using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollector : MonoBehaviour
{
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag("Obstacle"))
		{
			Destroy(col.gameObject);
		}
	}
}
