using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoronoiMetrics {
	
	public const int ChunkSize = 25;
	
	public const float SolidFactor = 0.8f;
	public const float BlendFactor = 1f - SolidFactor;

	public const float ElevationStep = 0.1f;
	
	public const int TerracesPerSlope = 2;
	public const int TerraceSteps = TerracesPerSlope * 2 + 1;
	public const float HorizontalTerraceStepSize = 1f / TerraceSteps;
	public const float VerticalTerraceStepSize = 1f / (TerracesPerSlope + 1);

	public static Texture2D NoiseSource;
	public const float NoiseScale = 0.05f;
	public const float CellPerturbStrength = 0.2f;
	public const float ElevationPerturbSrength = 0.05f;

	public const float StreamBedElevationOffset = -1.75f;
	public const float RiverSurfaceElevationOffset = -0.5f;
	
	public static float OuterRadius (VoronoiCell cell, VoronoiDirection direction) {
		return (cell.Corners[direction].magnitude + cell.Corners[direction + 1].magnitude) * 0.5f;
	}

	public static float InnerRadius (VoronoiCell cell, VoronoiDirection direction) {
		return Vector3.Lerp (cell.Corners[direction], cell.Corners[direction + 1], 0.5f).magnitude;
	}

	public static float OuterToInner (VoronoiCell cell, VoronoiDirection direction) {
		return InnerRadius (cell, direction) / OuterRadius (cell, direction);
	}

	public static float InnerToOuter (VoronoiCell cell, VoronoiDirection direction) {
		return 1f / OuterToInner (cell, direction);
	}
	
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

	public static VoronoiEdgeType GetEdgeType (int elevation1, int elevation2) {
		if (elevation1 == elevation2) {
			return VoronoiEdgeType.Flat;
		}

		int delta = elevation2 - elevation1;
		if (delta == 1 || delta == -1) {
			return VoronoiEdgeType.Slope;
		}

		return VoronoiEdgeType.Cliff;
	}

	public static Vector3 TerraceLerp (Vector3 a, Vector3 b, int step) {
		if (a.sqrMagnitude < b.sqrMagnitude) {
			float h = step * HorizontalTerraceStepSize;
			Vector3 r = Vector3.Slerp (a, Vector3.Project (a, b), h);
			float v = ((step + 1) / 2) * VerticalTerraceStepSize;
			r += r.normalized * (b.magnitude - a.magnitude) * v;
			return r;
		} else {
			step = TerraceSteps - step;
			float h = step * HorizontalTerraceStepSize;
			Vector3 r = Vector3.Slerp (b, Vector3.Project (b, a), h);
			float v = ((step + 1) / 2) * VerticalTerraceStepSize;
			r += r.normalized * (a.magnitude - b.magnitude) * v;
			return r;
		}
	}
	
	public static Color TerraceLerp (Color a, Color b, int step) {
		float h = step * HorizontalTerraceStepSize;
		return Color.Lerp (a, b, h);
	}

	public static Vector4 SampleNoise (Vector3 position) {
		Vector3 blendWeights = new Vector3 (Mathf.Abs (position.x), Mathf.Abs (position.y), Mathf.Abs (position.z)) - Vector3.one * 0.2f;
		blendWeights *= 7;
		blendWeights = new Vector3(Mathf.Pow (blendWeights.x, 3), Mathf.Pow (blendWeights.y, 3), Mathf.Pow (blendWeights.z, 3));
		blendWeights = new Vector3(Mathf.Max (blendWeights.x, 0), Mathf.Max (blendWeights.y, 0), Mathf.Max (blendWeights.z, 0));
		blendWeights /= Vector3.Dot (blendWeights, Vector3.one);

		Vector4 x = NoiseSource.GetPixelBilinear (position.y * NoiseScale, position.z * NoiseScale);
		Vector4 y = NoiseSource.GetPixelBilinear (position.x * NoiseScale, position.z * NoiseScale);
		Vector4 z = NoiseSource.GetPixelBilinear (position.x * NoiseScale, position.y * NoiseScale);
		
		return (x * blendWeights.x) + (y * blendWeights.y) + (z * blendWeights.z);
	}

	public static Vector3 GetSolidEdgeMiddle (VoronoiCell cell, VoronoiDirection direction) {
		return (cell.Corners[direction] + cell.Corners[direction + 1]) * (0.5f * SolidFactor);
	}
	
	public static Vector3 Perturb (Vector3 position) {
		Vector4 sample = SampleNoise (position);
		
		Vector3 perturbedPosition = position;
		perturbedPosition.x += (sample.x * 2f - 1) * CellPerturbStrength;
		perturbedPosition.y += (sample.y * 2f - 1) * CellPerturbStrength;
		perturbedPosition.z += (sample.z * 2f - 1) * CellPerturbStrength;

		return perturbedPosition.normalized * position.magnitude;
	}
}
