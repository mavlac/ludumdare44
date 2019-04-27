using UnityEngine;

public class Billboard : MonoBehaviour {

	public bool freezeHorizontaly = true;

	Transform mainCameraTransform;

	Vector3 lookDirection;

	void Awake ()
	{
		mainCameraTransform = Camera.main.transform;
	}

	void Update ()
	{
		lookDirection = mainCameraTransform.position;
		
		if (freezeHorizontaly)
		{
			lookDirection.y = transform.position.y;
		}
		
		transform.LookAt(lookDirection, Vector3.up);
	}
}