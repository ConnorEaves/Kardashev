using System.Collections.Generic;
using MIConvexHull;
using UnityEngine;

public partial class VoronoiCell : MonoBehaviour, IVertex {

	public List<Vector3> Corners = new List<Vector3> ();
	public List<VoronoiCell> Neighbors = new List<VoronoiCell> ();
	public List<VoronoiDirection> EdgeConnections = new List<VoronoiDirection> ();
	public List<VoronoiDirection> CornerConnections = new List<VoronoiDirection> ();

	public VoronoiGridChunk Chunk;
	
	// IVertex
	public double[] Position {
		get { return new double[] { transform.position.x, transform.position.y, transform.position.z }; }
	}

	public VoronoiCell GetNeighbor (VoronoiDirection direction) {
		return Neighbors[direction];
	}
	
	public void SetNeighbor (VoronoiCell neighbor) {
		if (!Neighbors.Contains (neighbor)) {
			Neighbors.Add (neighbor);
		}
		if (!neighbor.Neighbors.Contains (this)) {
			neighbor.Neighbors.Add (this);
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

	private void RefreshSelfOnly () {
		Chunk.Refresh ();
	}
}
