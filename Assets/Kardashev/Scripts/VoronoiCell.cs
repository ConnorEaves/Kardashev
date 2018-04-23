using System.Collections.Generic;
using MIConvexHull;
using UnityEngine;

public class VoronoiCell : MonoBehaviour, IVertex {

	public List<Vector3> Corners = new List<Vector3> ();
	public List<VoronoiCell> Neighbors = new List<VoronoiCell> ();
	public List<VoronoiDirection> EdgeConnections = new List<VoronoiDirection> ();
	public List<VoronoiDirection> CornerConnections = new List<VoronoiDirection> ();

	public VoronoiGridChunk Chunk;
	
	private Color _color;
	public Color Color {
		get { return _color; }
		set {
			if (_color == value) {
				return;
			}
			_color = value;
			Refresh ();
		}
	}

	public float BaseElevation;
	
	private int _elevation = int.MinValue;
	public int Elevation {
		get { return _elevation; }
		set {
			if (_elevation == value) {
				return;
			}
			_elevation = value;
			Vector3 position = transform.localPosition;
			position = position.normalized * (BaseElevation + value * VoronoiMetrics.ElevationStep +
			                                  (VoronoiMetrics.SampleNoise (position).y * 2f - 1f) *
			                                  VoronoiMetrics.ElevationPerturbSrength);
			transform.localPosition = position;
			Refresh ();
		}
	}

	private void Refresh () {
		if (Chunk) {
			Chunk.Refresh ();

			for (int i = 0; i < Neighbors.Count; ++i) {
				VoronoiCell neighbor = Neighbors[i];
				if (neighbor.Chunk != null && neighbor.Chunk != Chunk) {
					neighbor.Chunk.Refresh ();
				}
			}
		}
	}

	// IVertex
	public double[] Position {
		get {
			return new double[] {
				transform.position.x,
				transform.position.y,
				transform.position.z
			};
		}
	}
	
	public void SetNeighbor (VoronoiCell neighbor) {
		if (!Neighbors.Contains (neighbor)) {
			Neighbors.Add (neighbor);
		}
		if (!neighbor.Neighbors.Contains (this)) {
			neighbor.Neighbors.Add (this);
		}
	}

	public VoronoiCell GetNeighbor (VoronoiDirection direction) {
		return Neighbors[direction];
	}

	public VoronoiEdgeType GetEdgeType (VoronoiDirection direction) {
		return VoronoiMetrics.GetEdgeType (_elevation, Neighbors[direction]._elevation);
	}

	public VoronoiEdgeType GetEdgeType (VoronoiCell otherCell) {
		return VoronoiMetrics.GetEdgeType (_elevation, otherCell._elevation);
	}
}
