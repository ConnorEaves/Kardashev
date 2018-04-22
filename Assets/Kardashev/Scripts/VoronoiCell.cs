using System.Collections.Generic;
using MIConvexHull;
using UnityEngine;

public class VoronoiCell : MonoBehaviour, IVertex {

	public List<Vector3> Corners = new List<Vector3> ();
	public List<VoronoiCell> Neighbors = new List<VoronoiCell> ();
	public List<VoronoiCell> EdgeConnections = new List<VoronoiCell> ();
	public List<Vector3> CornerConnections = new List<Vector3> ();

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
			EdgeConnections.Add (neighbor);
		}
		if (!neighbor.Neighbors.Contains (this)) {
			neighbor.Neighbors.Add (this);
		}
	}
	
	public void SetCornerConnection (Vector3 corner) {
		if (!CornerConnections.Contains (corner)) {
			CornerConnections.Add (corner);
		}
	}
}
