using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCradle : MonoBehaviour
{
	public float speedX, speedZ;
	public float amount;
	
	
	Quaternion initialRotation;
	
	void Awake()
	{
		initialRotation = transform.localRotation;
	}
	
	void Update()
	{
		Vector3 rot = transform.localRotation.eulerAngles;
		
		Vector3 rotDelta = Vector3.zero;
		rotDelta.x = Mathf.Sin(Time.time * speedX) * amount;
		rotDelta.z = Mathf.Cos(Time.time * speedZ) * amount;
		
		transform.localRotation = initialRotation * Quaternion.Euler(rotDelta);
	}
}
