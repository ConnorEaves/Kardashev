using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EdgeVertices {

	public Vector3 V1, V2, V3, V4;

	public EdgeVertices (Vector3 corner1, Vector3 corner2) {
		V1 = corner1;
		V2 = Vector3.Lerp (corner1, corner2, 1 / 3f);
		V3 = Vector3.Lerp (corner1, corner2, 2 / 3f);
		V4 = corner2;
	}

	public static EdgeVertices TerraceLerp (EdgeVertices a, EdgeVertices b, int step) {
		EdgeVertices result;
		result.V1 = VoronoiMetrics.TerraceLerp (a.V1, b.V1, step);
		result.V2 = VoronoiMetrics.TerraceLerp (a.V2, b.V2, step);
		result.V3 = VoronoiMetrics.TerraceLerp (a.V3, b.V3, step);
		result.V4 = VoronoiMetrics.TerraceLerp (a.V4, b.V4, step);
		return result;
	}
}
