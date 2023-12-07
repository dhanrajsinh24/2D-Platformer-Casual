using UnityEngine;
using System.Collections;

public class CameraUpwords : MonoBehaviour {

	private float cameraSpeed = 1.5f;
	private readonly float maxCameraSpeed = 5f;
	private Vector3 currentV;

	private void Start() {
		//InvokeRepeating("UpdateSpeed", 5f, 5f);
	}

	private void UpdateSpeed() {
		cameraSpeed *= 1.05f;
		//Debug.Log("cameraSpeed : "+cameraSpeed);
		if(cameraSpeed >= maxCameraSpeed) {
			cameraSpeed = maxCameraSpeed;
			CancelInvoke("UpdateSpeed");
		} 
			
	}

	private void Update() {
		//transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * cameraSpeed, transform.position.z);
	}
}
