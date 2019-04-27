using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
	
	public float forwardSpeed;
	
	Rigidbody rb;
	
	void Awake() {
		rb = GetComponent<Rigidbody>();
	}
	
	void Update()
	{
		
	}
	
	void FixedUpdate()
	{
		rb.MovePosition(rb.position + (transform.forward * forwardSpeed * Time.fixedDeltaTime));
	}
}
