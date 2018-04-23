using System.Collections.Generic;
using MIConvexHull;
using UnityEngine;

public class VoronoiCell : MonoBehaviour, IVertex {

	public List<Vector3> Corners = new List<Vector3> ();
	public List<VoronoiCell> Neighbors = new List<VoronoiCell> ();
	public List<VoronoiDirection> EdgeConnections = new List<VoronoiDirection> ();
	public List<VoronoiDirection> CornerConnections = new List<VoronoiDirection> ();

	public Color Color = Color.white;

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
	
	
}
