using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EdgeVertices {

	public Vector3 V1, V2, V3, V4, V5;

	public EdgeVertices (Vector3 corner1, Vector3 corner2) {
		V1 = corner1;
		V2 = Vector3.Lerp (corner1, corner2, 0.25f);
		V3 = Vector3.Lerp (corner1, corner2, 0.50f);
		V4 = Vector3.Lerp (corner1, corner2, 0.75f);
		V5 = corner2;
	}
	
	public EdgeVertices (Vector3 corner1, Vector3 corner2, float outerStep) {
		V1 = corner1;
		V2 = Vector3.Lerp (corner1, corner2, outerStep);
		V3 = Vector3.Lerp (corner1, corner2, 0.50f);
		V4 = Vector3.Lerp (corner1, corner2, 1f - outerStep);
		V5 = corner2;
	}

	public static EdgeVertices TerraceLerp (EdgeVertices a, EdgeVertices b, int step) {
		EdgeVertices result;
		result.V1 = VoronoiMetrics.TerraceLerp (a.V1, b.V1, step);
		result.V2 = VoronoiMetrics.TerraceLerp (a.V2, b.V2, step);
		result.V3 = VoronoiMetrics.TerraceLerp (a.V3, b.V3, step);
		result.V4 = VoronoiMetrics.TerraceLerp (a.V4, b.V4, step);
		result.V5 = VoronoiMetrics.TerraceLerp (a.V5, b.V5, step);
		return result;
	}
}
