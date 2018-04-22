using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class VoronoiMapCamera : MonoBehaviour {

	public VoronoiGrid Target;
	public float StickMinZoom, StickMaxZoom;
	public float SwivelMinZoom, SwivelMaxZoom;

	public float MoveSpeedMinZoom, MoveSpeedMaxZoom;
	public float RotationSpeed;
	
	private Transform _swivel, _stick;
	private float _zoom;

	private void Awake () {
		transform.localPosition = Target.transform.position + Vector3.up * Target.Radius;
		_swivel = transform.GetChild (0);
		_stick = _swivel.GetChild (0);
		AdjustZoom (1f);
	}

	private void Update () {
		float zoomDelta = Input.GetAxis ("Mouse ScrollWheel");
		if (zoomDelta != 0) {
			AdjustZoom (zoomDelta);
		}

		float rotationDelta = Input.GetAxis ("Rotation");
		if (rotationDelta != 0f) {
			AdjustRotation (rotationDelta);
		}

		float xDelta = Input.GetAxis ("Horizontal");
		float zDelta = Input.GetAxis ("Vertical");
		if (xDelta != 0f || zDelta != 0f) {
			AdjustPosition (xDelta, zDelta);
		}
	}

	private void AdjustZoom (float delta) {
		_zoom = Mathf.Clamp01 (_zoom + delta);

		float distance = Mathf.Lerp (StickMinZoom, StickMaxZoom, _zoom);
		_stick.localPosition = new Vector3 (0f, 0f, distance);

		float angle = Mathf.Lerp (SwivelMinZoom, SwivelMaxZoom, _zoom);
		_swivel.localRotation = Quaternion.Euler (angle, 0f, 0f);
	}

	private void AdjustPosition (float xDelta, float zDelta) {
		Vector3 direction = new Vector3 (xDelta, 0f, zDelta).normalized;
		float damping = Mathf.Max (Mathf.Abs (xDelta), Mathf.Abs (zDelta));
		float distance = Mathf.Lerp (MoveSpeedMinZoom, MoveSpeedMaxZoom, _zoom) / Target.Radius * damping * Time.deltaTime;
		transform.RotateAround (Target.transform.position, transform.forward, -direction.x * distance);
		transform.RotateAround (Target.transform.position, transform.right, direction.z * distance);
		transform.position = (transform.position - Target.transform.position).normalized * Target.Radius;
	}

	private void AdjustRotation (float delta) {
		float rotationAngle = delta * RotationSpeed * Time.deltaTime;
		transform.Rotate (transform.up, rotationAngle, Space.World);
	}
}
