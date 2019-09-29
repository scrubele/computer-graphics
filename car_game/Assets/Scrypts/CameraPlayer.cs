using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer: MonoBehaviour {
	public Transform PlayerTransform;
	private Vector3 _cameraOffset;
	public float cameraDistance;

	[Range(0.01f, 1.0f)]
	public float SmoothFactor = 0.5f;
	void Start() {
		_cameraOffset = transform.position - PlayerTransform.position;
		cameraDistance = 0.3f;
	}

	void Update() {
		Vector3 newPos = PlayerTransform.position + _cameraOffset;
		transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
		transform.LookAt(PlayerTransform.transform);
	}

}