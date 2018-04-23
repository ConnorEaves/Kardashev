using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoronoiMetrics {
	public const float SolidFactor = 0.75f;
	public const float BlendFactor = 1f - SolidFactor;
	
	public static Vector3 GetFirstCorner (VoronoiCell cell, VoronoiDirection direction) {
		return cell.Corners[direction];
	}

	public static Vector3 GetFirstSolidCorner (VoronoiCell cell, VoronoiDirection direction) {
		return cell.Corners[direction] * SolidFactor;
	}

	public static Vector3 GetSecondCorner (VoronoiCell cell, VoronoiDirection direction) {
		return cell.Corners[direction + 1];
	}

	public static Vector3 GetSecondSolidCorner (VoronoiCell cell, VoronoiDirection direction) {
		return cell.Corners[direction + 1] * SolidFactor;
	}

	public static Vector3 GetBridge (VoronoiCell cell, VoronoiDirection direction) {
		Vector3 v1 = cell.transform.localPosition + GetFirstSolidCorner (cell, direction);
		VoronoiCell neighbor = cell.GetNeighbor (direction);
		Vector3 v2 = neighbor.transform.localPosition + GetSecondSolidCorner (neighbor, neighbor.Neighbors.IndexOf (cell));
		return v2 - v1;
	}
}
