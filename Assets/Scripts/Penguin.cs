using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
	public GameManager gameManager;
	
	[Space]
	public Transform penguinBody;
	
	
	void Update()
	{
		Vector3 penguinRot;
		penguinRot = penguinBody.localEulerAngles;
		
		penguinRot.y = (180f + gameManager.playerLook.transform.localEulerAngles.y) * -1f;
		
		penguinBody.localEulerAngles = penguinRot;
	}
}
