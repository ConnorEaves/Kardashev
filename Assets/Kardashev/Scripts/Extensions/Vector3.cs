using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions {

	public static float ClockwiseAngle (this Vector3 v, Vector3 axis) {
		Quaternion rotation = Quaternion.FromToRotation (axis, Vector3.up);
		v = rotation * v;
		return Mathf.Atan2 (v.z, v.x);
	}
	
}
